namespace FQ
{
    /// <summary>
    /// Logging out information
    /// </summary>
    public interface ILog
    {
        /// <summary>
        /// Logs info.
        /// </summary>
        /// <param name="message">Message to log. </param>
        void Info(string message);
        
        /// <summary>
        /// Logs info.
        /// </summary>
        /// <param name="message">Message to log. </param>
        /// <param name="origin">Origin of the log. </param>
        void Info(string message, string origin);
        
        /// <summary>
        /// Logs info.
        /// </summary>
        /// <param name="message">Message to log. </param>
        /// <param name="origin">Origin of the log. </param>
        /// <param name="subject">Subject which caused the log. </param>
        void Info(string message, string origin, string subject);
        
        /// <summary>
        /// Logs warning.
        /// </summary>
        /// <param name="message">Message to log. </param>
        void Warning(string message);
        
        /// <summary>
        /// Logs warning.
        /// </summary>
        /// <param name="message">Message to log. </param>
        /// <param name="origin">Origin of the log. </param>
        void Warning(string message, string origin);
        
        /// <summary>
        /// Logs warning.
        /// </summary>
        /// <param name="message">Message to log. </param>
        /// <param name="origin">Origin of the log. </param>
        /// <param name="subject">Subject which caused the log. </param>
        void Warning(string message, string origin, string subject);
        
        /// <summary>
        /// Logs error.
        /// </summary>
        /// <param name="message">Message to log. </param>
        void Error(string message);
        
        /// <summary>
        /// Logs error.
        /// </summary>
        /// <param name="message">Message to log. </param>
        /// <param name="origin">Origin of the log. </param>
        void Error(string message, string origin);
        
        /// <summary>
        /// Logs error.
        /// </summary>
        /// <param name="message">Message to log. </param>
        /// <param name="origin">Origin of the log. </param>
        /// <param name="subject">Subject which caused the log. </param>
        void Error(string message, string origin, string subject);
    }
}