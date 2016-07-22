using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _0.CodeProject.Exceptions
{
    class LogToFileException : Exception
    {
        public LogToFileException()
        {

        }

        public LogToFileException(string message): base(message)
        {

        }

        public LogToFileException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

}
