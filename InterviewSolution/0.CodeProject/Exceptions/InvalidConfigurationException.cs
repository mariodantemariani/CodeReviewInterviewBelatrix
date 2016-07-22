using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _0.CodeProject.Exceptions
{
    class InvalidConfigurationException : Exception
    {
        public InvalidConfigurationException()
        {

        }

        public InvalidConfigurationException(string message): base(message)
        {

        }

    }

}
