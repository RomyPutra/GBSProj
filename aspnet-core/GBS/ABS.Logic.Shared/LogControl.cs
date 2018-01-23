using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Diagnostics;
using System.Reflection;

    public class LogControl
{
        #region declaration
        string InfoLogLocation = "C%3a%5cLogEvent%5cInfoLog%5c";
		//System.Configuration.ConfigurationManager.AppSettings["InfoLogLocation"].ToString();
        string ErrorLogLocation = "C%3a%5cLogEvent%5cErrorLog%5c";
		//System.Configuration.ConfigurationManager.AppSettings["ErrorLogLocation"].ToString();
        #endregion

        /// <summary>
        /// save log in .txt
        /// </summary>
        /// <param name="msg">information message</param>
        public void Info(object obj, string msg)
        {
            //Get Class's Name
            string className = string.Empty;
            if (obj.GetType().Name != "String")
            {
                className = obj.GetType().Name;
            }
            else
            {
                className = obj.ToString();
            }

            if (Directory.Exists(InfoLogLocation) == false)
            {
                Directory.CreateDirectory(InfoLogLocation);
            }
            string logFileName = "Log" + string.Format("{0:yyyyMMdd}", System.DateTime.Now) + ".txt";
            string logFilePath = InfoLogLocation + logFileName;

            string methodName = string.Empty;
            StackTrace stackTrace = new StackTrace();
            StackFrame stackFrame = stackTrace.GetFrame(stackTrace.FrameCount - 1);
            MethodBase methodBase = stackFrame.GetMethod();
            methodName = methodBase.Name;
            string lineNumber = stackFrame.GetFileLineNumber().ToString();

            string message = string.Empty;
            message = "Info-[" + System.DateTime.Now.ToString() + "] " + className + " | " + methodName + " | " + msg.Replace("\n", "").Replace("\r","");

            //using (StreamWriter sw = new StreamWriter(logFilePath, true))
            using (StreamWriter sw = File.AppendText(logFilePath))
            {
                sw.WriteLine(message);
                //sw.WriteLine("");
                sw.Dispose();
            }
        }

        /// <summary>
        /// save log in .txt
        /// </summary>
        /// <param name="msg">information message</param>
        public void Warning(object obj, string msg)
        {
            //Get Class's Name
            string className = string.Empty;
            if (obj.GetType().Name != "String")
            {
                className = obj.GetType().Name;
            }
            else
            {
                className = obj.ToString();
            }

            if (Directory.Exists(InfoLogLocation) == false)
            {
                Directory.CreateDirectory(InfoLogLocation);
            }
            string logFileName = "Log" + string.Format("{0:yyyyMMdd}", System.DateTime.Now) + ".txt";
            string logFilePath = InfoLogLocation + logFileName;
            
            string methodName = string.Empty;
            StackTrace stackTrace = new StackTrace();
            StackFrame stackFrame = stackTrace.GetFrame(stackTrace.FrameCount - 1);
            MethodBase methodBase = stackFrame.GetMethod();
            methodName = methodBase.Name;
            string lineNumber = stackFrame.GetFileLineNumber().ToString();

            string message = string.Empty;
            message = "Warning-[" + System.DateTime.Now.ToString() + "] " + className + " | " + methodName + " | " + msg.Replace("\r\n", "");

            //using (StreamWriter sw = new StreamWriter(logFilePath, true))
            using (StreamWriter sw = File.AppendText(logFilePath))
            {
                sw.WriteLine(message);
                sw.WriteLine("");
                sw.Dispose();
            }
        }

        /// <summary>
        /// save error log in .txt
        /// </summary>
        /// <param name="ex">exception</param>
        public void Error(object obj, Exception ex, string msg = "")
        {
            //Get Class's Name
            string className = string.Empty;
            if (obj.GetType().Name != "String")
            {
                className = obj.GetType().Name;
            }
            else
            {
                className = obj.ToString();
            }

            if (Directory.Exists(ErrorLogLocation) == false)
            {
                Directory.CreateDirectory(ErrorLogLocation);
            }

            string logFileName = "Log" + string.Format("{0:yyyyMMdd}", System.DateTime.Now) + ".txt";
            string logFilePath = ErrorLogLocation + logFileName;
            
            string methodName = string.Empty;
            StackTrace stackTrace = new StackTrace();
            StackFrame stackFrame = stackTrace.GetFrame(stackTrace.FrameCount-1);
            MethodBase methodBase = stackFrame.GetMethod();
            methodName = methodBase.Name;
            string errorOnLine = stackFrame.GetFileLineNumber().ToString();

            string message = string.Empty;
            message = "[" + System.DateTime.Now.ToString() + "] " + className + " | " + methodName + " | " + ex.Message.Replace("\r\n", "");
            message += "\n" + ex.StackTrace.ToString();
            if (msg != "")
            {
                message += "\n Custom message: " + msg.ToString();
            }

            //using (StreamWriter sw = new StreamWriter(logFilePath, true))
            using (StreamWriter sw = File.AppendText(logFilePath))
            {
                sw.WriteLine(message);
                sw.WriteLine("");
                sw.Dispose();
            }
        }

        public void Error(object obj, string msg)
        {
            //Get Class's Name
            string className = string.Empty;
            if (obj.GetType().Name != "String")
            {
                className = obj.GetType().Name;
            }
            else
            {
                className = obj.ToString();
            }

            if (Directory.Exists(ErrorLogLocation) == false)
            {
                Directory.CreateDirectory(ErrorLogLocation);
            }

            string logFileName = "Log" + string.Format("{0:yyyyMMdd}", System.DateTime.Now) + ".txt";
            string logFilePath = ErrorLogLocation + logFileName;

            string methodName = string.Empty;
            StackTrace stackTrace = new StackTrace();
            StackFrame stackFrame = stackTrace.GetFrame(stackTrace.FrameCount - 1);
            MethodBase methodBase = stackFrame.GetMethod();
            methodName = methodBase.Name;
            string errorOnLine = stackFrame.GetFileLineNumber().ToString();

            string message = string.Empty;
            message = "[" + System.DateTime.Now.ToString() + "] " + className + " | " + methodName + " | " + msg.Replace("\r\n", "");

            //using (StreamWriter sw = new StreamWriter(logFilePath, true))
            using (StreamWriter sw = File.AppendText(logFilePath))
            {
                sw.WriteLine(message);
                sw.WriteLine("");
                sw.Dispose();
            }
        }
    }

    

