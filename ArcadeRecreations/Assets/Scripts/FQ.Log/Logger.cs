using FQ.Libraries.MonoSingleton;
using UnityEngine;

namespace FQ.Log
{
    /// <summary>
    /// Logging out information
    /// </summary>
    public class Logger : MonoBehaviourSingleton<Logger>, ILog
    {
        /// <summary>
        /// Logs info.
        /// </summary>
        /// <param name="message">Message to log. </param>
        public void Info(string message)
        {
            Debug.Log(message);
        }

        /// <summary>
        /// Logs info.
        /// </summary>
        /// <param name="message">Message to log. </param>
        /// <param name="origin">Origin of the log. </param>
        public void Info(string message, string origin)
        {
            Debug.Log($"{origin}: {message}");
        }

        /// <summary>
        /// Logs info.
        /// </summary>
        /// <param name="message">Message to log. </param>
        /// <param name="origin">Origin of the log. </param>
        /// <param name="subject">Subject which caused the log. </param>
        public void Info(string message, string origin, string subject)
        {
            Debug.Log($"{origin}[{subject}]: {message}");
        }

        /// <summary>
        /// Logs warning.
        /// </summary>
        /// <param name="message">Message to log. </param>
        public void Warning(string message)
        {
            Debug.LogWarning($"{message}");
        }

        /// <summary>
        /// Logs warning.
        /// </summary>
        /// <param name="message">Message to log. </param>
        /// <param name="origin">Origin of the log. </param>
        public void Warning(string message, string origin)
        {
            Debug.LogWarning($"{origin}: {message}");
        }

        /// <summary>
        /// Logs warning.
        /// </summary>
        /// <param name="message">Message to log. </param>
        /// <param name="origin">Origin of the log. </param>
        /// <param name="subject">Subject which caused the log. </param>
        public void Warning(string message, string origin, string subject)
        {
            Debug.LogWarning($"{origin}[{subject}]: {message}");
        }

        /// <summary>
        /// Logs error.
        /// </summary>
        /// <param name="message">Message to log. </param>
        public void Error(string message)
        {
            Debug.LogWarning($"{message}");
        }

        /// <summary>
        /// Logs error.
        /// </summary>
        /// <param name="message">Message to log. </param>
        /// <param name="origin">Origin of the log. </param>
        public void Error(string message, string origin)
        {
            Debug.LogWarning($"{origin}: {message}");
        }

        /// <summary>
        /// Logs error.
        /// </summary>
        /// <param name="message">Message to log. </param>
        /// <param name="origin">Origin of the log. </param>
        /// <param name="subject">Subject which caused the log. </param>
        public void Error(string message, string origin, string subject)
        {
            Debug.LogError($"{origin}[{subject}]: {message}");
        }
    }
}