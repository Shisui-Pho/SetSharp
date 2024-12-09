namespace SetsLibrary;

internal class InvalidBracesException : Exception
{
    public InvalidBracesException(string message)
        : base(message)
    { }
    public InvalidBracesException(string message, Exception innerException, string expression) 
        : base($"{message}. Invalid expression: {expression}", innerException)
    { }
}//class
//namespace