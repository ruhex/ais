using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace DayzOrigins
{
    public class Status
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public static List<Status> massiv = new List<Status>();


        public static List<Status> Select()
        {
            MemoryManagement.FlushMemory();
            massiv.Clear();

            string Query = "SELECT id_st, name_st FROM status";

            MySqlDataReader dataReader = MySQL.DataReader(Query);
            while (dataReader.Read())
            {
                massiv.Add(new Status
                {
                    ID = dataReader.GetInt16("id_st"),
                    Name = dataReader.GetString("name_st")
                });
            }
            MySQL.connection.Close();
            return massiv;
        }

        public static Status SelectPoID(int Id)
        {
            return massiv.Find(delegate(Status st) { return st.ID == Id; });
        }
    }
}
