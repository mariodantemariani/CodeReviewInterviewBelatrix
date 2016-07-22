using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _0.CodeProject.Exceptions
{
    class LogToDataBaseException : Exception
    {
        public LogToDataBaseException()
        {

        }

        public LogToDataBaseException(string message): base(message)
        {

        }

        public LogToDataBaseException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

}
