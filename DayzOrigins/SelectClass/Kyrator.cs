using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace DayzOrigins
{
    class Kyrator
    {
        public int Id { get; set; }
        public int KurId { get; set; }
        public string FirstName { get; set; }   // Фамилия
        public string LastName { get; set; }    // Имя
        public List<Groups> Group { get { return Groups.SelectPoOtdel(this.KurId); } set { } }

        public static List<Kyrator> massiv = new List<Kyrator>();

        public static List<Kyrator> Select()
        {
            MemoryManagement.FlushMemory();
            massiv.Clear();
            string Query = "SELECT id_str, fname_str, lname_str, kur_sotr FROM sotrudniki";
            MySqlDataReader dataRead = MySQL.DataReader(Query);

            while (dataRead.Read())
            {
                massiv.Add(new Kyrator
                {
                    Id = dataRead.GetInt16("id_str"),
                    KurId = dataRead.GetInt16("kur_sotr"),
                    FirstName = dataRead.GetString("fname_str"),
                    LastName = dataRead.GetString("lname_str")
                });
            }
            MySQL.connection.Close();
            return massiv;
        }

        internal static string SelectByID(int OtdelId)
        {
            if (massiv.Count == 0)
                return null;
            return massiv.Find(delegate(Kyrator temp) { return temp.KurId == OtdelId; }).LastName;
        }
    }
}
