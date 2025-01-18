using System;

namespace Imast.Yagen.Cli;

/// <summary>
/// The yagen exception definition
/// </summary>
public class YagenException : Exception
{
    /// <summary>
    /// Creates new yagen exception
    /// </summary>
    /// <param name="message">The error message</param>
    /// <param name="innerException">The inner exception</param>
    public YagenException(string message = null, Exception innerException = null) : base(message, innerException)
    {

    }
}