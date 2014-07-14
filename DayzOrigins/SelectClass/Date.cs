using System;

namespace DayzOrigins
{
    class Date
    {
        /// <summary>
        /// Возрашает текущую дату компьютера
        /// </summary>
        public static string GetDate()
        {
            return DateTime.Now.Year + "-" + DateTime.Now.Month.ToString("00") + "-" + DateTime.Now.Day.ToString("00");            
        }
    }
}
