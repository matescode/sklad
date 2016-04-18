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
	/// Interaction logic for CustomerRecordWindow.xaml
	/// </summary>
	public partial class CustomerRecordWindow : Window
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CustomerRecordWindow"/> class.
		/// </summary>
		public CustomerRecordWindow()
		{
			InitializeComponent();
			this.noteDatePicker.SelectedDate = DateTime.Now;
			this.noteTextBox.Focus();
		}

		#region PROPERTIES

		/// <summary>
		/// Gets or sets the date.
		/// </summary>
		/// <value>The date.</value>
		public DateTime Date
		{
			get
			{
				return this.noteDatePicker.SelectedDate.Value;
			}
			set
			{
				this.noteDatePicker.SelectedDate = value;
			}
		}

		/// <summary>
		/// Gets or sets the note.
		/// </summary>
		/// <value>The note.</value>
		public string Note
		{
			get
			{
				return this.noteTextBox.Text;
			}
			set
			{
				this.noteTextBox.Text = value;
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
			this.Title = (windowMode == OpenWindowMode.Add) ? "Přidat záznam" : "Upravit záznam";
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
				MessageBox.Show("Nejsou vyplněny všechny údaje!", "Sklad", MessageBoxButton.OK, MessageBoxImage.Stop);
				
			}
		}

		/// <summary>
		/// Validates this instance.
		/// </summary>
		/// <returns></returns>
		private bool Validate()
		{
			return (this.noteDatePicker.SelectedDate.HasValue && this.noteTextBox.Text.Length > 0);
		}
	}
}
