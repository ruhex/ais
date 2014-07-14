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
using System.Windows.Shapes;

namespace DayzOrigins
{
	/// <summary>
	/// Interaction logic for DocGenerationForm.xaml
	/// </summary>
	public partial class DocGenerationForm : Window
	{
		public DocGenerationForm(UserControl DocControl)
		{
			this.InitializeComponent();

            this.LayoutRoot.Children.Add(DocControl);
			
			// Insert code required on object creation below this point.
		}
	}
}