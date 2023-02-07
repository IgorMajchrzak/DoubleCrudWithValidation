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
using System.Windows.Shapes;
using System.Threading;

using DoubleCrudWithValidation.Models;

namespace DoubleCrudWithValidation
{
    /// <summary>
    /// Popup window for connecting to a chosen database
    /// </summary>
    public partial class ConnectionWindow : Window
    {
        public ConnectionInfo Result
        {
            get {
                string dbType = "MongoDB";
                if (MySqlRbtn.IsChecked == true) { dbType = "MySQL"; }
                return new ConnectionInfo { DbType = dbType, Uri = AddressTbx.Text, Port = PortTbx.Text, Username = UsernameTbx.Text, Password = PassPbx.Password };
                }
        }

        public ConnectionWindow()
        {
            InitializeComponent();
        }

        private void ConnectBtn_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).DialogResult = true;
            Window.GetWindow(this).Close();
        }
    }
}
