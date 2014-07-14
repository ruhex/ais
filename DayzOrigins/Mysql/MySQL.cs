using MySql.Data.MySqlClient;




namespace DayzOrigins
{
    class MySQL
    {
        // Задание параметров подключения к базе данных. Запись их в конфигурационный файл

        static public string DB { get { return Settings.Default.db; } set { Settings.Default.db = value; } }
        static public string Host { get { return Settings.Default.server; } set { Settings.Default.server = value; } }
        static public string User { get { return Settings.Default.login; } set { Settings.Default.login = value; } }
        static public string Pass { get { return Settings.Default.passwd; } set { Settings.Default.passwd = value; } }
        static public MySqlConnection connection { get; set; }

        internal static bool MySqlConnection()
        {
            return MySqlConnection(User, Pass, Host, DB, false);
        }

        
        // Передача параметров подключения фенкции, с последующим назначением свойств.
        /// <summary>
        /// Метод MySqlConnection. Устанавливает подлючение к БД.
        /// </summary>
        public static bool MySqlConnection(string user, string passwd, string host, string db, bool test)
        {
            User = user;
            Pass = passwd;
            Host = host;
            DB = db;

            MySqlConnectionStringBuilder mysqlSting = new MySqlConnectionStringBuilder();

            mysqlSting.Server = Host;
            mysqlSting.Database = DB;
            mysqlSting.UserID = User;
            mysqlSting.Password = Pass;
            mysqlSting.CharacterSet = "utf8";

            connection = new MySqlConnection(mysqlSting.ToString());
            {
                try
                {
                    connection.Open();
                    connection.Close();
                    return true;
                }
                catch { return false; }
            }
        }

        /// <summary>
        /// Статический метод DataReader. Реализует считывание данных из БД. Принимает на вход строку запроса. Возрашает MySqlDataReader.
        /// </summary>
        /// <param name="Query">Строка запроса</param>
        public static MySqlDataReader DataReader(string Query)
        {
            connection.Open();
            MySqlCommand cmd = new MySqlCommand(Query, connection);
            return cmd.ExecuteReader();
        }

        /// <summary>
        /// Статический метод ImageUpload. Реализует загрузку фотографии студента в БД. На вход принимает 3 параметра.
        /// </summary>
        /// <param name="Id">ID студента</param>
        /// <param name="size">размер картинки (в байтах)</param>
        /// <param name="array">массив байт картинки</param>
        public static void ImageUpload(int Id, int size, byte[] array)
        {
            connection.Open();
            MySqlCommand cmd = new MySqlCommand("UPDATE study SET photo_st = ?photo_st, photoSize_st = " + size + " WHERE id_st = " + Id, connection);
            MySqlParameter _Param = new MySqlParameter("photo_st", MySqlDbType.MediumBlob);
            _Param.Value = array;
            cmd.Parameters.Add(_Param);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            connection.Close();
        }

        /// <summary>
        /// Статический метод ImageSelect. Производит выборку фотографии студента из БД. На вход принимает 1 параметр. Возрашает MySqlDataReader.
        /// </summary>
        /// <param name="Id">ID студента</param>
        public static MySqlDataReader ImageSelect(int Id)
        {
            connection.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT photo_st, photoSize_st FROM study WHERE id_st = " + Id, connection);
            return cmd.ExecuteReader();
        }

        /// <summary>
        /// Статический метод MySqlDataReader. Производит запрос к БД для проверки Логина и Пароля. На вход принимает 2 параметра. Возрашает MySqlDataReader.
        /// </summary>
        /// <param name="user">Логин</param>
        /// <param name="pass">Пароль</param>
        /// <returns></returns>
        public static MySqlDataReader UserAut(string user, string pass)
        {
            connection.Close();
            connection.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT id_user, login_user, pass_user, role FROM user WHERE login_user = '" + user + "'", connection);
            return cmd.ExecuteReader();
        }

        /// <summary>
        /// Статический метод InsertLog. Производит производит запись логов в БД. На вход принимает 1 параметр. Возрашает Boolean.
        /// </summary>
        /// <param name="log">Лог</param>
        /// <returns></returns>
        public static bool InsertLog(string log)
        {
            string SQL = "INSERT INTO log (info, datetime) VALUES (";           
            SQL += "'" + log + "',";
            SQL += "now())";

            connection.Open();
            MySqlCommand cmd = new MySqlCommand(SQL, connection);
            cmd.ExecuteNonQuery();
            connection.Close();
            return true;
        }
    }
}
