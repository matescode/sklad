using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Xps;

using Fluent;

using SkladCore;
using SkladCore.Model;
using System.ComponentModel;
using System.Printing;

namespace Sklad.Windows
{
    /// <summary>
    /// Open-window-mode
    /// </summary>
    public enum OpenWindowMode
    {
        /// <summary>
        /// Add-mode
        /// </summary>
        Add,
        /// <summary>
        /// Edit-mode
        /// </summary>
        Edit
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RibbonWindow
    {
        private const int RecordsPerPage = 45;

        #region ATTRIBUTES

        private Group selectedGroup = null;

        private Card selectedCard = null;

        private Record selectedRecord = null;

        private int selectedStockYear = 0;

        private Customer selectedCustomer = null;

        private bool askToExit = true;

        #endregion ATTRIBUTES

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            this.Init();
        }

        /// <summary>
        /// Inits this instance.
        /// </summary>
        private void Init()
        {
            if (!Directory.Exists("Data") || !File.Exists(System.IO.Path.Combine("Data", "data.xml")))
            {
                MessageBox.Show("Nelze načíst data!\nProgram bude ukončen!", "Sklad", MessageBoxButton.OK, MessageBoxImage.Stop);
                this.askToExit = false;
                this.Close();
                return;
            }
            DataManager.Instance.Load();
            this.RefreshGroups();
            this.RefreshCustomers();
            this.ActivateMenuButtons();
        }

        /// <summary>
        /// Refreshes the groups.
        /// </summary>
        private void RefreshGroups()
        {
            this.groupsListBox.ItemsSource = DataManager.Instance.Groups;
        }

        /// <summary>
        /// Refreshes the customers.
        /// </summary>
        private void RefreshCustomers()
        {
            this.customersListBox.ItemsSource = DataManager.Instance.Customers;
        }

        /// <summary>
        /// Activates the menu buttons.
        /// </summary>
        private void ActivateMenuButtons()
        {
            this.editGroupButton.IsEnabled = (this.selectedGroup != null);
            this.deleteGroupButton.IsEnabled = (this.selectedGroup != null);

            this.addCardButton.IsEnabled = (this.selectedGroup != null);
            this.editCardButton.IsEnabled = (this.selectedCard != null);
            this.deleteCardButton.IsEnabled = (this.selectedCard != null);

            this.addRecordButton.IsEnabled = (this.selectedCard != null);
            this.removeRecordButton.IsEnabled = (this.selectedCard != null);

            this.editCustomerButton.IsEnabled = (this.selectedCustomer != null);
            this.removeCustomerButton.IsEnabled = (this.selectedCustomer != null);
            this.addCustomerRecordButton.IsEnabled = (this.selectedCustomer != null);
            this.removeCustomerRecordButton.IsEnabled = (this.selectedCustomer != null);
        }

        /// <summary>
        /// Handles the Click event of the exitButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Handles the Closing event of the RibbonWindow control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.CancelEventArgs"/> instance containing the event data.</param>
        private void RibbonWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.askToExit)
            {
                if (MessageBox.Show("Skutečně chcete ukončit program?", "Sklad", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    if (MessageBox.Show("Chcete uložit provedené změny?", "Sklad", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        this.Save();
                    }
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the saveButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            this.Save();
        }

        /// <summary>
        /// Saves this instance.
        /// </summary>
        private void Save()
        {
            DataManager.Instance.Save();
            MessageBox.Show("Data byla uložena", "Sklad", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Handles the Click event of the aboutButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void aboutButton_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow window = new AboutWindow();
            window.ShowDialog();
        }

        /// <summary>
        /// Handles the Click event of the addGroupButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void addGroupButton_Click(object sender, RoutedEventArgs e)
        {
            GroupWindow window = new GroupWindow();
            if (window.OpenWindowInMode(OpenWindowMode.Add) == true)
            {
                DataManager.Instance.AddGroup(new Group(window.GroupName, window.Color));
                this.RefreshGroups();
            }
        }

        /// <summary>
        /// Handles the Click event of the editGroupButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void editGroupButton_Click(object sender, RoutedEventArgs e)
        {
            GroupWindow window = new GroupWindow();
            window.GroupName = this.selectedGroup.Name;
            window.Color = this.selectedGroup.Color;
            if (window.OpenWindowInMode(OpenWindowMode.Edit) == true)
            {
                this.selectedGroup.Name = window.GroupName;
                this.selectedGroup.Color = window.Color;
                this.selectedGroup.InitGroupBrush();
                this.RefreshSelectedGroup();
                this.RefreshGroups();
            }
        }

        /// <summary>
        /// Refreshes the selected group.
        /// </summary>
        private void RefreshSelectedGroup()
        {
            this.selectedCustomer = null;
            this.customerStackPanel.Visibility = System.Windows.Visibility.Collapsed;
            this.stockStackPanel.Visibility = System.Windows.Visibility.Collapsed;
            this.recordsStackPanel.Visibility = System.Windows.Visibility.Collapsed;
            if (this.selectedGroup != null)
            {
                this.mainWindowTitle.Foreground = this.selectedGroup.GroupBrush;
                this.mainWindowTitle.Text = this.selectedGroup.Name;
                this.cardsListBox.ItemsSource = this.selectedGroup.Cards;
                this.cardsListBox.BorderBrush = this.selectedGroup.GroupBrush;
                this.cardListTitle.Background = this.selectedGroup.GroupBrush;
                this.cardStackPanel.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                this.mainWindowTitle.Text = "";
                this.cardsListBox.ItemsSource = null;
                this.cardsListBox.BorderBrush = Brushes.Transparent;
                this.cardListTitle.Background = Brushes.Transparent;
                this.cardStackPanel.Visibility = System.Windows.Visibility.Collapsed;
            }
            this.ActivateMenuButtons();
        }

        /// <summary>
        /// Refreshes the selected card.
        /// </summary>
        private void RefreshSelectedCard()
        {
            if (this.selectedCard != null)
            {
                this.recordsStackPanel.Visibility = System.Windows.Visibility.Visible;
                this.recordsStackSubTitle.Text = String.Format("{0} ({1})", this.selectedCard.Name, this.selectedCard.CodeJK);
                this.recordListTitle.Background = this.selectedGroup.GroupBrush;
                this.recordsListBox.ItemsSource = this.selectedCard.Records;
            }
            else
            {
                this.recordsStackPanel.Visibility = System.Windows.Visibility.Collapsed;
                this.recordsStackSubTitle.Text = "";
                this.recordListTitle.Background = Brushes.Transparent;
                this.recordsListBox.ItemsSource = null;
            }
        }

        /// <summary>
        /// Handles the Click event of the deleteGroupButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void deleteGroupButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Opravdu chcete smazat skupinu?", "Sklad", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                DataManager.Instance.DeleteGroup(this.selectedGroup);
                this.selectedGroup = null;
                this.RefreshSelectedGroup();
                this.RefreshGroups();
            }
        }

        /// <summary>
        /// Handles the Click event of the addCardButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void addCardButton_Click(object sender, RoutedEventArgs e)
        {
            CardWindow window = new CardWindow();
            if (window.OpenWindowInMode(OpenWindowMode.Add) == true)
            {
                this.selectedGroup.AddCard(new Card(window.CardName, window.CodeJK));
                this.RefreshSelectedGroup();
            }
        }

        /// <summary>
        /// Handles the Click event of the editCardButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void editCardButton_Click(object sender, RoutedEventArgs e)
        {
            CardWindow window = new CardWindow();
            window.CardName = this.selectedCard.Name;
            window.CodeJK = this.selectedCard.CodeJK;
            if (window.OpenWindowInMode(OpenWindowMode.Edit) == true)
            {
                this.selectedCard.Name = window.CardName;
                this.selectedCard.CodeJK = window.CodeJK;
                this.RefreshSelectedCard();
            }
        }

        /// <summary>
        /// Handles the Click event of the deleteCardButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void deleteCardButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Opravdu chcete smazat tuto kartu?", "Sklad", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                this.selectedGroup.DeleteCard(this.selectedCard);
                this.selectedCard = null;
                this.RefreshSelectedGroup();
            }
        }

        /// <summary>
        /// Handles the Click event of the addRecordButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void addRecordButton_Click(object sender, RoutedEventArgs e)
        {
            RecordWindow window = new RecordWindow();
            if (window.OpenWindowInMode(OpenWindowMode.Add) == true)
            {
                this.selectedCard.AddRecord(new Record(window.RecordType, window.Date, window.Price, window.Amount, window.Note));
                this.RefreshSelectedCard();
            }
        }

        /// <summary>
        /// Handles the Click event of the removeRecordButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void removeRecordButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Opravdu chcete smazat tento záznam?", "Sklad", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                if (this.recordsListBox.SelectedItem != null)
                {
                    this.selectedCard.DeleteRecord(this.recordsListBox.SelectedItem as Record);
                    this.selectedRecord = null;
                    this.RefreshSelectedCard();
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the backToGroupButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void backToGroupButton_Click(object sender, RoutedEventArgs e)
        {
            this.selectedCard = null;
            this.recordsStackPanel.Visibility = System.Windows.Visibility.Collapsed;
            this.RefreshSelectedGroup();
        }

        /// <summary>
        /// Handles the MouseDoubleClick event of the cardsListBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void cardsListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.selectedCard = (this.cardsListBox.SelectedItem as Card);
            this.cardStackPanel.Visibility = System.Windows.Visibility.Collapsed;
            this.RefreshSelectedCard();
            this.ActivateMenuButtons();
        }

        /// <summary>
        /// Handles the MouseDoubleClick event of the groupsListBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void groupsListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.selectedCustomer = null;
            this.selectedGroup = (this.groupsListBox.SelectedItem as Group);
            this.selectedCard = null;
            this.RefreshSelectedGroup();
        }

        /// <summary>
        /// Handles the MouseDoubleClick event of the recordsListBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void recordsListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.selectedRecord = (this.recordsListBox.SelectedItem as Record);
            RecordWindow window = new RecordWindow();
            window.RecordType = this.selectedRecord.RecordType;
            window.Date = this.selectedRecord.Date;
            window.Price = this.selectedRecord.Price;
            window.Amount = this.selectedRecord.Amount;
            window.Note = this.selectedRecord.Note;
            if (window.OpenWindowInMode(OpenWindowMode.Edit) == true)
            {
                this.selectedRecord.RecordType = window.RecordType;
                this.selectedRecord.Date = window.Date;
                this.selectedRecord.Price = window.Price;
                this.selectedRecord.Amount = window.Amount;
                this.selectedRecord.Note = window.Note;
                this.selectedCard.CountCurrentRecordStock();
                this.RefreshSelectedCard();
            }
        }

        /// <summary>
        /// Handles the Click event of the currentStockButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void currentStockButton_Click(object sender, RoutedEventArgs e)
        {
            this.ShowStockPanel(StockStatusType.Current);
        }

        /// <summary>
        /// Handles the Click event of the afterYearStockButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void afterYearStockButton_Click(object sender, RoutedEventArgs e)
        {
            SelectYearWindow window = new SelectYearWindow();
            if (window.ShowDialog() == true)
            {
                this.selectedStockYear = window.Year;
                this.ShowStockPanel(StockStatusType.AfterYear);
            }
        }

        /// <summary>
        /// Handles the Click event of the forYearStockButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void forYearStockButton_Click(object sender, RoutedEventArgs e)
        {
            SelectYearWindow window = new SelectYearWindow();
            if (window.ShowDialog() == true)
            {
                this.selectedStockYear = window.Year;
                this.ShowStockPanel(StockStatusType.ForYear);
            }
        }

        /// <summary>
        /// Shows the stock panel.
        /// </summary>
        /// <param name="stockStatusType">Type of the stock status.</param>
        private void ShowStockPanel(StockStatusType stockStatusType)
        {
            this.selectedGroup = null;
            this.selectedCard = null;
            this.selectedRecord = null;
            this.selectedCustomer = null;
            this.ActivateMenuButtons();

            this.recordsStackPanel.Visibility = System.Windows.Visibility.Collapsed;
            this.cardStackPanel.Visibility = System.Windows.Visibility.Collapsed;
            this.customerStackPanel.Visibility = System.Windows.Visibility.Collapsed;
            this.stockStackPanel.Visibility = System.Windows.Visibility.Visible;

            int year = stockStatusType == StockStatusType.Current ? -1 : this.selectedStockYear;

            List<GroupStock> stock = DataManager.Instance.GetGroupStock(stockStatusType, year);

            switch (stockStatusType)
            {
                case StockStatusType.Current:
                    this.stockPanelTitle.Text = "Aktuální stav zásob";
                    break;

                case StockStatusType.AfterYear:
                    this.stockPanelTitle.Text = String.Format("Stav zásob po roce {0}", this.selectedStockYear);
                    break;

                case StockStatusType.ForYear:
                    this.stockPanelTitle.Text = String.Format("Stav zásob pro rok {0}", this.selectedStockYear);
                    break;
            }

            this.stockPriceListBox.ItemsSource = stock;
            this.stockCountAndPriceListBox.ItemsSource = stock;
            GroupStock summaryStock = DataManager.Instance.GetSummaryStock(stockStatusType, year);
            this.stockSummaryStackPanel.DataContext = new
            {
                SummaryName = summaryStock.Name,
                SummaryCount = String.Format("{0} ks", summaryStock.Count),
                SummaryPrice = String.Format("{0} Kč", summaryStock.Price)
            };
        }

        /// <summary>
        /// Handles the Click event of the printCardsButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void printCardsButton_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDlg = new PrintDialog();
            if (printDlg.ShowDialog() == true)
            {
                PrintDocument(printDlg);
                MessageBox.Show("Tisk dokončen", "Sklad", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// Handles the Click event of the printStockButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void printStockButton_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDlg = new PrintDialog();
            if (printDlg.ShowDialog() == true)
            {
                printDlg.PrintVisual(this.stockPrintStackPanel, this.stockPanelTitle.Text);
                MessageBox.Show("Tisk dokončen", "Sklad", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// Handles the Click event of the addCustomerButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void addCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            CustomerWindow window = new CustomerWindow();
            if (window.OpenWindowInMode(OpenWindowMode.Add) == true)
            {
                DataManager.Instance.AddCustomer(
                    new Customer(
                        window.CustomerName,
                        window.Phone,
                        window.Address,
                        window.Furnace,
                        window.Date
                    )
                );
                this.RefreshCustomers();
            }
        }

        /// <summary>
        /// Handles the Click event of the editCustomerButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void editCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            CustomerWindow window = new CustomerWindow();
            window.CustomerName = this.selectedCustomer.Name;
            window.Phone = this.selectedCustomer.Phone;
            window.Address = this.selectedCustomer.Address;
            window.Furnace = this.selectedCustomer.Furnace;
            window.Date = this.selectedCustomer.Date;

            if (window.OpenWindowInMode(OpenWindowMode.Edit) == true)
            {
                this.selectedCustomer.Name = window.CustomerName;
                this.selectedCustomer.Phone = window.Phone;
                this.selectedCustomer.Address = window.Address;
                this.selectedCustomer.Furnace = window.Furnace;
                this.selectedCustomer.Date = window.Date;
                this.RefreshCustomers();
                this.RefreshSelectedCustomer();
            }
        }

        /// <summary>
        /// Handles the Click event of the removeCustomerButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void removeCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.selectedCustomer != null)
            {
                if (MessageBox.Show("Skutečně si přejete smazat tohoto zákazníka?", "Sklad", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    DataManager.Instance.DeleteCustomer(this.selectedCustomer);
                    this.selectedCustomer = null;
                    this.RefreshCustomers();
                    this.RefreshSelectedCustomer();
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the addCustomerRecordButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void addCustomerRecordButton_Click(object sender, RoutedEventArgs e)
        {
            CustomerRecordWindow window = new CustomerRecordWindow();
            if (window.OpenWindowInMode(OpenWindowMode.Add) == true)
            {
                this.selectedCustomer.AddCustomerRecord(new CustomerRecord(window.Date, window.Note));
                this.RefreshSelectedCustomer();
            }
        }

        /// <summary>
        /// Handles the Click event of the removeCustomerRecordButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void removeCustomerRecordButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Opravdu chcete smazat tento záznam?", "Sklad", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                if (this.customerRecordsListBox.SelectedItem != null)
                {
                    this.selectedCustomer.DeleteCustomerRecord(this.customerRecordsListBox.SelectedItem as CustomerRecord);
                    this.RefreshSelectedCustomer();
                }
            }
        }

        /// <summary>
        /// Handles the MouseDoubleClick event of the customersListBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void customersListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.selectedGroup = null;
            this.selectedCard = null;
            this.selectedRecord = null;
            this.RefreshSelectedGroup();
            this.selectedCustomer = (this.customersListBox.SelectedItem as Customer);
            this.RefreshSelectedCustomer();
        }

        /// <summary>
        /// Refreshes the selected customer.
        /// </summary>
        private void RefreshSelectedCustomer()
        {
            this.customerInfoGrid.DataContext = null;
            this.customerRecordsListBox.ItemsSource = null;

            if (this.selectedCustomer != null)
            {
                this.customerStackPanel.Visibility = System.Windows.Visibility.Visible;
                this.customerNameTextBox.Text = this.selectedCustomer.Name;
                this.customerInfoGrid.DataContext = this.selectedCustomer;
                this.customerRecordsListBox.ItemsSource = this.selectedCustomer.CustomerRecords;
            }
            else
            {
                this.customerStackPanel.Visibility = System.Windows.Visibility.Collapsed;
            }
            this.ActivateMenuButtons();
        }

        /// <summary>
        /// Handles the MouseDoubleClick event of the customerRecordsListBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void customerRecordsListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CustomerRecord customerRecord = (this.customerRecordsListBox.SelectedItem as CustomerRecord);
            CustomerRecordWindow window = new CustomerRecordWindow();
            window.Date = customerRecord.Date;
            window.Note = customerRecord.Text;
            if (window.OpenWindowInMode(OpenWindowMode.Edit) == true)
            {
                customerRecord.Date = window.Date;
                customerRecord.Text = window.Note;
                this.RefreshSelectedCustomer();
            }
        }

        private void PrintDocument(PrintDialog printDlg)
        {
            PrintContexts printContexts = new PrintContexts(this.selectedGroup);

            FixedDocument fixedDocument = new FixedDocument();

            foreach (PrintContext context in printContexts.Contexts)
            {
                PageContent pageContent = new PageContent();
                FixedPage fixedPage = new FixedPage();

                fixedPage.Children.Add(context.StackPanel);
                (pageContent as System.Windows.Markup.IAddChild).AddChild(fixedPage);
                fixedDocument.Pages.Add(pageContent);
            }

            XpsDocumentWriter writer = PrintQueue.CreateXpsDocumentWriter(printDlg.PrintQueue);

            writer.Write(fixedDocument, printDlg.PrintTicket);
        }

        private class PrintContexts
        {
            private Group _group;

            private List<PrintContext> _contexts;

            public PrintContexts(Group group)
            {
                _group = group;
                _contexts = new List<PrintContext>();
                CreateContexts();
            }

            private void CreateContexts()
            {
                int recordCount = _group.Cards.Count;
                int pageCount = recordCount / RecordsPerPage + ((recordCount % RecordsPerPage == 0) ? 0 : 1);

                for (int i = 0; i < pageCount; ++i)
                {
                    _contexts.Add(new PrintContext(string.Format("{0} ({1}/{2})", _group.Name, i + 1, pageCount), _group.Cards.GetRange(i * RecordsPerPage, Math.Min(RecordsPerPage, recordCount - i*RecordsPerPage))));
                }
            }
            public List<PrintContext> Contexts
            {
                get
                {
                    return _contexts;
                }
            }
        }

        private class PrintContext
        {
            public PrintContext(string label, List<Card> data)
            {
                Label = label;
                Data = data;
                StackPanel = CreatePrintStack();
            }

            private StackPanel CreatePrintStack()
            {
                TextBlock textBlock = new TextBlock()
                {
                    Text = Label,
                    FontSize = 24,
                    FontWeight = FontWeights.Bold,
                    Margin = new Thickness(50, 50, 0, 10),
                    HorizontalAlignment = HorizontalAlignment.Left
                };

                DataGrid grid = new DataGrid()
                {
                    AutoGenerateColumns = false,
                    Margin = new Thickness(50, 10, 0, 10),
                    SelectionUnit = DataGridSelectionUnit.CellOrRowHeader,
                    SelectionMode = DataGridSelectionMode.Single,
                    CanUserResizeColumns = false,
                    CanUserSortColumns = false,
                    CanUserAddRows = false,
                    CanUserDeleteRows = false,
                    CanUserReorderColumns = false,
                    EnableRowVirtualization = false,
                    ItemsSource = Data,
                    IsReadOnly = true,
                    HeadersVisibility = DataGridHeadersVisibility.Column,
                    BorderBrush = Brushes.White,
                    Background = Brushes.White,
                    GridLinesVisibility = DataGridGridLinesVisibility.All
                };

                DataGridTextColumn col = new DataGridTextColumn();
                col.Header = "Název";
                col.Width = 400;
                col.Binding = new Binding("Name");
                grid.Columns.Add(col);

                col = new DataGridTextColumn();
                col.Header = "Kód JK";
                col.Width = 100;
                col.Binding = new Binding("CodeJK");
                grid.Columns.Add(col);

                col = new DataGridTextColumn();
                col.Header = "Zásoba";
                col.Width = 100;
                col.Binding = new Binding("StockCount");
                grid.Columns.Add(col);

                StackPanel stack = new StackPanel();
                stack.Children.Add(textBlock);
                stack.Children.Add(grid);
                return stack;
            }

            public string Label { get; private set; }

            public List<Card> Data { get; private set; }

            public StackPanel StackPanel { get; private set; }
        }
    }
}
