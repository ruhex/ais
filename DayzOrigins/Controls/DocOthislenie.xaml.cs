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
	public partial class DocOthislenie : UserControl
	{
		public DocOthislenie()
		{
			this.InitializeComponent();
		}

        public DocOthislenie(Students std)
        {
            this.InitializeComponent();
            this.datarozdenia_text.Text = std.Birth;
            this.lname_text.Text = std.LastName;
            this.fname_text.Text = std.FirstName;
            this.sname_text.Text = std.SecondName;            
            this.num_spec_text.Text = std.SpecCode;
            this.name_spec_text.Text = std.SpecName;
        }

        public void DocGereration(string FileName)
        {
            var WordApp = new Microsoft.Office.Interop.Word.Application();
            WordApp.Visible = false;
            var doc = WordApp.Documents.Open(FileName);

            DocForm.Form("{Data}", this.data_pricaz_text.Text, doc);
            DocForm.Form("{Nember}", this.num_pricaz_text.Text, doc);
            DocForm.Form("{LastName}", this.lname_text.Text, doc);
            DocForm.Form("{FirstName}", this.fname_text.Text, doc);
            DocForm.Form("{SecondName}", this.sname_text.Text, doc);
            DocForm.Form("{DataBerzday}", this.datarozdenia_text.Text, doc);
            DocForm.Form("{GroupName}", this.num_spec_text.Text, doc);
            DocForm.Form("{SpecName}", this.name_spec_text.Text, doc);
            DocForm.Form("{Prihina}", this.prihina_text.Text, doc);
            DocForm.Form("{Osnovanie}", this.osnovanie_text.Text, doc);

            WordApp.Visible = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DocGereration(Environment.CurrentDirectory + @"\DOC\DocOthislit.docx");
        }
	}
}