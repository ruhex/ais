using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;//----
using MySql.Data.MySqlClient;

namespace DayzOrigins
{
	/// <summary>
	/// Interaction logic for StudFulInfo.xaml
	/// </summary>
	public partial class StudFulInfo : UserControl
	{
        private string Lname { get; set; }
        private string Fname { get; set; }
        private string Sname { get; set; }

        private MainWindow MainForm { get; set; }
        private int Id { get { return Student.Id; } set { } }
        public int size { get; set; }

        Students Student;
        

        public StudFulInfo(MainWindow mw, Students obj)
        {
            


            MainForm = mw;
            mw.DelStFullInfo();
            this.InitializeComponent();

            this.fio.IsEnabled = false;
            this.propiska_st.IsEnabled = false;
            this.faktAddr_st.IsEnabled = false;
            this.PassportVidan_st.IsEnabled = false;
            this.PassportNum_st.IsEnabled = false;
            this.snils_st.IsEnabled = false;
            this.birth_st.IsEnabled = false;
            this.baza_st.IsEnabled = false;
            this.forma_st.IsEnabled = false;
            this.datePodachi_st.IsEnabled = false;
            this.add_foto.IsEnabled = false;
            this.status_st.IsEnabled = false;
            combo_st.ItemsSource = Status.massiv;
            Student = obj;
            ImageSelect();
            LayoutRoot.DataContext = Student; // для биндинга

            #region Привязка события LostFocus для обновления контента
            this.snils_st.LostFocus += (s, e) => { SavData(Students.EditUpdate(Id, snils_st.Text, snils_st.Name)); };
            this.PassportNum_st.LostFocus += (s, e) => { SavData(Students.EditUpdate(Id, PassportNum_st.Text, PassportNum_st.Name)); };
            this.PassportVidan_st.LostFocus += (s, e) => { SavData(Students.EditUpdate(Id, PassportVidan_st.Text, PassportVidan_st.Name)); };
            this.propiska_st.LostFocus += (s, e) => { SavData(Students.EditUpdate(Id, propiska_st.Text, propiska_st.Name)); };
            this.faktAddr_st.LostFocus += (s, e) => { SavData(Students.EditUpdate(Id, faktAddr_st.Text, faktAddr_st.Name)); };
            this.forma_st.LostFocus += (s, e) => { SavData(Students.EditUpdate(Id, forma_st.Text, forma_st.Name)); };
            this.birth_st.LostFocus += (s, e) => { SavData(Students.EditUpdate(Id, birth_st.Text, birth_st.Name)); };
            this.datePodachi_st.LostFocus += (s, e) => { SavData(Students.EditUpdate(Id, datePodachi_st.Text, datePodachi_st.Name)); };
            this.fio.KeyDown += (s, e) => 
            {if (Lname != null)
                SavData(Students.EditUpdate(Id, Lname, "lname_st"));
            if (Sname != null)
                SavData(Students.EditUpdate(Id, Sname, "sname_st"));
            if (Fname != null)
                SavData(Students.EditUpdate(Id, Fname, "fname_st"));
            };
            //this.oplata_st.LostFocus += (s, e) => { SavData(Students.EditUpdate(Id, oplata_st.Text, oplata_st.Name)); };
            #endregion
        }

        #region Метод отображения обновления данных
        private void SavData(bool status)
        {
            switch (status)
            {
                case true:
                    this.SaveData.Foreground = Brushes.Green;
                    this.SaveData.Text = "Данные обновлены!";
                    break;

                case false:
                    this.SaveData.Foreground = Brushes.Red;
                    this.SaveData.Text = "Ошибка обновления!";
                    break;
            }
        }
        #endregion

        #region Метод выборки изображения из базы данных
        public void ImageSelect()
        {
            if (true)
            {
                byte[] array = new byte[0];
                MySqlDataReader dataReaderTemp = MySQL.ImageSelect(Id);
                while (dataReaderTemp.Read())
                {
                    if (dataReaderTemp.GetInt32("photoSize_st") != 0)
                    {
                        int _size = dataReaderTemp.GetInt32("photoSize_st");
                        {
                            Array.Resize(ref array, _size);

                            dataReaderTemp.GetBytes(0, 0, array, 0, _size);

                        }
                    }
                    else { MySQL.connection.Close(); return; }
                }
                pic.Source = ImageFromBuffer(array);
                MySQL.connection.Close();
            }
        }

        
        
        private BitmapImage ImageFromBuffer(Byte[] bytes)
        {
            BitmapImage image = new BitmapImage();
            MemoryStream stream = new MemoryStream(bytes);
            image.BeginInit();
            image.StreamSource = stream;
            image.EndInit();
            return image;
        }
        #endregion

        public void ImageUpload()
        {
            try
            {
                OpenFileDialog FileOpen = new OpenFileDialog();
                FileOpen.ShowDialog();

                FileStream FileRead = new FileStream(FileOpen.FileName, FileMode.Open);

                Byte[] array = new Byte[FileRead.Length];
                FileRead.Read(array, 0, (int)FileRead.Length);
                this.size = (int)FileRead.Length;
                FileRead.Close();
                
                MySQL.ImageUpload(Id, size, array);
                pic.Source = ImageFromBuffer(array);
            }
            catch (Exception ex)
            {
                MySQL.connection.Close();
                MessageBox.Show(ex.Message);
            }
        }


        public void zapilit_Click(object sender, RoutedEventArgs e)
        {
            ImageUpload();            
        }

        private void CloseStudFormButton_Click(object sender, RoutedEventArgs e)
        {
            MainForm.DelStFullInfo();
        }

        private void textBox1_GotFocus(object sender, RoutedEventArgs e)
        {
            fio.Template = (ControlTemplate)FindResource("TextBoxControlTemplate1");
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                fio.Template = (ControlTemplate)FindResource("TextBoxControlTemplate2");
            }
        }

        private void textBox1_LostFocus(object sender, RoutedEventArgs e)
        {
            fio.Template = (ControlTemplate)FindResource("TextBoxControlTemplate2");
        }

        private void LastName_text_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            Lname = tb.Text;
        }

        private void FirstName_text_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            Fname = tb.Text;
        }

        private void SecondName_text_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            Sname = tb.Text;
        }

        private void edit_data_Checked(object sender, RoutedEventArgs e)
        {
            if (edit_data.IsChecked.Value == true)
            {
                this.fio.IsEnabled = true;
                this.propiska_st.IsEnabled = true;
                this.faktAddr_st.IsEnabled = true;
                this.PassportVidan_st.IsEnabled = true;
                this.PassportNum_st.IsEnabled = true;
                this.snils_st.IsEnabled = true;
                this.birth_st.IsEnabled = true;
                this.baza_st.IsEnabled = true;
                this.forma_st.IsEnabled = true;
                this.datePodachi_st.IsEnabled = true;
                this.add_foto.IsEnabled = true;
                this.status_st.IsEnabled = true;
            }
            else
            {
                this.fio.IsEnabled = false;
                this.propiska_st.IsEnabled = false;
                this.faktAddr_st.IsEnabled = false;
                this.PassportVidan_st.IsEnabled = false;
                this.PassportNum_st.IsEnabled = false;
                this.snils_st.IsEnabled = false;
                this.birth_st.IsEnabled = false;
                this.baza_st.IsEnabled = false;
                this.forma_st.IsEnabled = false;
                this.datePodachi_st.IsEnabled = false;
                this.add_foto.IsEnabled = false;
                this.status_st.IsEnabled = false;
            }

        }

        private void edit_data_Click(object sender, RoutedEventArgs e)
        {
            edit_data_Checked(sender, e);
        }

        private void combo_st_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Status status = (Status)combo_st.SelectedItem;
            SavData(Students.EditUpdate(Id, status.ID.ToString(), "status_st"));
            status_st.Text = status.Name;
            combo_st.Visibility = System.Windows.Visibility.Hidden;
        }

        private void status_st_GotFocus(object sender, RoutedEventArgs e)
        {
            combo_st.Visibility = System.Windows.Visibility.Visible;
        }

        
    }
}