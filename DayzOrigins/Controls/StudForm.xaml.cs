using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using MySql.Data.MySqlClient;

namespace DayzOrigins.Controls
{
    public partial class StudForm : Window
    {
        private int Id { get { return Student.Id; } set { } }
        public int size { get; set; }

        Students Student;

        public StudForm(Students obj)
        {
            this.InitializeComponent();
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
    }
}
