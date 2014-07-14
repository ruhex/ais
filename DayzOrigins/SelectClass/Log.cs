using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;



namespace DayzOrigins
{
    public class Log
    {
        public enum LogEvent { ApplicationExit, UserLogin, UserLogout, UserLoginError, UserEditData }
        private static string UserName { get { return User.Login; } }
        public static void LogSend(LogEvent eve)
        {
            string log = "";

            switch (eve)
            {
                case LogEvent.ApplicationExit: log = "Пользователь " + UserName + " завершил работу"; break;
                case LogEvent.UserEditData:  break;
                case LogEvent.UserLogin: log = "Пользователь " + UserName + " успешно авторизировался";  break;
                case LogEvent.UserLogout: log = "Пользователь " + UserName + " отключился"; break;
                case LogEvent.UserLoginError: log = "Аутентификация пользователя " + UserName + " не удалась"; break;
            }
            MySQL.InsertLog(log);
            MemoryManagement.FlushMemory();
        }
    }
}
