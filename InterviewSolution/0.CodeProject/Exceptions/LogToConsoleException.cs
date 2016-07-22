using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _0.CodeProject.Exceptions
{
    class LogToConsoleException : Exception
    {
        public LogToConsoleException()
        {

        }

        public LogToConsoleException(string message): base(message)
        {

        }

        public LogToConsoleException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

}
