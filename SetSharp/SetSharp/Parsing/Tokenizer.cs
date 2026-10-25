/*
 * File: Tokenizer.cs
 * Author: Phiwokwakhe Khathwane
 * Date: 12 July 2026
 *
 * Description:
 * Tokenizer turns a set-expression string into a stream (queue) of lexical tokens
 * consumed by the parser. The tokenizer is aware of the configured field and row
 * terminators and must emit tokens for braces, terminators, element payloads and
 * a final end-of-line token. This file provides documentation for the class and
 * its public surface; the tokenization algorithm itself is intentionally left
 * unimplemented here and will throw a NotImplementedException until implemented.
 */
using System.Text;

namespace SetSharp;
/// <summary>
/// Produces lexical <see cref="Token"/> instances from a textual set expression.
/// </summary>
/// <remarks>
/// The tokenizer is responsible for converting the raw input expression into an
/// ordered <see cref="Queue{Token}"/> of tokens. Tokens emitted should include:
/// - <see cref="TokenType.OPENING_BRACE"/> and <see cref="TokenType.CLOSING_BRACE"/>
///   for '{' and '}' characters.
/// - <see cref="TokenType.FIELD_TERMINATOR"/> and <see cref="TokenType.ROW_TERMINATOR"/>
///   for the configured terminators (which may be multi-character strings).
/// - <see cref="TokenType.ELEMENT"/> carrying the textual payload of an element/record.
/// - <see cref="TokenType.END_OF_LINE"/> to mark end of the input stream.
///
/// Token positions must be zero-based indices into the original expression and are
/// intended for diagnostics and error reporting. For multi-character terminators the
/// token's <see cref="Token.Value"/> should contain the full terminator string.
/// For single-character brace tokens the <see cref="Token.Value"/> may be null.
/// </remarks>
public sealed class Tokenizer
{
    private readonly string expression;
    private readonly SetsConfigurations configurations;

    /// <summary>
    /// Initializes a new instance of the <see cref="Tokenizer"/> class.
    /// </summary>
    /// <param name="expression">The source expression to tokenize. Must not be null.</param>
    /// <param name="configurations">Extraction configurations that provide field/row terminators.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="expression"/> or <paramref name="configurations"/> is null.</exception>
    public Tokenizer(string expression, SetsConfigurations configurations)
    {
        ArgumentNullException.ThrowIfNull(expression, nameof(expression));
        ArgumentNullException.ThrowIfNull(configurations, nameof(configurations));

        this.expression = expression;
        this.configurations = configurations;
    }

    /// <summary>
    /// Tokenizes the configured input expression and returns an ordered queue of tokens.
    /// </summary>
    /// <returns>
    /// A non-null <see cref="Queue{Token}"/> containing the lexical tokens.
    /// The queue always ends with an <see cref="TokenType.END_OF_LINE"/> token.
    /// </returns>
    /// <remarks>
    /// Implementation details:
    /// - Emits <see cref="TokenType.OPENING_BRACE"/> and <see cref="TokenType.CLOSING_BRACE"/>
    ///   for brace characters.
    /// - Recognizes the configured row terminator string and emits a single
    ///   <see cref="TokenType.ROW_TERMINATOR"/> token for it.
    /// - Accumulates characters into an element buffer and emits a trimmed
    ///   <see cref="TokenType.ELEMENT"/> token when encountering a structural token
    ///   (brace or row terminator) or at end of input.
    /// - Supports escaping with `\`. Supported escapes: `{`, `}`, `\`, and the row terminator.
    /// - Positions on tokens reflect the index in the original expression where the token
    ///   was emitted (start index for structural tokens; flush index for elements).
    /// </remarks>
    public Queue<Token> Tokenize()
    {
        Queue<Token> tokens = [];

        int expressionLength = expression.Length;
        int currentIndex = 0;

        StringBuilder currentElement = new();

        while (currentIndex < expressionLength)
        {
            var token = expression[currentIndex];

            //if escaping
            if (token == '\\')
            {
                if ((currentIndex + 1) >= expressionLength)
                {
                    throw new SetsException("Escape character at the end of expression.");
                }

                if(!IsEscapableSequence(currentIndex))
                {
                    currentElement.Append('\\');
                    currentIndex++;
                    continue;
                }

                currentIndex++; //Consume escape character

                if(IsRowTerminator(currentIndex))
                {
                    //Consume escaped characters
                    currentElement.Append(configurations.RowTerminator);
                    currentIndex += configurations.RowTerminator.Length;
                }
                else
                {
                    currentElement.Append(expression[currentIndex]);
                    currentIndex++;
                }

                continue;
            }

            //If opening brace
            if (token == '{')
            {
                FlushElement(tokens, currentElement, currentIndex);
                tokens.Enqueue(new Token(TokenType.OPENING_BRACE, null, currentIndex));
                currentIndex++;
                continue;
            }

            //if closing brace
            if(token == '}')
            {
                FlushElement(tokens, currentElement, currentIndex);
                tokens.Enqueue(new Token(TokenType.CLOSING_BRACE, null, currentIndex));
                currentIndex++;
                continue;
            }

            if(IsRowTerminator(currentIndex))
            {
                FlushElement(tokens, currentElement, currentIndex);
                tokens.Enqueue(new Token(TokenType.ROW_TERMINATOR, null, currentIndex));
                currentIndex += configurations.RowTerminator.Length;
                continue;
            }

            //Consume element normally
            currentElement.Append(token);
            currentIndex++;
        }
        FlushElement(tokens, currentElement, currentIndex);
        tokens.Enqueue(new Token(TokenType.END_OF_LINE, null, currentIndex));
        return tokens;
    }

    private static void FlushElement(Queue<Token> tokens, StringBuilder builder, int position)
    {
        string value = builder.ToString().Trim();
        
        if(value.Length > 0 )
        {
            tokens.Enqueue(new Token(TokenType.ELEMENT, value, position));
        }

        builder.Clear();
    }

    private bool IsRowTerminator(int index)
    {
        if(index + configurations.RowTerminator.Length > expression.Length)
            return false;

        return expression.AsSpan(index).StartsWith(configurations.RowTerminator);
    }
    private bool IsEscapableSequence(int index)
    {
        if (index + 1 >= expression.Length)
            return false;

        char next = expression[index + 1];

        return next == '{' || next == '}'|| next == '\\' || IsRowTerminator(index + 1);
    }
}