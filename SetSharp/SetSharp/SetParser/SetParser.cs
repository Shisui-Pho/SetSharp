/*
 * File: SetParser.cs
 * Author: Phiwokwakhe Khathwane
 * Date: 12 July 2026
 *
 * Description:
 * Provides a small, focused wrapper that uses the lexical Tokenizer to produce
 * a token stream for a set-expression string and exposes an API to build an
 * ISetTree<T> from that stream.
 *
 * Notes:
 * - This class is intentionally lightweight: tokenization and low-level parsing
 *   responsibilities are delegated to `Tokenizer` and, eventually, to a parser
 *   that will consume the token queue and construct the set tree.
 * - The current implementation handles the trivial "empty input" case and
 *   otherwise delegates to a future parsing implementation (NotImplementedException).
 * - Documentation explains expected behavior and the contract for `BuildSetTree`.
 */
using SetSharp.Collections;

namespace SetSharp.SetParser;
/// <summary>
/// Orchestrates tokenization and parsing of a set expression into an <see cref="ISetTree{T}"/>.
/// </summary>
/// <typeparam name="T">Element type stored in the set tree. Must implement <see cref="IComparable{T}"/>.</typeparam>
public class SetParser<T> where T : IComparable<T>
{
    private readonly SetsConfigurations _extractionConfig;
    private readonly Tokenizer _tokenizer;

    /// <summary>
    /// Initializes a new instance of <see cref="SetParser{T}"/>.
    /// </summary>
    /// <param name="expression">The string expression to parse. May include nested sets using '{' and '}'.</param>
    /// <param name="extractionConfig">Configuration that provides row/field terminators and parsing options.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="expression"/> or <paramref name="extractionConfig"/> is null.</exception>
    public SetParser(string expression, SetsConfigurations extractionConfig)
    {
        ArgumentNullException.ThrowIfNull(expression, nameof(expression));
        ArgumentNullException.ThrowIfNull(extractionConfig, nameof(extractionConfig));

        this._extractionConfig = extractionConfig;
        _tokenizer = new Tokenizer(expression, extractionConfig);
    }

    /// <summary>
    /// Builds the <see cref="ISetTree{T}"/> representation of the provided expression.
    /// </summary>
    /// <returns>
    /// A populated <see cref="ISetTree{T}"/> representing the parsed structure of the expression.
    /// For an empty expression (only an <see cref="TokenType.END_OF_LINE"/> token), an empty set tree is returned.
    /// </returns>
    /// <remarks>
    /// Contract and expected behavior:
    /// - The tokenizer produces a <see cref="Queue{Token}"/> including tokens for braces, terminators,
    ///   element payloads and a final <see cref="TokenType.END_OF_LINE"/> token.
    /// - If the token stream contains only the <see cref="TokenType.END_OF_LINE"/> token this method
    ///   returns an empty <see cref="SetTree{T}"/> (no root elements and no subsets).
    /// - For non-empty streams the parser must consume tokens and construct the tree. That parser is not
    ///   implemented yet; as a result this method will throw <see cref="NotImplementedException"/> for
    ///   non-trivial inputs until parsing is implemented.
    /// - Implementors should ensure the produced tree respects the configured <see cref="SetsConfigurations"/>
    ///   (field/row terminators, custom object conversion, trimming/escaping rules).
    /// </remarks>
    /// <exception cref="SetsException">May be thrown by the tokenizer when lexical issues are encountered.</exception>
    public ISetTree<T> BuildSetTree()
    {
        var tokens = _tokenizer.Tokenize();

        return ParseSet(tokens, true, true);
    }
    private SetTree<T> ParseSet(Queue<Token> tokens, bool expectClosingBrace, bool isRoot = false)
    {
        SortedElements<T> elements = [];
        SortedSubSets<T> subsets = [];
        bool hasEmptySets = false;
        int countEmptySets = 0;
        while(tokens.Peek().TokenType != TokenType.END_OF_LINE)
        {
            var token = tokens.Peek();

            switch (token.TokenType)
            {
                case TokenType.OPENING_BRACE:
                    //Consume token
                    tokens.Dequeue();

                    //Only consider it a subset if we're not looking at root
                    if (!isRoot)
                    {
                        var subTree = ParseSet(tokens, true);
                        subsets.Add(subTree);
                        hasEmptySets = subTree.TreeInfo.IsEmptyTree;
                        countEmptySets++;
                    }
                    isRoot = false;//Second brace is not longer root
                    break;


                case TokenType.CLOSING_BRACE:

                    if (!expectClosingBrace)
                        throw new MissingBraceException("Did not expect a closing brace",
                            $"""
                            A closing brace was detected at position {token.Position} of the expression.
                            """);

                    tokens.Dequeue();

                    return new SetTree<T>(_extractionConfig, elements, subsets);

                case TokenType.ELEMENT:
                    //consume token
                    tokens.Dequeue();

                    var value = ConvertElement(token.Value!);

                    if(value == null)
                    {
                        hasEmptySets = true;
                        countEmptySets++;
                    }
                    else
                    {
                        elements.Add(value);
                    }
                    break;

                case TokenType.ROW_TERMINATOR or TokenType.END_OF_LINE or TokenType.FIELD_TERMINATOR:
                    //Just consume those tokens
                    tokens.Dequeue();
                    break;
            }
        }

        if (expectClosingBrace)
            throw new MissingBraceException("Expected closing brace.", $"Expression ended with a missing closing brace at position {tokens.Peek().Position}");


        var tree = new SetTree<T>(_extractionConfig, elements, subsets);
        tree.WillHaveNullElements(hasEmptySets, countEmptySets);
        return tree;
    }

    private T? ConvertElement(string value)
    {
        try
        {
            if (_extractionConfig.IsICustomObject)
            {
               return ConvertToCustomType(value);
            }
            else
            {
                return ConvertToPrimitiveType(value);
            }
        }
        catch(Exception ex)
        {
            string det = $"Failed to convert the string \'{value}\' to type of \'{typeof(T)}\'";
            throw new SetsException("Conversion failed due to invalid format", det, ex);
        }
    }
    private T? ConvertToCustomType(string value)
    {
        var fields = ExtractFields(value, out bool isEmpty);

        if (fields.Length == 0 || isEmpty) return default(T);

        if (fields is null) throw new Exception();

        if (_extractionConfig is CustomSetsConfigurations<T> converterConfig)
        {
            var converter = converterConfig.Funct_ToObject;

            return converter == null ? throw new SetsException("Expected custom sets to have a converter.") : converter(fields);
        }

        throw new SetsException("Expected custom sets to have a converter.");
    }
    private static T? ConvertToPrimitiveType(string value)
    {
        if (value is null)
            return default(T);

        //Trim sting to remove white spaces
        if (typeof(T) != typeof(string))
        {
            value = value.Trim();
        }

        if (typeof(T) != typeof(string) && string.IsNullOrEmpty(value))
            return default(T);

        //ConvertToObject to the specified type T
        var item = (T)Convert.ChangeType(value, typeof(T));

        return item;
    }
    private string?[] ExtractFields(string element, out bool isEmpty)
    {
        string?[] fields = element.Split(_extractionConfig.FieldTerminator);

        //Check if all the fields are empty
        int countEmptyFields = 0;
        for (int i = 0; i < fields.Length; i++)
        {
            fields[i] = fields[i]?.Trim();
            if (string.IsNullOrWhiteSpace(fields[i]))
            {
                //Replace with a null
                fields[i] = null;
                countEmptyFields++;
            }
        }

        isEmpty = countEmptyFields == fields.Length;

        return fields;
    }
}