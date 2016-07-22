using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _0.CodeProject.Exceptions
{
    class ObsoleteMethodException : Exception
    {
        public ObsoleteMethodException()
        {

        }

        public ObsoleteMethodException(string message): base(message)
        {

        }

    }

}
