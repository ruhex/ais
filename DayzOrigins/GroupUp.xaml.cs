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
	/// Interaction logic for GroupUp.xaml
	/// </summary>
	public partial class GroupUp : Window
	{
		public GroupUp()
		{
			this.InitializeComponent();
		}

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Students.GroupUp(Convert.ToInt32(group_new.Text), Students.SelectByGroup(Groups.SelectByNameGroup(Convert.ToInt32(group_old.Text)))))
                MessageBox.Show("Операция успешно завершена", "True", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                MessageBox.Show("Операция не удалась", "False", MessageBoxButton.OK, MessageBoxImage.Error);
        }
	}
}