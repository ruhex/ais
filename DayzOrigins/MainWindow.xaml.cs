using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
namespace DayzOrigins
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public delegate void DelegateForTime(TextBlock label);
        static Boolean temp { get; set; }

        public MainWindow()
        {
            temp = false;
            
            InitializeComponent();

            if (!MySQL.MySqlConnection())
                MessageBox.Show("Настройте соединение с БД", "", MessageBoxButton.OK, MessageBoxImage.Warning);

            Main_openForm frm = new Main_openForm(this);
            this.gd_main.Children.Add(frm);
            listBox1.ContextMenu = null;
        }

        /// <summary>
        /// Метод события, которое создаёт екземпляр окна SettingsForm. Позволяет задать параметры подключения к базе данных.
        /// </summary>
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            new SettingsForm();
        }
        /// <summary>
        /// Тестовый метод выборки всех студентов из базы
        /// </summary>
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            DelStFullInfo();
            Students.SelectAll();
            listBox1.ItemTemplate = (DataTemplate)FindResource("DataTemplateSelect1");
            listBox1.ItemsSource = Students.massiv;
        }
        /// <summary>
        /// Выборка кураторов. ListBox' y List_One  в ItemsSource пишется массив который сформирован посредствам метода .Select() который реализован в классе Kyrator.
        /// ListBox' y List_One присваивается ItemTemplate DataSelestKur.
        /// </summary>
        private void MenuItem_Kyrator(object sender, RoutedEventArgs e)
        {
            DelStFullInfo();
            List_One.ItemsSource = Kyrator.Select();
            List_One.ItemTemplate = (DataTemplate)FindResource("DataSelestKur");
        }
        /// <summary>
        /// Выборка отделений. ListBox' y List_One  в ItemsSource пишется массив который сформирован посредствам метода .Select() который реализован в классе Otdels.
        /// ListBox' y List_One присваивается ItemTemplate DataSelestLeft.
        /// </summary>
        private void MenuItem_Otdel(object sender, RoutedEventArgs e)
        {
            DelStFullInfo();
            List_One.ItemsSource = Otdels.Select();
            List_One.ItemTemplate = (DataTemplate)FindResource("DataSelestLeft");
        }

        private void List_One_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBoxSet(sender);            
        }

        private void listBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            ListBoxSet(sender);
           
        }
        /// <summary>
        /// Работа с элементом в типе Object, который был выбран по средстам метода listBox1_SelectionChanged
        /// </summary>
        /// <param name="Sender">Выделенный объект</param>
        protected void ListBoxSet(object Sender)
        {
            ListBox temp = (ListBox)Sender;
            if (temp.SelectedItem == null) { return; }
            SelectInfo(temp.SelectedItem.GetType().Name, temp.SelectedItem, temp.Name);
        }

        //StudForm st = null;
        /// <summary>
        /// Основной метод для работы с выборками. Тут задаются стили ListBox's, переброс данных из ListBox в ListBox
        /// </summary>
        /// <param name="name">Тип выбранного элемента</param>
        /// <param name="selectedItem">Сам выбранный элемент</param>
        /// <param name="listboxName">Имя ListBox из которого была проведена операция</param>
        private void SelectInfo(string name, object selectedItem, string listboxName)
        {
            List_One.ContextMenu = null;
            listBox1.ContextMenu = null;
            switch (name)
            {
                case "Groups":
                    
                    if (listboxName == "listBox1")
                    {
                        listBox1.ContextMenu = ListBoxGroup;
                        Groups item0 = (Groups)listBox1.SelectedItem;
                        List_One.ItemsSource = listBox1.ItemsSource;
                        listBox1.ItemsSource = null; //------------------
                        List_One.SelectedItem = listBox1.SelectedItem; //-----------------------------------

                        listBox1.ItemsSource = Students.SelectByGroup((Groups)selectedItem);
                        List_One.ItemTemplate = (DataTemplate)FindResource("DataSelestGroup");
                        listBox1.ItemTemplate = (DataTemplate)FindResource("DataTemplateSelect1");
                    }
                    if (listboxName == "List_One")
                    {
                        List_One.ContextMenu = ListBoxGroup;
                        listBox1.ItemsSource = null;
                        listBox1.ItemsSource = Students.SelectByGroup((Groups)selectedItem);
                        listBox1.ItemTemplate = (DataTemplate)FindResource("DataTemplateSelect1");
                    }
                    break; 
                case "Students":
                    List_One.ContextMenu = ContextMenuStudents;
                    DelStFullInfo();                    
                    //if (st != null)
                        //st.Close();                        
                    if (listboxName == "listBox1")
                    {
                        List_One.ItemsSource = listBox1.ItemsSource;
                        Students item = (Students)listBox1.SelectedItem;
                        Students.SelectAllInfo(item);
                        listBox1.ItemsSource = null;
                        RootForm.Children.Add( new StudFulInfo(this, item));
                        //st = new StudForm(item);
                        List_One.ItemTemplate = (DataTemplate)FindResource("DataTemplateSelect5");
                    }
                    else
                    {
                        Students item = (Students)List_One.SelectedItem;
                        Students.SelectAllInfo(item);
                        RootForm.Children.Add(new StudFulInfo(this, item));
                        //st = new StudForm(item);
                    }

                    listBox1.SelectedItem = null;
                    //st.Show();
                    break;
                case "Otdels":
                    Otdels item1 = (Otdels)List_One.SelectedItem;
                    listBox1.ItemsSource = Groups.SelectPoOtdel(item1.Id);
                    listBox1.ItemTemplate = (DataTemplate)FindResource("DataSelestGroup");
                    break;
            }            
        }

        /// <summary>
        /// Отчистка RootForm от контролов студентов
        /// </summary>
        public void DelStFullInfo()
        {
            foreach (Control ctrl in RootForm.Children)
                if (ctrl.GetType().ToString() == "DayzOrigins.StudFulInfo") { RootForm.Children.Remove(ctrl); MemoryManagement.FlushMemory(); break; }         
        }

        /// <summary>
        /// Завершение сессии
        /// </summary>
        private void Log_Out(object sender, RoutedEventArgs e)
        {
            this.gd_main.Children.Add(new Main_openForm(this));
            Log.LogSend(Log.LogEvent.UserLogout);
        }

        /// <summary>
        /// Завершение работы программы
        /// </summary>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MySQL.MySqlConnection()) { Log.LogSend(Log.LogEvent.ApplicationExit); }
        }

        /// <summary>
        /// Выборка студентов по возрасту
        /// </summary>
        private void MenuCoutext_SelectAge(object sender, RoutedEventArgs e)
        {
            Groups item0 = (Groups)List_One.SelectedItem;
            listBox1.ItemsSource = null;
            listBox1.ItemsSource = Students.SelectPoAge(19);            
        }

        /// <summary>
        /// Метод события взятия в фокус элемента share_text. Реализует обнуление параметра Text данного элемента.
        /// </summary>
        private void share_text_GotFocus(object sender, RoutedEventArgs e)
        {
            this.share_text.Text = null;
        }

        /// <summary>
        /// Метод события нажатия на клавишу элемента share_text. Реализует проверку на тип нажатой клавиши. Если клавиша Enter: обнуляет ItemsSource элеметна listBox1,
        /// производит выборку студента по фамилии. Если не было найдено не одного студента то выдаёт соответствующие диалоговое окно.
        /// </summary>
        private void share_text_KeyDown(object sender, KeyEventArgs e)
        {
            DelStFullInfo();
            if (e.Key == Key.Return)
            {
                listBox1.ItemsSource = null;
                Students.SelectShare(share_text.Text);
                if (Students.massiv.Count == 0) { MessageBox.Show("Студентов с такой фамилией не найдено.", "Info dialog", MessageBoxButton.OK, MessageBoxImage.Information); return; } 
                listBox1.ItemTemplate = (DataTemplate)FindResource("DataTemplateSelect1");
                listBox1.ItemsSource = Students.massiv;
                share_text.Text = "Введите фамилию для поиска";

            }
        }

        private void test_Click(object sender, RoutedEventArgs e)
        {
            //DocGenerationForm DocForm = new DocGenerationForm();
            //DocForm.LayoutRoot.Children.Add(new DocSpravca("DD","123","AWFAG","GWEH4544","SSAH","SDFW2","QQA"));
           // DocForm.Show();

        }

        private void group_up(object sender, RoutedEventArgs e)
        {
            new GroupUp().Show();
        }

        private void DocClickVoencom(object sender, RoutedEventArgs e)
        {
            DocGenerationForm DocForm = new DocGenerationForm(new DocVoencom((Students)List_One.SelectedItem));
            DocForm.Show();
        }

        private void DocClickOthislit(object sender, RoutedEventArgs e)
        {
            DocGenerationForm DocForm = new DocGenerationForm(new DocOthislenie((Students)List_One.SelectedItem));
            DocForm.Show();
        }

        private void DocClickSpravca(object sender, RoutedEventArgs e)
        {
            DocGenerationForm DocForm = new DocGenerationForm(new DocSpravca((Students)List_One.SelectedItem));
            DocForm.Show();
        }

        private void About(object sender, RoutedEventArgs e)
        {
            new AboutBox().Show();
        }

        private void Group_Up_Click(object sender, RoutedEventArgs e)
        {
            if (Groups.GroupUP(Groups.Select()))
                MessageBox.Show("Операция успешно завершена", "True", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                MessageBox.Show("Операция не удалась", "False", MessageBoxButton.OK, MessageBoxImage.Error);
        }
      }
}
