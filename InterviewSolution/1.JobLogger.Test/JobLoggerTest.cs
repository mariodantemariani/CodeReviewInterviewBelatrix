using Microsoft.VisualStudio.TestTools.UnitTesting;
using _0.CodeProject;
using System;

namespace _1.JobLoggerTest
{
    
    [TestClass]
    public class JobLoggerTest
    {
        #region variables
        private string msgToTest = "test";
        #endregion


        public JobLoggerTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }


        #region tests about JobLog initialization

        #endregion

        #region tests about JobLog validation

        #endregion

        #region tests about JobLog method
        
        [TestMethod]
        [Ignore]
        public void LogToDataBaseTest()
        {
            //Only LogDataBase test
            JobLogger init = new JobLogger(false, false, true, true, true, true);

            init.LogMessage(msgToTest, JobLogger.LogLevel.Error);

            string expectedString = string.Format("{0} {1}", DateTime.Now.ToShortDateString(), msgToTest);
            string actualString = expectedString; 

            Assert.AreEqual(expectedString, actualString);
        }

        [TestMethod]
        public void LogToFileTest()
        {
            //Only LogFile Test
            JobLogger init = new JobLogger(true, false, false, true, true, true);

            init.LogMessage(msgToTest, JobLogger.LogLevel.Error);

            string expectedString = string.Format("{0} {1}", DateTime.Now.ToShortDateString(), msgToTest);
            string actualString = expectedString;

            Assert.AreEqual(expectedString, actualString);
        }

        #endregion


    }
}
