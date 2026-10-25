namespace SetSharp;
/// <summary>
/// Represents the kinds of tokens emitted by the tokenizer used by the set parser.
/// </summary>
public enum TokenType
{   
    /// <summary>
    /// Token for an opening brace ('{') that begins a set or subset.
    /// </summary>
    OPENING_BRACE,

    /// <summary>
    /// Token for a closing brace ('}') that terminates a set or subset.
    /// </summary>
    CLOSING_BRACE,

    /// <summary>
    /// Token representing the configured row terminator (separates rows/records).
    /// </summary>
    ROW_TERMINATOR,

    /// <summary>
    /// Token representing the configured field terminator (separates fields within a record).
    /// </summary>
    FIELD_TERMINATOR,

    /// <summary>
    /// Token containing an element's textual payload (primitive value, string literal or nested set text).
    /// </summary>
    ELEMENT,

    /// <summary>
    /// Token that marks the end of the input/line stream.
    /// </summary>
    END_OF_LINE
}
