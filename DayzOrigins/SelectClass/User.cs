using System.Security.Cryptography;
using System.Text;
using MySql.Data.MySqlClient;

namespace DayzOrigins
{
    public class User
    {
        static int Id { get; set; }
        public static string Login { get; set; }
        static string Pass { get; set; }
        static string Role { get; set; }
        static Hash HashType { get; set; }
        

        /// <summary>
        /// Метод реализующий авторизацию пользователя. Принимает 2 параметра: логин и пароль. Задаёт метод щифрования и устанавливает соединение с БД, передавая принятые параметры.
        /// Возрашает Boolean соединения.
        /// </summary>
        /// <param name="user">Логин</param>
        /// <param name="pass">Пароль</param>
        /// <returns>Успех или крах авторизации</returns>
        public static bool Authorization(string user, string pass)
        {
            //Задаём алгоритм хэширования пасса
            HashType = Hash.SHA1;

            MySqlDataReader dataReaderTemp = MySQL.UserAut(user, pass);
            while (dataReaderTemp.Read())
            {
                Id = dataReaderTemp.GetInt32("id_user");
                Login = dataReaderTemp.GetString("login_user");
                Role = dataReaderTemp.GetString("role");
            }

            if (!dataReaderTemp.HasRows)
            {
                MySQL.connection.Close();
                return false;
            }
            bool toReturn = false;
            switch (HashType)
            {
                case Hash.SHA1:
                    if (sha1(pass) == dataReaderTemp.GetString("pass_user"))
                        toReturn = true;
                    break;
                case Hash.MD5:
                    if (md5(pass) == dataReaderTemp.GetString("pass_user"))
                        toReturn = true;
                    break;
            }
            MySQL.connection.Close();
            MemoryManagement.FlushMemory();
            return toReturn;
        }

        /// <summary>
        /// Метод хэширования sha1
        /// </summary>
        static string sha1(string input)
        {
            byte[] hash;
            using (var sha1 = new SHA1CryptoServiceProvider())
                hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
            var sb = new StringBuilder();
            foreach (byte b in hash) sb.AppendFormat("{0:x2}", b);
            MemoryManagement.FlushMemory();
            return sb.ToString();
        }

        /// <summary>
        /// Метод хэширования md5
        /// </summary>
        static string md5(string input)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder sBuilder = new StringBuilder();

                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
                MemoryManagement.FlushMemory();
                return sBuilder.ToString();
            }
        }
        enum Hash { SHA1, MD5 };
    }
}
