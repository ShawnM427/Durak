using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Durak.Common
{
    /// <summary>
    /// A utility for logging information to a human-readable text file
    /// </summary>
    public class Logger : TextWriter
    {
        #region Class-Level

        /// <summary>
        /// Gets the time format for the log's time stamps
        /// </summary>
        public static readonly string LOG_TIME_FORMAT = "HH:mm:ss";
        /// <summary>
        /// Gets the date format for log file naming
        /// </summary>
        public static readonly string LOG_FILE_TIME_FORMAT = "yyyy-MM-dd_HH_mm";

        /// <summary>
        /// Stores a singleton isntance of a logger for use statically
        /// </summary>
        private static Logger mySingleton;

        /// <summary>
        /// Gets the singleton instance of the logger, created when any logger function was invoked or referenced
        /// </summary>
        public static Logger Singleton
        {
            get { return mySingleton; }
        }

        /// <summary>
        /// Static contructor, creates a new isntance of the logger
        /// </summary>
        static Logger()
        {
            mySingleton = new Logger();
        }

        #endregion

        #region Instance-Level

        /// <summary>
        /// The stream to write to
        /// </summary>
        private StreamWriter myStream;

        /// <summary>
        /// Creates a new isntance of a logger
        /// </summary>
        public Logger()
        {
            string fileName = "log_" + DateTime.Now.ToString(LOG_FILE_TIME_FORMAT) + ".txt";
            myStream = new StreamWriter(fileName);
        }

        new private void WriteLine(string line)
        {
            myStream.WriteLine(line);
            mySingleton.myStream.Flush();
        }

        public static void Write(Exception e)
        {
            Write("Encounted {0} at:", e.GetType().Name);

            StackTrace trace = new StackTrace(e, true);

            foreach (StackFrame frame in trace.GetFrames())
            {
                Write("\t{0}.{1} - line {2}", frame.GetMethod().DeclaringType, frame.GetMethod().Name, frame.GetFileLineNumber());
            }
        }

        /// <summary>
        /// Writes single line of unformatted text to the log file
        /// </summary>
        /// <param name="rawText">The line to write</param>
        new public static void Write(string rawText)
        {
            mySingleton.WriteLine(rawText);
        }

        /// <summary>
        /// Writes a single line of formatted text to the log file
        /// </summary>
        /// <param name="format">The format string</param>
        /// <param name="parameters">The parameters for the format</param>
        new public static void Write(string format, params object[] parameters)
        {
            mySingleton.WriteLine(string.Format("[{0}] {1}", DateTime.Now.ToString(LOG_TIME_FORMAT), string.Format(format, parameters)));
        }

        /// <summary>
        /// Gets the encoding for this logger, will be ASCII
        /// </summary>
        public override Encoding Encoding
        {
            get { return Encoding.ASCII; }
        }
        
        #endregion
    }
}
