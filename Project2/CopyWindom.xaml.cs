using System;
using System.Collections.Generic;
using System.Data;
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
using BookLib.Models.ToUx;

namespace Project2
{
    /// <summary>
    /// Interaction logic for CopyWindom.xaml
    /// </summary>
    public partial class CopyWindom : Window
    {
        private MainWindow mainWindow;
        bool _isCopysBook = false;

        public CopyWindom()
        {
            InitializeComponent();
        }

        public CopyWindom(MainWindow mainWindow): this()
        {
            this.mainWindow = mainWindow;
        }

        internal void SetCopysBook()
        {
            _isCopysBook = true;
        }

        internal void SetCopysJornal()
        {
            dpEdtion.IsEnabled = false;
            tbPublisher.Text = "Null Is Jornal";
            tbPublisher.IsEnabled = false;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                mainWindow.currentsCopys.Add(getThisCopy());
            }
            catch (Exception)
            {
                MessageBox.Show("Not Valid Copy");
            }

            mainWindow.dgNewBookCopysTable.ItemsSource = mainWindow.currentsCopys;
            mainWindow.Show();
            this.Close();
        }

        private EnItem.EnCopy getThisCopy()
        {
            return new EnItem.EnCopy() { Edition = dpEdtion.SelectedDate, Plubisher = _isCopysBook ? tbPublisher.Text : null };
        }
    }
}
