namespace SetsLibrary.Utility
{
    /// <summary>
    /// Enum representing the possible types of missing braces in a code structure.
    /// </summary>
    public enum MissingBrace
    {
        /// <summary>
        /// No missing braces.
        /// </summary>
        None = 0,

        /// <summary>
        /// Missing an opening brace.
        /// </summary>
        Openning = 1,

        /// <summary>
        /// Missing a closing brace.
        /// </summary>
        Clossing = 2,

        /// <summary>
        /// Missing both the opening and closing braces.
        /// </summary>
        Both = 4
    }
}
