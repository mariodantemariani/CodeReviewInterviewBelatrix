using _0.CodeProject.Exceptions;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
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

    public JobLogger()
    {
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
            throw new InvalidMessageManagementException("Error or Warning or Message must be specified");
        }
    }

    private void LogToDataBase(string message, LogLevel logLevel)
    {
        try
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);

            var levelErrorOnDataBase = 0;
            switch (logLevel)
            {
                case LogLevel.Message:
                    if (_logMessage)
                    {
                        levelErrorOnDataBase = 1;
                    }
                    break;
                case LogLevel.Error:
                    if (_logError)
                    {
                        levelErrorOnDataBase = 2;
                    }
                    break;
                case LogLevel.Warning:
                default:
                    if (_logWarning)
                    {
                        levelErrorOnDataBase = 3;
                    }
                    break;
            }

            connection.Open();
            SqlCommand command = new SqlCommand("Insert into Log Values('" + message + "', " + levelErrorOnDataBase.ToString() + ")");
            command.ExecuteNonQuery();
            connection.Close();
        }
        catch (Exception ex)
        {
            throw new LogToDataBaseException("Error DataBase: ", ex);
        }
    }

    private void LogToFile(string messageText, LogLevel logLevel)
    {
        try
        {
            var levelErrorOnFile = string.Empty;

            var parseDate = DateTime.Now.ToShortDateString().Replace("/", "-");

            var path = ConfigurationManager.AppSettings["LogFileDirectory"] + "LogFile" + parseDate + ".txt";

            if (!File.Exists(path))
            {
                File.Create(path).Dispose();

                levelErrorOnFile = File.ReadAllText(path);
            }

            switch (logLevel)
            {
                case LogLevel.Error:
                    if (_logError)
                    {
                        levelErrorOnFile = String.Format("{0} {1}", levelErrorOnFile + DateTime.Now.ToShortDateString(), messageText + Environment.NewLine);

                    }
                    break;
                case LogLevel.Warning:
                    if (_logWarning)
                    {
                        levelErrorOnFile = String.Format("{0} {1}", levelErrorOnFile + DateTime.Now.ToShortDateString(), messageText + Environment.NewLine);
                    }
                    break;
                case LogLevel.Message:
                default:
                    if (_logMessage)
                    {
                        levelErrorOnFile = String.Format("{0} {1}", levelErrorOnFile + DateTime.Now.ToShortDateString(), messageText + Environment.NewLine);
                    }
                    break;
            }
            
            System.IO.File.AppendAllText(path, levelErrorOnFile);

        }
        catch (Exception ex)
        {
            throw new LogToFileException("Error log to File:", ex);
        }
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
            throw new LogToConsoleException("Error to Console: ", ex);
        }
    }

}