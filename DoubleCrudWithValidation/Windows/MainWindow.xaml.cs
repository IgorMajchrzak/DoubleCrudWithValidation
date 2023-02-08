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

using DoubleCrudWithValidation.Models;
using DoubleCrudWithValidation.Interfaces;
using DoubleCrudWithValidation.Cruds;
using DoubleCrudWithValidation.StringBuilders;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;

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
                var objectSerializer = new ObjectSerializer(type => ObjectSerializer.DefaultAllowedTypes(type) || type.FullName.StartsWith("DoubleCrudWithValidation"));
                BsonSerializer.RegisterSerializer(objectSerializer);
                InitializeComponent();
                (_dbType, _currentCrud) = CrudSelection();
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
                ConnectionWindow content = connectionWindow;
                ConnectionInfo result = content.Result;
                IDbCrud dbCrud;
                if (result.DbType == "MySQL")
                {
                    TableDisplay1.Visibility = Visibility.Visible;
                    TableDisplay2.Visibility = Visibility.Hidden;
                    Tbx5Lbl.Visibility = Visibility.Visible;
                    Tbx5.Visibility = Visibility.Visible;

                    Tbx2Lbl.Content = "Name";
                    Tbx3Lbl.Content = "Description";
                    Tbx4Lbl.Content = "Price";
                    Tbx5Lbl.Content = "Number in stock";

                    string connectionString = new MysqlConnectionStringBuilder(result.Uri, result.Port, result.Username, result.Password).ConnectionString();
                    dbCrud = new MysqlCrud(connectionString);
                }
                else
                {
                    TableDisplay2.Visibility = Visibility.Visible;
                    TableDisplay1.Visibility = Visibility.Hidden;
                    Tbx5Lbl.Visibility = Visibility.Hidden;
                    Tbx5.Visibility = Visibility.Hidden;

                    Tbx2Lbl.Content = "First name";
                    Tbx3Lbl.Content = "Last name";
                    Tbx4Lbl.Content = "Age";
                    Tbx5Lbl.Content = "";

                    string connectionString = new MongodbConnectionStringBuilder(result.Uri, result.Port, result.Username, result.Password).ConnectionString();
                    dbCrud = new MongodbCrud(connectionString);
                }
                return (result.DbType, dbCrud);
            }
        }

        private void Disconnect()
        {
            _dbType = "";
            _currentCrud = null;
            ClearLists();
            ClearTextBoxes();
            (_dbType, _currentCrud) = CrudSelection();
        }

        private static void CrudExceptionDisplay(Exception e)
        {
            string message = e.Message;
            if (e.InnerException != null)
            {
                message += "\n" + e.InnerException.Message;
            }
            MessageBox.Show(message, "Operation failed", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private IValidatable ItemFromTextBoxes()
        {
            IValidatable item;
            if (_dbType == "MySQL")
            {
                int id = int.TryParse(Tbx1.Text, out id) ? id : -1;
                decimal price = decimal.TryParse(Tbx4.Text, out price) ? price : 0;
                int stock = int.TryParse(Tbx5.Text, out stock) ? stock : -1;
                item = new Product(id, Tbx2.Text, Tbx3.Text, price, stock);
            }
            else
            {
                int age = int.TryParse(Tbx4.Text, out age) ? age : 0;
                string? id = null;
                if (Tbx1.Text != "")
                {
                    id = Tbx1.Text;
                }
                item = new Person(id, Tbx2.Text, Tbx3.Text, age);
            }
            return item;
        }

        private void ClearTextBoxes()
        {
            Tbx1.Text = "";
            Tbx2.Text = "";
            Tbx3.Text = "";
            Tbx4.Text = "";
            Tbx5.Text = "";
        }

        private void ClearLists()
        {
            _products.Clear();
            _people.Clear();
            TableDisplay1.ItemsSource = null;
            TableDisplay1.SelectedItem = null;
            TableDisplay2.ItemsSource = null;
            TableDisplay2.SelectedItem = null;
        }

        private void RefreshList(string idString)
        {
            if (_currentCrud == null)
            {
                ClearTextBoxes();
                ClearLists();
            }
            else if (_dbType == "MySQL")
            {
                try
                {
                    _products = _currentCrud.Read<Product>(idString);
                }
                catch (Exception ex)
                {
                    CrudExceptionDisplay(ex);
                    return;
                }
                TableDisplay1.ItemsSource = _products;
                TableDisplay1.SelectedItem = null;
            }
            else
            {
                _people = _currentCrud.Read<Person>(idString);
                TableDisplay2.ItemsSource = _people;
                TableDisplay2.SelectedItem = null;
            }
        }

        private void ReadBtn_Click(object sender, RoutedEventArgs e)
        {
            RefreshList(Tbx1.Text);
        }

        private void CreateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_currentCrud == null) { return; }
            IValidatable item = ItemFromTextBoxes();
            if (item.Error == String.Empty)
            {
                try
                {
                    _currentCrud.Create(item);
                }
                catch (Exception ex)
                {
                    CrudExceptionDisplay(ex);
                    return;
                }
                RefreshList("");
            }
            else
            {
                MessageBox.Show("Inputted item has following issues: \n" + item.Error, "Item validation error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_currentCrud == null) { return; }
            IValidatable item = ItemFromTextBoxes();
            if (item.Error == String.Empty)
            {
                try
                {
                    _currentCrud.Update(item);
                }
                catch (Exception ex)
                {
                    CrudExceptionDisplay(ex);
                    return;
                }
                RefreshList("");
            }
            else
            {
                MessageBox.Show("Inputted item has following issues: \n" + item.Error, "Item validation error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_currentCrud == null) { return; }
            if (Tbx1.Text != "")
            {
                try
                {
                    _currentCrud.Delete(Tbx1.Text);
                }
                catch (Exception ex)
                {
                    CrudExceptionDisplay(ex);
                    return;
                }
                RefreshList("");
            }
        }

        private void TableDisplay1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TableDisplay1.SelectedItem == null)
            {
                ClearTextBoxes();
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
                ClearTextBoxes();
                return;
            }
            var item = (dynamic)TableDisplay2.SelectedItem;

            Tbx1.Text = item.Id;
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
