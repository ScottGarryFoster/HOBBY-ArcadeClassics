namespace FQ.Log
{
    /// <summary>
    /// Result of a Logger.
    /// </summary>
    public enum Result
    {
        /// <summary>
        /// Simply logging info.
        /// </summary>
        Info,
        
        /// <summary>
        /// Something incorrect occured but operation may continue.
        /// </summary>
        Warning,
        
        /// <summary>
        /// Something incorrect occured and affected behaviour.
        /// </summary>
        Error,
    }
}