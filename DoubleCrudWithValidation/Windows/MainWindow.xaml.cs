using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using MySql.Data.MySqlClient;
using MongoDB.Driver;
using DoubleCrudWithValidation.Structs;
using DoubleCrudWithValidation.Interfaces;
using DoubleCrudWithValidation.Cruds;

namespace DoubleCrudWithValidation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Product> _products = new List<Product>();
        private List<Person> _people = new List<Person>();
        internal IDbCrud? _currentCrud;
        private string _dbType = "";
        public MainWindow()
        {
            {
                //items = new List<Product>();
                InitializeComponent();
                (_dbType, _currentCrud) = CrudSelection();
                TableDisplay1.Items.Clear();
                TableDisplay2.Items.Clear();
            }
        }

        private (string, IDbCrud?) CrudSelection()
        {
            ConnectionWindow connectionWindow = new ConnectionWindow();
            bool? connect = connectionWindow.ShowDialog();
            if (connect == null || connect == false)
            {
                Application.Current.Shutdown();
                return ("", null);
            }
            else
            {
                ConnectionWindow content = connectionWindow as ConnectionWindow;
                ConnectionInfo result = content.Result;
                if (result.DbType == "MySQL")
                {
                    TableDisplay1.Visibility = Visibility.Visible;
                    TableDisplay2.Visibility = Visibility.Hidden;
                    Tbx5.Visibility = Visibility.Visible;

                    Tbx2Lbl.Content = "Name";
                    Tbx3Lbl.Content = "Description";
                    Tbx4Lbl.Content = "Price";
                    Tbx5Lbl.Content = "Number in stock";

                    return (result.DbType, new MysqlCrud($"datasource={result.Uri};port={result.Port};username={result.Username};password={result.Password}"));
                }
                else
                {
                    TableDisplay2.Visibility = Visibility.Visible;
                    TableDisplay1.Visibility = Visibility.Hidden;
                    Tbx5.Visibility = Visibility.Hidden;

                    Tbx2Lbl.Content = "First name";
                    Tbx3Lbl.Content = "Last name";
                    Tbx4Lbl.Content = "Age";
                    Tbx5Lbl.Content = "";

                    return (result.DbType, new MongodbCrud($"mongodb+srv://{result.Username}:{result.Password}@{result.Uri}:{result.Port}"));
                }
            }
        }

        private void Disconnect()
        {
            _currentCrud = null;
            (_dbType, _currentCrud) = CrudSelection();
        }

        private dynamic ItemFromTextBoxes()
        {
            dynamic item;
            if (_dbType == "MySQL")
            {
                int id = int.TryParse(Tbx1.Text, out id) ? id : -1;
                double price = double.TryParse(Tbx4.Text, out price) ? price : 0;
                int stock = int.TryParse(Tbx5.Text, out stock) ? stock : -1;
                item = new Product { ProductId = id, Name = Tbx2.Text, Description = Tbx3.Text, Price = price, NumberInStock = stock };
            }
            else
            {
                item = new Person { };
            }
            return item;
        }

        private void RefreshList(int id)
        {
            if (_currentCrud == null) { return; }

            if (_dbType == "MySQL")
            {
                _products = _currentCrud.Read<Product>(id);
                TableDisplay1.ItemsSource = _products;
                TableDisplay1.SelectedItem = null;
            }
            else
            {
                _people = _currentCrud.Read<Person>(id);
                TableDisplay2.ItemsSource = _people;
                TableDisplay2.SelectedItem = null;
            }
        }

        private void ReadBtn_Click(object sender, RoutedEventArgs e)
        {
            int id = int.TryParse(Tbx1.Text, out id) ? id : -1;
            RefreshList(id);
        }

        private void CreateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_currentCrud == null) { return; }
            dynamic item = ItemFromTextBoxes();

            _currentCrud.Create(item);
            RefreshList(-1);
        }

        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_currentCrud == null) { return; }
            dynamic item = ItemFromTextBoxes();

            _currentCrud.Update(item);
            RefreshList(-1);
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_currentCrud == null) { return; }
            int id = int.TryParse(Tbx1.Text, out id) ? id : -1;
            if (id > 0)
            {
                _currentCrud.Delete(id);
                RefreshList(-1);
            }
        }

        private void TableDisplay1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TableDisplay1.SelectedItem == null)
            {
                Tbx1.Text = "";
                Tbx2.Text = "";
                Tbx3.Text = "";
                Tbx4.Text = "";
                Tbx5.Text = "";
                return;
            }
            var item = (dynamic)TableDisplay1.SelectedItem;

            Tbx1.Text = item.ProductId.ToString();
            Tbx2.Text = item.Name;
            Tbx3.Text = item.Description;
            Tbx4.Text = item.Price.ToString();
            Tbx5.Text = item.NumberInStock.ToString();
        }

        private void TableDisplay2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TableDisplay2.SelectedItem == null)
            {
                Tbx1.Text = "";
                Tbx2.Text = "";
                Tbx3.Text = "";
                Tbx4.Text = "";
                return;
            }
            var item = (dynamic)TableDisplay2.SelectedItem;

            Tbx1.Text = item.PersonId.ToString();
            Tbx2.Text = item.FirstName;
            Tbx3.Text = item.LastName;
            Tbx4.Text = item.Age.ToString();
        }

        private void DisconnectBtn_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                Disconnect();
            }
        }
    }
}
