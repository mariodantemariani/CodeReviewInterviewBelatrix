using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _0.CodeProject.Exceptions
{
    class InvalidMessageManagement : Exception
    {
        public InvalidMessageManagement()
        {

        }

        public InvalidMessageManagement(string message): base(message)
        {

        }

    }

}
