using _0.CodeProject.Exceptions;
using System;
using System.Linq;
using System.Text;

public class JobLogger
{

    private static bool _logToFile;
    private static bool _logToConsole;
    private static bool _logMessage;
    private static bool _logWarning;
    private static bool _logError;

    private static bool _logToDatabase;

    private bool _initialized;

    public JobLogger(bool logToFile, bool logToConsole, bool logToDatabase, bool logMessage, bool logWarning, bool logError)
    {
        _logError = logError;
        _logMessage = logMessage;
        _logWarning = logWarning;
        _logToDatabase = logToDatabase;
        _logToFile = logToFile;
        _logToConsole = logToConsole;

        if (!_logToConsole && !_logToFile && !_logToDatabase)
        {
            throw new InvalidConfigurationException("Invalid configuration");
        }
        else
        {
            _initialized = true;
        }
    }

    //replace "string message" by string messageText
    public void LogMessage(string messageText, bool message, bool warning, bool error)
    {
        if (!_initialized)
        {
            throw new NotInitializedException("Must initialized");
        }

        if (string.IsNullOrWhiteSpace(messageText))
        {
            return;
        }

        validateMessageManagement(message, warning, error);

        if (_logToDatabase)
        {
            System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConnectionString"]);
            connection.Open();

            //variable t must be initialized
            int levelErrorOnDataBase = 0;
            if (message && _logMessage)
            {
                levelErrorOnDataBase = 1;
            }
            if (error && _logError)
            {
                levelErrorOnDataBase = 2;
            }
            if (warning && _logWarning)
            {
                levelErrorOnDataBase = 3;
            }
            System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand("Insert into Log Values('" + message + "', " + levelErrorOnDataBase.ToString() + ")");
            command.ExecuteNonQuery();
        }

       

        //variable l must be initialized
        string levelErrorOnFile = string.Empty;
        if(!System.IO.File.Exists(System.Configuration.ConfigurationManager.AppSettings["LogFileDirectory"] + "LogFile" + DateTime.Now.ToShortDateString() + ".txt")) 
        {
            levelErrorOnFile = System.IO.File.ReadAllText(System.Configuration.ConfigurationManager.AppSettings["LogFileDirectory"] + "LogFile" + DateTime.Now.ToShortDateString() + ".txt"); 
        }
        if (error && _logError)
        {
            levelErrorOnFile = levelErrorOnFile + DateTime.Now.ToShortDateString() + message;
        }
        if (warning && _logWarning)
        {
            levelErrorOnFile = levelErrorOnFile + DateTime.Now.ToShortDateString() + message;
        }
        if (message && _logMessage)
        {
            levelErrorOnFile = levelErrorOnFile + DateTime.Now.ToShortDateString() + message;
        }

        System.IO.File.WriteAllText(System.Configuration.ConfigurationManager.AppSettings["LogFileDirectory"] + "LogFile" + DateTime.Now.ToShortDateString() + ".txt", levelErrorOnFile);
        if (error && _logError)
        {
            Console.ForegroundColor = ConsoleColor.Red;
        }
        if (warning && _logWarning)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
        }
        if (message && _logMessage)
        {
            Console.ForegroundColor = ConsoleColor.White;
        }
        Console.WriteLine(DateTime.Now.ToShortDateString() + message);
    }

    private void validateMessageManagement(bool message, bool warning, bool error)
    {
        if ((!_logError && !_logMessage && !_logWarning) || (!message && !warning && !error))
        {
            throw new InvalidMessageManagement("Error or Warning or Message must be specified");
        }
    }
}