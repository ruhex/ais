using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace DayzOrigins
{
    public class Students
    {
        public int Id { get; set; }             // Универсальный идентификатор студента
        public int GroupID { get; set; }        // Учебная группа студента id!!!!!
        public string FirstName { get; set; }   // Фамилия
        public string LastName { get; set; }    // Имя
        public string SecondName { get; set; }  // Отчество
        public int Age { get; set; }            // Возраст
        public string Sex { get; set; }         // Пол М/Ж

        public string FIO { get { return LastName + " " + FirstName + " " + SecondName; } set { } }

        public Groups GroupRef { get; set; }

        public Status StatusRef { get; set; }
        
        public static List<Students> massiv = new List<Students>();

        public static List<Students> SelectAll()
        {
            string Query = "SELECT id_st, fname_st, lname_st, sname_st, sex_st, birth_st, status_st, group_st FROM study";
            return Select(Query);
        }

        /// <summary>
        /// Выборка студента по фамилии (Lastname)
        /// </summary>
        /// <param name="name">Lastname</param>
        public static List<Students> SelectShare(string name)
        {
            string Query = "SELECT id_st, fname_st, lname_st, sname_st, sex_st, birth_st, group_st, status_st FROM study WHERE lname_st = '" + name + "' AND status_st = 4 ";
            return Select(Query);
        }

        public static List<Students> SelectByGroup(Groups group)
        {
            string Query = "SELECT id_st, fname_st, lname_st, sname_st, sex_st, group_st, birth_st, status_st FROM study WHERE group_st = " + group.Id + " AND status_st = 4";
            return Select(Query);
        }

        public static bool GroupUp(int new_group, List<Students> list)
        {
            try
            {
                Groups group = Groups.SelectByNameGroup(new_group);

                MySQL.connection.Open();
                foreach (var temp in list)
                {
                    MySqlCommand cmd = new MySqlCommand("UPDATE study SET group_st = '" + group.Id + "' WHERE id_st = '" + temp.Id + "'", MySQL.connection);
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

        /// <summary>
        /// Статический метод Select. Реализует выборку студентов по входной строке запроса.
        /// </summary>
        /// <param name="Query">Строка запроса</param>
        /// <returns>List</returns>
        static List<Students> Select(string Query)
        {
            massiv.Clear();

            MySqlDataReader dataReader = MySQL.DataReader(Query);
            int groupID = 0;
            int statusID = 0;
            while (dataReader.Read())
            {
                statusID = dataReader.GetInt16("status_st");
                groupID = dataReader.GetInt16("group_st"); 
                
                massiv.Add(new Students
                {
                    Id = dataReader.GetInt16("id_st"),
                    GroupID = groupID,
                    FirstName = dataReader.GetString("fname_st"),
                    LastName = dataReader.GetString("lname_st"),
                    SecondName = dataReader.GetString("sname_st"),
                    Age = age(Date.GetDate(), dataReader.GetString("birth_st")),
                    Sex = dataReader.GetString("sex_st"),
                    GroupRef = Groups.SelectByID(groupID),
                    StatusRef = Status.SelectPoID(statusID)
                });
            }
            MySQL.connection.Close();
            return massiv;
        }
        /// <summary>
        /// Возрашает возраст студента
        /// </summary>
        /// <param name="i">возраст</param>
        public static List<Students> SelectPoAge(int i)
        {
            return massiv.FindAll(delegate(Students std) { return std.Age == i; });
        }

        /// <summary>
        /// Публичный статический метод SelectPoGroup. Реализует выбоку студентов по группам. На вход принимает 1 параметр.
        /// </summary>
        /// <param name="i">Номер (ID?) группы</param>
        public static List<Students> SelectPoGroup(Groups g)
        {
            return massiv.FindAll(delegate(Students group) { return group.GroupID == g.Id; });
        }

        

        /// <summary>
        /// Публичный статический метод SelectAllInfo. Производит полную выборку информации о студенте. Студент подаётся на вход.
        /// </summary>
        /// <param name="student">Объект типа Students</param>
        public static void SelectAllInfo(Students student)
        {
            string Query = "SELECT * FROM study WHERE id_st =" + student.Id;
            MySqlDataReader dataReader = MySQL.DataReader(Query);
                       
            while (dataReader.Read())
            {
                student.Id = dataReader.GetInt16("id_st");
                student.FirstName = dataReader.GetString("fname_st");
                student.LastName = dataReader.GetString("lname_st");
                student.SecondName = dataReader.GetString("sname_st");
                student.PassportNum = dataReader.GetString("PassportNum_st");
                student.PassportVidan = dataReader.GetString("PassportVidan_st");
                student.Propiska = dataReader.GetString("propiska_st");
                student.FaktAddr = dataReader.GetString("faktAddr_st");
                student.Snils = dataReader.GetString("snils_st");
                student.Sex = dataReader.GetString("sex_st");
                student.Birth = dataReader.GetString("birth_st");
                student.Baza = dataReader.GetString("baza_st");
                student.Obshez = dataReader.GetString("obshez_st");
                student.Srball = dataReader.GetString("srball_st");
                student.StatusID = dataReader.GetInt16("status_st");
                student.Forma = dataReader.GetString("forma_st");
                student.Oplata = dataReader.GetString("oplata_st");
                student.DatePodachi = dataReader.GetString("datePodachi_st");
                student.GroupID = dataReader.GetInt16("group_st");
                student.Podgruppa = dataReader.GetInt16("podgruppa_st");
                student.Phone = dataReader.GetInt16("phone_st");
                student.Idzakaz = dataReader.GetInt16("idzakaz_st");
                student.Temp = dataReader.GetString("temp_st");
                student.Age = age(Date.GetDate(), dataReader.GetString("birth_st"));
            }       
            MySQL.connection.Close();
        }

        /// <summary>
        /// Публичный статический метод EditUpdate. Реализует обновление данных студента. На вход подаются 3 параметра
        /// </summary>
        /// <param name="Id">id студента</param>
        /// <param name="Edit">Изменёные входные данные</param>
        /// <param name="name">Имя поля которое нужно обновить</param>
        public static bool EditUpdate(int Id, string Edit, string name)
        {
            try
            {
                MySQL.connection.Open();
                MySqlCommand cmd = new MySqlCommand("UPDATE study SET " + name + " = '" + Edit + "' WHERE id_st = '" + Id + "'", MySQL.connection);
                int numRowsUpdated = cmd.ExecuteNonQuery();
                MySQL.connection.Close();
                return true;
            }
            catch 
            {
                MySQL.connection.Close();
                return false;
            }
        }

        /// <summary>
        /// Преобразование даты в новый формат для расчёта возраста.
        /// </summary>
        /// <param name="age">Строка даты. Формат: YYYY-MM-DD</param>
        static private int age(string age)
        {
            string[] tmp = age.Split('-');
            age = "";
            foreach (string s in tmp)
            {
                age += s;
            }
            return Convert.ToInt32(age);
        }

        /// <summary>
        /// Расчёт возраста по преобразованным стокам даты.
        /// </summary>
        /// <param name="arg1">Строка даты, ТЕКУЩЯЯ преобразованная методом age(string age)</param>
        /// <param name="arg2">Строка даты, РОЖДЕНИЯ преобразованная методом age(string age)</param>
        static private int age(string arg1, string arg2)
        {
            return Convert.ToInt32((age(arg1) - age(arg2)).ToString().Remove(2));
        }

        public string Temp { get; set; }

        public int Idzakaz { get; set; }

        public int Podgruppa { get; set; }

        public int Phone { get; set; }

        public string Photo { get; set; }

        public string DatePodachi { get; set; }

        public string Forma { get; set; }

        public string Oplata { get; set; }

        public int StatusID { get; set; }

        public string Srball { get; set; }

        public string Obshez { get; set; }

        public string Baza { get; set; }

        public string Birth { get; set; }

        public string Snils { get; set; }

        public string PassportNum { get; set; }

        public string Propiska { get; set; }

        public string PassportVidan { get; set; }

        public string FaktAddr { get; set; }

        public string SpecName { get { return GroupRef.SpecName; } set { } }

        public string SpecCode { get { return GroupRef.SpecCode; } set { } }

        public string SpecQual { get { return GroupRef.SpecQual; } set { } }

        public int PhotoSize { get; set; }

        
    }
}

