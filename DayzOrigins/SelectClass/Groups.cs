using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace DayzOrigins
{
    public class Groups
    {
        public int Id { get; set; }
        public int Group { get; set; }
        int SpecId { get; set; }
        public int OtdelId { get; set; }
        public int KuratorId { get; set; }
        public string SpecName { get { return Specials.SelectName(SpecId); } set { } }
        public string SpecCode { get { return Specials.SelectCode(SpecId); } set { } }
        public string SpecQual { get { return Specials.SelectQual(SpecId); } set { } }
        public string Otdel { get { return Otdels.SelectByID(OtdelId); } set { } }
        
        public static List<Groups> massiv = new List<Groups>();
        
        public static List<Groups> Select()
        {
            MemoryManagement.FlushMemory();
            massiv.Clear();
            
            string Query = "SELECT id_groups, num_groups, specid_group, otdelid_groups, kurator_groups FROM groups";

            MySqlDataReader dataReader = MySQL.DataReader(Query);
            while (dataReader.Read())
            {
                massiv.Add(new Groups
                {
                    Id = dataReader.GetInt16("id_groups"),
                    Group = dataReader.GetInt16("num_groups"),
                    SpecId = dataReader.GetInt16("specid_group"),
                    OtdelId = dataReader.GetInt16("otdelid_groups"),
                    KuratorId = dataReader.GetInt16("kurator_groups")
                });
            }
            MySQL.connection.Close();
            return massiv;
        }

        public static bool GroupUP(List<Groups> groups)
        {
            try
            {
                MySQL.connection.Open();
                foreach (Groups group in groups)
                {
                    group.Group += 10;
                    MySqlCommand cmd = new MySqlCommand("UPDATE groups SET num_groups = '" + group.Group + "' WHERE id_groups = '" + group.Id + "'", MySQL.connection);
                    int numRowsUpdated = cmd.ExecuteNonQuery();
                }
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                MySQL.connection.Close();
            }
        }

        public static List<Groups> SelectPoOtdel(int OtdelId)
        {
            return massiv.FindAll(delegate(Groups group) { return group.OtdelId == OtdelId; });
        }

        public static Groups SelectByID(int Id)
        {
            return massiv.Find(delegate(Groups group) { return group.Id == Id; });
        }

        public static Groups SelectByNameGroup(int name)
        {
            return massiv.Find(delegate(Groups group) { return group.Group == name; });
        }

        public static void SelectedAction()
        {
            
        }
    }
}

