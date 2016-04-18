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
	/// Interaction logic for CustomerWindow.xaml
	/// </summary>
	public partial class CustomerWindow : Window
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CustomerWindow"/> class.
		/// </summary>
		public CustomerWindow()
		{
			InitializeComponent();
			this.nameTextBox.Focus();
			this.startDatePicker.SelectedDate = DateTime.Now;
		}

		#region PROPERTIES

		/// <summary>
		/// Gets or sets the identifying name of the element. The name provides a reference so that code-behind, such as event handler code, can refer to a markup element after it is constructed during processing by a XAML processor.
		/// </summary>
		/// <value></value>
		/// <returns>The name of the element. The default is an empty string.</returns>
		public string CustomerName
		{
			get
			{
				return this.nameTextBox.Text;
			}
			set
			{
				this.nameTextBox.Text = value;
			}
		}

		/// <summary>
		/// Gets or sets the phone.
		/// </summary>
		/// <value>The phone.</value>
		public string Phone
		{
			get
			{
				return this.phoneTextBox.Text;
			}
			set
			{
				this.phoneTextBox.Text = value;
			}
		}

		/// <summary>
		/// Gets or sets the furnace.
		/// </summary>
		/// <value>The furnace.</value>
		public string Furnace
		{
			get
			{
				return this.furnaceTextBox.Text;
			}
			set
			{
				this.furnaceTextBox.Text = value;
			}
		}

		/// <summary>
		/// Gets the date.
		/// </summary>
		/// <value>The date.</value>
		public string Date
		{
			get
			{
				if (this.startDatePicker.SelectedDate.HasValue)
				{
					return String.Format("{0}.{1}.{2}", this.startDatePicker.SelectedDate.Value.Day, this.startDatePicker.SelectedDate.Value.Month, this.startDatePicker.SelectedDate.Value.Year);
				}
				else
				{
					return "";
				}
			}
			set
			{
				this.startDatePicker.SelectedDate = Convert.ToDateTime(value);
			}
		}

		/// <summary>
		/// Gets or sets the address.
		/// </summary>
		/// <value>The address.</value>
		public string Address
		{
			get
			{
				return this.addressTextBox.Text;
			}
			set
			{
				this.addressTextBox.Text = value;
			}
		}

		#endregion PROPERTIES

		/// <summary>
		/// Opens the window in mode.
		/// </summary>
		/// <param name="windowMode">The window mode.</param>
		/// <returns></returns>
		public bool? OpenWindowInMode(OpenWindowMode windowMode)
		{
			this.Title = (windowMode == OpenWindowMode.Add) ? "Přidat zákazníka" : "Upravit zákazníka";
			this.Icon = (windowMode == OpenWindowMode.Add) ? (Application.Current.Resources["icon_add"] as ImageSource) : (Application.Current.Resources["icon_edit"] as ImageSource);
			return this.ShowDialog();
		}

		/// <summary>
		/// Handles the Click event of the cancelButton control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
		private void cancelButton_Click(object sender, RoutedEventArgs e)
		{
			this.DialogResult = false;
			this.Close();
		}

		/// <summary>
		/// Handles the Click event of the okButton control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
		private void okButton_Click(object sender, RoutedEventArgs e)
		{
			if (this.Validate())
			{
				this.DialogResult = true;
				this.Close();
			}
			else
			{
				MessageBox.Show("Není vyplněno jméno!", "Sklad", MessageBoxButton.OK, MessageBoxImage.Stop);
				this.nameTextBox.Focus();
			}
		}

		/// <summary>
		/// Validates this instance.
		/// </summary>
		/// <returns></returns>
		private bool Validate()
		{
			return (this.nameTextBox.Text.Length > 0);
		}
	}
}
