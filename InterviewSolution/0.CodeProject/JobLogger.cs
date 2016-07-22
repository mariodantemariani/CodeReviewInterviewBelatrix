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

    #region Enum LogLevel is used for handling the message type 

    public enum LogLevel
    {
        Message = 0,
        Warning = 1,
        Error = 2
    }

    #endregion

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

    [System.Obsolete("use method LogMessage(string message, LogLevel logLevel)")]
    public void LogMessage(string messageText, bool message, bool warning, bool error)
    {
        throw new ObsoleteMethodException("Method obselete, you must use LogMessage(string message, LogLevelEnm logLevel)");
    }

    public void LogMessage(string messageText, LogLevel logLevel = 0)
    {
        if (!_initialized)
        {
            throw new NotInitializedException("Must initialized");
        }

        if (string.IsNullOrWhiteSpace(messageText))
        {
            return;
        }

        validateMessageManagement(logLevel);

        if (_logToDatabase)
        {
            LogToDataBase(messageText, logLevel);
        }

        if (_logToFile)
        {
            LogToFile(messageText, logLevel);            
        }

        if (_logToConsole)
        {
            LogToConsole(messageText, logLevel);
        }
    }

    private void validateMessageManagement(LogLevel logLevel)
    {
        if ((!_logError && !_logMessage && !_logWarning) || !Enum.IsDefined(typeof(LogLevel), logLevel))
        {
            throw new Exception("Error003: Error or Warning or Message must be specified");
        }
    }

    private void LogToDataBase(string messageText, LogLevel logLevel)
    {
        System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConnectionString"]);
        connection.Open();

        //variable t must be initialized
        int levelErrorOnDataBase = 0;
        switch (logLevel)
        {
            case LogLevel.Message:
                if (_logMessage){
                    levelErrorOnDataBase = 1;
                }
                break;
            case LogLevel.Error:
                if (_logError){
                    levelErrorOnDataBase = 2;
                }
                break;
            case LogLevel.Warning:
            default:
                if (_logWarning){
                    levelErrorOnDataBase = 3;
                }
                break;
        }

        System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand("Insert into Log Values('" + messageText + "', " + levelErrorOnDataBase.ToString() + ")");
        command.ExecuteNonQuery();
    }

    private void LogToFile(string messageText, LogLevel logLevel)
    {
        string levelErrorOnFile = string.Empty;
        if (!System.IO.File.Exists(System.Configuration.ConfigurationManager.AppSettings["LogFileDirectory"] + "LogFile" + DateTime.Now.ToShortDateString() + ".txt"))
        {
            levelErrorOnFile = System.IO.File.ReadAllText(System.Configuration.ConfigurationManager.AppSettings["LogFileDirectory"] + "LogFile" + DateTime.Now.ToShortDateString() + ".txt");
        }

        switch (logLevel)
        {
            case LogLevel.Error:
                if (_logError)
                {
                    levelErrorOnFile = levelErrorOnFile + DateTime.Now.ToShortDateString() + messageText;
                }
                break;
            case LogLevel.Warning:
                if (_logWarning)
                {
                    levelErrorOnFile = levelErrorOnFile + DateTime.Now.ToShortDateString() + messageText;
                }
                break;
            case LogLevel.Message:
            default:
                if (_logMessage)
                {
                    levelErrorOnFile = levelErrorOnFile + DateTime.Now.ToShortDateString() + messageText;
                }
                break;
        }

        System.IO.File.WriteAllText(System.Configuration.ConfigurationManager.AppSettings["LogFileDirectory"] + "LogFile" + DateTime.Now.ToShortDateString() + ".txt", levelErrorOnFile);
    }

    private void LogToConsole(string message, LogLevel logLevel)
    {
        try
        {
            switch (logLevel)
            {
                case LogLevel.Error:
                    if (_logError){
                        Console.ForegroundColor = ConsoleColor.Red;
                    }

                    break;
                case LogLevel.Warning:
                    if (_logWarning){
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    }
                    break;
                case LogLevel.Message:
                default:
                    if (_logMessage){
                        Console.ForegroundColor = ConsoleColor.White;
                    }

                    break;
            }

            Console.WriteLine("{0} {1}", DateTime.Now.ToShortDateString(), message);

        }
        catch (Exception ex)
        {
            throw new ApplicationException("Error: Error console", ex);
        }
    }

}