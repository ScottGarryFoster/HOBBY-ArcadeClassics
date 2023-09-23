using FQ.Libraries.MonoSingleton;
using UnityEngine;

namespace FQ.Logger
{
    /// <summary>
    /// Logging out information
    /// </summary>
    /// <remarks>
    /// Cannot make this static but the methods we can.
    /// This is due to Unity, if we want to interact with Unity Debugging this is the way.
    /// </remarks>
    public class Log : MonoBehaviour
    {
        /// <summary>
        /// Turns errors into warning to ensure tests do not fail.
        /// </summary>
        private static bool testMode = false;
        
        /// <summary>
        /// Enables test mode meaning Errors are not called.
        /// </summary>
        internal static void TestMode()
        {
            testMode = true;
        }

        /// <summary>
        /// Resets Logs back to production level.
        /// </summary>
        internal static void ResetTestMode()
        {
            testMode = false;
        }
        
        /// <summary>
        /// Logs info.
        /// </summary>
        /// <param name="message">Message to log. </param>
        public static void Info(string message)
        {
            Debug.Log(message);
        }

        /// <summary>
        /// Logs info.
        /// </summary>
        /// <param name="message">Message to log. </param>
        /// <param name="origin">Origin of the log. </param>
        public static void Info(string message, string origin)
        {
            Debug.Log($"{origin}: {message}");
        }

        /// <summary>
        /// Logs info.
        /// </summary>
        /// <param name="message">Message to log. </param>
        /// <param name="origin">Origin of the log. </param>
        /// <param name="subject">Subject which caused the log. </param>
        public static void Info(string message, string origin, string subject)
        {
            Debug.Log($"{origin}[{subject}]: {message}");
        }

        /// <summary>
        /// Logs warning.
        /// </summary>
        /// <param name="message">Message to log. </param>
        public static void Warning(string message)
        {
            Debug.LogWarning($"{message}");
        }

        /// <summary>
        /// Logs warning.
        /// </summary>
        /// <param name="message">Message to log. </param>
        /// <param name="origin">Origin of the log. </param>
        public static void Warning(string message, string origin)
        {
            Debug.LogWarning($"{origin}: {message}");
        }

        /// <summary>
        /// Logs warning.
        /// </summary>
        /// <param name="message">Message to log. </param>
        /// <param name="origin">Origin of the log. </param>
        /// <param name="subject">Subject which caused the log. </param>
        public static void Warning(string message, string origin, string subject)
        {
            Debug.LogWarning($"{origin}[{subject}]: {message}");
        }

        /// <summary>
        /// Logs error.
        /// </summary>
        /// <param name="message">Message to log. </param>
        public static void Error(string message)
        {
            if (testMode)
            {
                Debug.LogWarning($"{message}");
            }
            else
            {
                Debug.LogError($"{message}");
            }
        }

        /// <summary>
        /// Logs error.
        /// </summary>
        /// <param name="message">Message to log. </param>
        /// <param name="origin">Origin of the log. </param>
        public static void Error(string message, string origin)
        {
            if (testMode)
            {
                Debug.LogWarning($"{origin}: {message}");
            }
            else
            {
                Debug.LogError($"{origin}: {message}");
            }
        }

        /// <summary>
        /// Logs error.
        /// </summary>
        /// <param name="message">Message to log. </param>
        /// <param name="origin">Origin of the log. </param>
        /// <param name="subject">Subject which caused the log. </param>
        public static void Error(string message, string origin, string subject)
        {
            if (testMode)
            {
                Debug.LogWarning($"{origin}[{subject}]: {message}");
            }
            else
            {
                Debug.LogError($"{origin}[{subject}]: {message}");
            }
        }
    }
}