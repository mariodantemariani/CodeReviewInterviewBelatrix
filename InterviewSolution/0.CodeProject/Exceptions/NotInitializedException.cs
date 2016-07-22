using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _0.CodeProject.Exceptions
{
    class NotInitializedException : Exception
    {
        public NotInitializedException()
        {

        }

        public NotInitializedException(string message): base(message)
        {

        }

    }

}
