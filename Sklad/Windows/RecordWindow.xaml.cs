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
    /// Interaction logic for RecordWindow.xaml
    /// </summary>
    public partial class RecordWindow : Window
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="RecordWindow"/> class.
		/// </summary>
        public RecordWindow()
        {
            InitializeComponent();
            this.dateDatePicker.Focus();
            this.dateDatePicker.SelectedDate = DateTime.Now;
        }

        #region PROPERTIES

		/// <summary>
		/// Gets or sets the type of the record.
		/// </summary>
		/// <value>The type of the record.</value>
        public SkladCore.Model.RecordType RecordType
        {
            get
            {
                if (this.takingRadioButton.IsChecked == true)
                {
                    return SkladCore.Model.RecordType.Taking;
                }
                else
                {
                    return SkladCore.Model.RecordType.Issue;
                }
            }
            set
            {
                if (value == SkladCore.Model.RecordType.Taking)
                {
                    this.takingRadioButton.IsChecked = true;
                }
                else
                {
                    this.issueRadioButton.IsChecked = true;
                }
            }
        }

		/// <summary>
		/// Gets or sets the date.
		/// </summary>
		/// <value>The date.</value>
        public DateTime Date
        {
            get
            {
                return this.dateDatePicker.SelectedDate.Value;
            }
            set
            {
                this.dateDatePicker.SelectedDate = value;
            }
        }

		/// <summary>
		/// Gets or sets the amount.
		/// </summary>
		/// <value>The amount.</value>
        public int Amount
        {
            get
            {
                return Int32.Parse(this.amountTextBox.Text);
            }
            set
            {
                this.amountTextBox.Text = value.ToString();
            }
        }

		/// <summary>
		/// Gets or sets the price.
		/// </summary>
		/// <value>The price.</value>
        public double Price
        {
            get
            {
                return Double.Parse(this.priceTextBox.Text);
            }
            set
            {
                this.priceTextBox.Text = value.ToString();
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
        }

		/// <summary>
		/// Validates this instance.
		/// </summary>
		/// <returns></returns>
        private bool Validate()
        {
            int iDummy;
            double dDummy;

            if (!Int32.TryParse(this.amountTextBox.Text, out iDummy))
            {
                MessageBox.Show("Množství je ve špatném tvaru!", "Sklad", MessageBoxButton.OK, MessageBoxImage.Stop);
                this.amountTextBox.Focus();
                return false;
            }

            if (!Double.TryParse(this.priceTextBox.Text, out dDummy))
            {
                MessageBox.Show("Cena je ve špatném tvaru!", "Sklad", MessageBoxButton.OK, MessageBoxImage.Stop);
                this.priceTextBox.Focus();
                return false;
            }

            
            if (!(this.priceTextBox.Text.Length > 0 && this.amountTextBox.Text.Length > 0 && this.dateDatePicker.SelectedDate.HasValue)) 
            {
                MessageBox.Show("Nejsou vyplněny všechny parametry!", "Sklad", MessageBoxButton.OK, MessageBoxImage.Stop);
                return false;
            }

            return true;
        }
    }
}
