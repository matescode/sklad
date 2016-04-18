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
    /// Interaction logic for SelectYearWindow.xaml
    /// </summary>
    public partial class SelectYearWindow : Window
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="SelectYearWindow"/> class.
		/// </summary>
        public SelectYearWindow()
        {
            InitializeComponent();
            this.yearTextBox.Text = DateTime.Now.Year.ToString();
			this.yearTextBox.Focus();
        }

		/// <summary>
		/// Gets or sets the year.
		/// </summary>
		/// <value>The year.</value>
        public int Year
        {
            get
            {
                return Int32.Parse(this.yearTextBox.Text);
            }
            set
            {
                this.yearTextBox.Text = value.ToString();
            }
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
            if (this.yearTextBox.Text.Length == 0)
            {
                MessageBox.Show("Není vyplněn rok!", "Sklad", MessageBoxButton.OK, MessageBoxImage.Stop);
                return false;
            }

            int dummyYear;
            if (!Int32.TryParse(this.yearTextBox.Text, out dummyYear))
            {
                MessageBox.Show("Rok je ve špatném formátu!", "Sklad", MessageBoxButton.OK, MessageBoxImage.Stop);
                return false;
            }

            return true;
        }
    }
}
