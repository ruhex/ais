using System.Windows;
using System.Windows.Controls;

namespace DayzOrigins
{
	/// <summary>
	/// Interaction logic for DB_Settings.xaml
	/// </summary>
	public partial class DB_Settings : UserControl
	{
        /* зачем дублировать параметры, они есть в классе MySQL, перенес туда обращение к настройкам
        static string DB { get { return Settings.Default.db; } set { Settings.Default.db = value; } }
        static string Server { get { return Settings.Default.server; } set { Settings.Default.server = value; } }
        static string Login { get { return Settings.Default.login; } set { Settings.Default.login = value; } }
        static string Password { get { return Settings.Default.passwd; } set { Settings.Default.passwd = value; } }
        */

        private static SettingsForm FormS { get; set; }

		public DB_Settings(SettingsForm frm)
		{
			this.InitializeComponent();
            // переделал на использование параметров из класса MySQL
            FormS = frm;
            DB_db.Text = MySQL.DB;
            DB_user.Text = MySQL.User;
            DB_host.Text = MySQL.Host;
            DB_passwd.Password = MySQL.Pass;
		}

        private void Button_OK_Click(object sender, RoutedEventArgs e)
        {            
            if (!(DB_db.Text == "" && DB_host.Text == "" && DB_user.Text == "" && DB_passwd.Password == ""))
            {
                //MySQL MysqlSetCon = new MySQL();
                if (MySQL.MySqlConnection(DB_user.Text, DB_passwd.Password, DB_host.Text, DB_db.Text, true))
                {
                    MessageBox.Show("Соеденение с базой данных установленно", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    FormS.Close();
                }
                else
                {
                    MessageBox.Show("Не удалось установить соединение с базой данных!", "", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                
            }
            else
            {
                MessageBox.Show("Вы ввели не все данные!", "", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            if (DB_save.IsChecked == true)
            {
                MySQL.DB = DB_db.Text;
                MySQL.Host = DB_host.Text;
                MySQL.User = DB_user.Text;
                MySQL.Pass = DB_passwd.Password;
                Settings.Default.Save();
            }


        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            FormS.Close();
        }
	}
}