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
    /// Interaction logic for CardWindow.xaml
    /// </summary>
    public partial class CardWindow : Window
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="CardWindow"/> class.
		/// </summary>
        public CardWindow()
        {
            InitializeComponent();
            this.nameTextBox.Focus();
        }

        #region PROPERTIES

		/// <summary>
		/// Gets or sets the name of the card.
		/// </summary>
		/// <value>The name of the card.</value>
        public string CardName
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
		/// Gets or sets the code JK.
		/// </summary>
		/// <value>The code JK.</value>
        public string CodeJK
        {
            get
            {
                return this.codeJKTextBox.Text;
            }
            set
            {
                this.codeJKTextBox.Text = value;
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
			this.Title = (windowMode == OpenWindowMode.Add) ? "Přidat kartu" : "Upravit kartu";
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
                MessageBox.Show("Nejsou vyplněny všechny parametry!", "Sklad", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }

		/// <summary>
		/// Validates this instance.
		/// </summary>
		/// <returns></returns>
        private bool Validate()
        {
            return (this.nameTextBox.Text.Length > 0 && this.codeJKTextBox.Text.Length > 0);
        }
    }
}
