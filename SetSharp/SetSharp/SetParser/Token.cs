/*
 * File: Token.cs
 * Author: Phiwokwakhe Khathwane
 * Date: 11 July 2026
 *
 * Description:
 * Represents a lexical token produced by the SetSharp tokenizer/parser.
 * Tokens are emitted during lexical analysis and consumed by the parser to
 * build the set tree. This record is intentionally compact and immutable.
 *
 * Key Notes:
 * - `TokenType` indicates the kind of token (brace, terminator, element, or end).
 * - `Value` contains the textual payload for tokens that carry data (for example
 *   an `ELEMENT` token or when terminators are multi-character strings). It will
 *   be null for tokens that do not carry a payload (for example single-character braces).
 * - `Position` is a zero-based index pointing to the start position of the token
 *   in the original input string. Useful for diagnostics and error messages.
 */
namespace SetSharp;
/// <summary>
/// A compact, immutable representation of a lexical token produced by the tokenizer.
/// </summary>
/// <param name="TokenType">The kind of token (see <see cref="TokenType"/>).</param>
/// <param name="Value">
/// The textual payload associated with the token, if any.
/// For example:
/// - ELEMENT: the element text (may be trimmed according to configuration),
/// - ROW_TERMINATOR/FIELD_TERMINATOR: the terminator string (when multi-character),
/// - OPENING_BRACE/CLOSING_BRACE/END_OF_LINE: typically <c>null</c>.
/// </param>
/// <param name="Position">Zero-based index in the source input where this token begins.</param>
public record Token(TokenType TokenType, string? Value, int Position);