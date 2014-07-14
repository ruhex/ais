using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace DayzOrigins
{
    class Otdels
    {
        public int Id { get; set; }
        public string Otdel { get; set; }
        public string OtdelZav { get; set; }

        public static List<Otdels> massiv = new List<Otdels>();

        //public List<Groups> Group { get { return  Groups.SelectPoOtdel(this.Id); } set { } }

        public static List<Otdels> Select()
        {
            MemoryManagement.FlushMemory();
            massiv.Clear();
            string Query = "SELECT id_otdel, name_otdel, zav_otdel FROM otdel";

            MySqlDataReader dataReader = MySQL.DataReader(Query);

            while (dataReader.Read())
            {
                massiv.Add(new Otdels
                {
                    Id = dataReader.GetInt16("id_otdel"),
                    Otdel = dataReader.GetString("name_otdel"),
                    OtdelZav = dataReader.GetString("zav_otdel")
                });
            }
            MySQL.connection.Close();
            return massiv;            
        }

        internal static string SelectByID(int OtdelId)
        {
            if (massiv.Count == 0)
                return null;
            return massiv.Find(delegate(Otdels temp) { return temp.Id == OtdelId; }).Otdel;
        }
    }
}
