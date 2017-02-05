using BookLib.Contracts;
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

namespace Project2
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void bntSubimit_Click(object sender, RoutedEventArgs e)
        {
            IBoolResult result = MainWindow.CurrentManeger.EnterUser(
                           txbUserName.Text, pabUserPassword.Password);

            if (result.Result)
            {
                MessageBox.Show(result.Message);
                MainWindow app = new MainWindow();
                app.Show();
                this.Close();
            }

            else
            {
                MessageBox.Show(result.Message);
                txbUserName.Text = string.Empty;
                pabUserPassword.Password = string.Empty;
            }
        }
    }
}
