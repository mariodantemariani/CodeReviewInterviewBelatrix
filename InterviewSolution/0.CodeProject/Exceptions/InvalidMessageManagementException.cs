using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _0.CodeProject.Exceptions
{
    public class InvalidMessageManagementException : Exception
    {
        public InvalidMessageManagementException()
        {

        }

        public InvalidMessageManagementException(string message): base(message)
        {

        }

    }

}
