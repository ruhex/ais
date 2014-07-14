using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace DayzOrigins
{
    class Specials
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Qual { get; set; }

        public static List<Specials> massiv = new List<Specials>();

        public static List<Specials> Select()
        {
            massiv.Clear();
            string Query = "SELECT id_spec, name_spec, code_spec, qual_spec FROM specials";

            MySqlDataReader dataReader = MySQL.DataReader(Query);

            while (dataReader.Read())
            {
                massiv.Add(new Specials
                {
                    Id = dataReader.GetInt16("id_spec"),
                    Name = dataReader.GetString("name_spec"),
                    Code = dataReader.GetString("code_spec"),
                    Qual = dataReader.GetString("qual_spec")
                });
                
            }
            MySQL.connection.Close();
            return massiv;            
        }

        public static string SelectName(int Id)
        {
            return massiv.Find(
                delegate(Specials spec) { return spec.Id == Id; }).Name;
        }

        public static string SelectCode(int Id)
        {
            return massiv.Find(
                delegate(Specials spec) { return spec.Id == Id; }).Code;
        }

        public static string SelectQual(int Id)
        {
            return massiv.Find(
                delegate(Specials spec) { return spec.Id == Id; }).Qual;
        }
    }
}
