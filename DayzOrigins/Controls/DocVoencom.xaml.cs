using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DayzOrigins
{
    /// <summary>
    /// Interaction logic for DocSpravca.xaml
    /// </summary>
    public partial class DocVoencom : UserControl
    {
        public DocVoencom(Students std)
        {
            this.InitializeComponent();
            this.datarozdenia_text.Text = std.Birth.ToString().Remove(4);
            this.lname_text.Text = std.LastName;
            this.fname_text.Text = std.FirstName;
            this.sname_text.Text = std.SecondName;
            this.oplata_text.Text = std.Oplata;
            this.num_spec_text.Text = std.SpecCode;
            this.name_spec_text.Text = std.SpecName;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DocGereration(Environment.CurrentDirectory + @"\DOC\DocVoencom.docx");
        }

        public void DocGereration(string FileName)
        {
            var WordApp = new Microsoft.Office.Interop.Word.Application();
            WordApp.Visible = false;
            var doc = WordApp.Documents.Open(FileName);
            string fio = this.lname_text.Text + " " +
                this.fname_text.Text + " " +
                this.sname_text.Text; 
            DocForm.Form("{FIO}", fio, doc);
            DocForm.Form("{Berzday}", this.datarozdenia_text.Text, doc);
            DocForm.Form("{NUM}", this.num_spec_text.Text, doc);
            DocForm.Form("{SPEH}", this.name_spec_text.Text, doc);
            DocForm.Form("{OSNOVA}", this.oplata_text.Text, doc);
            DocForm.Form("{N}", this.num_pricaz_text.Text, doc);
            DocForm.Form("{D}", this.data_pricaz_text.Text, doc);
            DocForm.Form("{SROC}", this.oconhanie_text.Text, doc);

            WordApp.Visible = true;
        }
	}
}