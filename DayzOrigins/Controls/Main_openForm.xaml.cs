using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace DayzOrigins
{
	/// <summary>
	/// Interaction logic for Main_openForm.xaml
	/// </summary>
	public partial class Main_openForm : UserControl
	{
        private MainWindow Mw { get; set; }
        Boolean Authorization { get { return User.Authorization(user_login.Text, user_pass.Password); } set { Authorization = value; } }

		public Main_openForm(MainWindow mw)
		{
			this.InitializeComponent();
            Mw = mw;
            
		}
        /// <summary>
        /// Метод события, которое создаёт екземпляр окна SettingsForm. Позволяет задать параметры подключения к базе данных.
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new SettingsForm();
        }

        /// <summary>
        /// Метод события нажатия на клавишу. Проверяет тип нажатой клавиши. Если нажатая клавиша Enter проверят соединение к БД. Если соединение есть выполняет функцию авторизации.
        /// В противном случае выдаёт ошибку подключения. После выполнения всех операций отчишает память.
        /// </summary>
        private void LayoutRoot_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
                {
                    if (!MySQL.MySqlConnection()) { MessageBox.Show("MySQL server connection error!", "Connection error!", MessageBoxButton.OK, MessageBoxImage.Error); return; }
                        if (Authorization)
                        {
                        grid_opa.Background = Brushes.Green; Thread.Sleep(10);
                        this.LayoutRoot.Visibility = Visibility.Hidden;
                        Log.LogSend(Log.LogEvent.UserLogin);
                        Groups.Select();
                        Specials.Select();
                        Status.Select();
                        

                        Mw.List_One.ItemsSource = Otdels.Select();
                        Mw.List_One.ItemTemplate = (DataTemplate)FindResource("DataSelestLeft");
                        }
                        else
                        {
                            grid_opa.Background = Brushes.Red;
                            Log.LogSend(Log.LogEvent.UserLoginError);
                        }
                    
                }
                MemoryManagement.FlushMemory();
        }

        /// <summary>
        /// Устанавливает фокус на текстовое поле: user_login.
        /// </summary>
        internal void FocusLogin()
        {
            user_login.Focus();
        }
    }
}