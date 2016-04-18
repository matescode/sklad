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
    /// Interaction logic for GroupWindow.xaml
    /// </summary>
    public partial class GroupWindow : Window
    {
        private Color color = Colors.Transparent;

		/// <summary>
		/// Initializes a new instance of the <see cref="GroupWindow"/> class.
		/// </summary>
        public GroupWindow()
        {
            InitializeComponent();
            this.nameTextBox.Focus();
        }

        #region PROPERTIES

		/// <summary>
		/// Gets or sets the name of the group.
		/// </summary>
		/// <value>The name of the group.</value>
        public string GroupName
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
		/// Gets or sets the color.
		/// </summary>
		/// <value>The color.</value>
        public Color Color
        {
            get
            {
                return this.color;
            }
            set
            {
                this.color = value;
                this.colorSchemaRectangle.Fill = new SolidColorBrush(this.Color);
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
			this.Title = (windowMode == OpenWindowMode.Add) ? "Přidat skupinu" : "Upravit skupinu";
			this.Icon = (windowMode == OpenWindowMode.Add) ? (Application.Current.Resources["icon_add"] as ImageSource) : (Application.Current.Resources["icon_edit"] as ImageSource);
            return this.ShowDialog();
        }

		/// <summary>
		/// Validates this instance.
		/// </summary>
		/// <returns></returns>
        public bool Validate()
        {
            return (this.nameTextBox.Text.Length > 0 && this.Color != Colors.Transparent);
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
		/// Handles the Click event of the chooseColorButton control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void chooseColorButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.ColorDialog dialog = new System.Windows.Forms.ColorDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.Color = new Color() { A = dialog.Color.A, R = dialog.Color.R, G = dialog.Color.G, B = dialog.Color.B };
            }
        }
    }
}
