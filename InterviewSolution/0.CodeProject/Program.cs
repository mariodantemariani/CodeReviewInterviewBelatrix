
using _0.CodeProject;
using System;

namespace CodeProject
{
    class Program
    {
        static void Main(string[] args)
        {
            JobLogger init = new JobLogger(true, true, true, true, true, true);

            init.LogMessage("message1: Error", JobLogger.LogLevel.Error);

            init.LogMessage("message2: Message", JobLogger.LogLevel.Message);
            init.LogMessage("message3: Warning", JobLogger.LogLevel.Warning);

            Console.ReadKey();
        }
    }
}
