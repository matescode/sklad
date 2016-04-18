using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Sklad.Windows
{
	/// <summary>
	/// Interaction logic for AboutWindow.xaml
	/// </summary>
	public partial class AboutWindow : Window
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="AboutWindow"/> class.
		/// </summary>
		public AboutWindow()
		{
			InitializeComponent();
			this.button1.Focus();
		}

		/// <summary>
		/// Handles the Click event of the button1 control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
		private void button1_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}
	}
}
