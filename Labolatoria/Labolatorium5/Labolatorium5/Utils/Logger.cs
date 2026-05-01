using System;
using System.Collections.Generic;
using System.Text;

namespace Labolatorium5.Utils
{
    internal class Logger
    {
        private static readonly string LogFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs.txt");
        public static void Info(string message) => Write("INFO", message); 
        public static void Error(string message) => Write("ERROR", message);
        private static void Write(string level, string message)
        {
            string line = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{level}] {message}";
            File.AppendAllText(LogFilePath, line + Environment.NewLine);
        }
    }
}

