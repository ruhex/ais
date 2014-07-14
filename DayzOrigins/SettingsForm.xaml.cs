using System.Windows;

namespace DayzOrigins
{
	/// <summary>
	/// Interaction logic for SettingsForm.xaml
	/// </summary>
	public partial class SettingsForm : Window
	{
		public SettingsForm()
		{
			this.InitializeComponent();
            this.LayoutRoot.Children.Add(new DB_Settings(this));
            this.ShowDialog();
			
			// Insert code required on object creation below this point.
		}
	}
}