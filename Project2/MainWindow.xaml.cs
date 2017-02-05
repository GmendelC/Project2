using BookLib;
using BookLib.Contracts;
using BookLib.Models;
using BookLib.Models.ToUx;
using Project2.Model;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Project2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<EnItem.EnCopy> currentsCopys { get; set; } = new List<EnItem.EnCopy>();
        static public BllManeger CurrentManeger { get; set; }
        public MainWindow()
        {
            //CurrentManeger = BllManeger.GetBLData();
            InitializeComponent();


            string[] listEnum = Enum.GetNames(typeof(eLicense));
            cbNewUserLicense.Items.Add(listEnum);
            listEnum = Enum.GetNames(typeof(eCategory));
            cbNewBookCategory.Items.Add(listEnum);
            listEnum = Enum.GetNames(typeof(eSubcategory));
            cbNewBookSubcategory.Items.Add(listEnum);
        }

        private void tabAllItems_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            dgAllItems.ItemsSource = CurrentManeger.GetItemsAll();
        }

        #region AddBook
        private void ListBoxItemNewJornal_Selected(object sender, RoutedEventArgs e)
        {
            tbNewBookPublisher.IsEnabled = true;
            tbNewBookPublisher.Text = "";
        }

        private void ListBoxItemNewBook_Selected(object sender, RoutedEventArgs e)
        {
            if (tbNewBookPublisher != null)
            {
                tbNewBookPublisher.IsEnabled = false;
                tbNewBookPublisher.Text = "Only In jornal";
            }
        }

        private void dgNewBookCopysTable_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            string text = ((ListBoxItem)lbNewBookType.SelectedItem).Content.ToString();
            CopyWindom newCopyWindow = new CopyWindom(this);

            if (text == "Book")
                newCopyWindow.SetCopysBook();
            else
                newCopyWindow.SetCopysJornal();

            this.Hide();
            newCopyWindow.Show();
        }
        private void bntAddBook_Click(object sender, RoutedEventArgs e)
        {
            Guid? newIsbn;
            Guid tmp;
            if (Guid.TryParse(tbNewBookISBN.Text, out tmp) & !string.IsNullOrWhiteSpace(tbNewBookISBN.Text))
                newIsbn = tmp;
            else
                newIsbn = null;

            try
            {
                EnItem newItem = new EnItem()
                {
                    ISBN = newIsbn,
                    Autors = tbNewBookAuthors.Text.Split(','),
                    Name = tbNewBookName.Text,
                    FirstEdition = dbNewBookDate.SelectedDate.Value,
                    CatgoryItem = (eCategory)Enum.Parse(typeof(eCategory), cbNewBookCategory.Text),
                    SubcategoryItem = (eSubcategory)Enum.Parse(typeof(eSubcategory), cbNewBookSubcategory.Text),
                    Plubisher = tbNewBookPublisher.Text,
                    Copys = currentsCopys
                };

                if (!CurrentManeger.AddItem(newItem))
                    MessageBox.Show("Your add fail, you can to add this book");// have to i bool result string
            }
            catch (Exception)
            {
                MessageBox.Show("Your add fail, not all mandatory");

            }
        }
        private void TabItem_ContextMenuClosing(object sender, ContextMenuEventArgs e)
        {
            tbNewBookISBN.Text = "";
            tbNewBookName.Text = "";
            tbNewBookAuthors.Text = "";
            cbNewBookCategory.SelectedIndex = -1;
            cbNewBookSubcategory.SelectedIndex = -1;
            ListBoxItemNewBook_Selected(null, null);
            dgNewBookCopysTable.ItemsSource = null;
        } 
        #endregion

        private void bntSearch_Click(object sender, RoutedEventArgs e)
        {
            string str = tbSearch.Text;
            List<EnItem> searchResult = new List<EnItem>();
            switch (cbSearch.SelectedIndex)
            {
                case 0:
                    searchResult = CurrentManeger.GetByLikeName(str);
                    break;
                case 1:
                    eCategory c;
                    if(Enum.TryParse<eCategory>(str,out c))
                    searchResult = CurrentManeger.GetByCategory(c);
                    break;
                case 2:
                    eSubcategory sc;
                    if(Enum.TryParse<eSubcategory>(str, out sc))
                    searchResult = CurrentManeger.GetByCategory(sc);
                    break;
                case 3:
                    searchResult = CurrentManeger.GetByLikeAutor(str);
                    break;
                case 4:
                    searchResult = CurrentManeger.GetByType(Type.GetType(str));
                    break;
                default:

                    break;
            }
            dgSearchResult.ItemsSource = searchResult;
        }

        private void btnAddUser_Click(object sender, RoutedEventArgs e)
        {
            bool valid = false;
            int id;
            if (int.TryParse(tbNewUserId.Text, out id))
            {
                eLicense newLicense;
                if (Enum.TryParse<eLicense>(cbNewUserLicense.Text, out newLicense))
                {
                    IUser newUser = new User()
                        { Id = id, Name = tbNewUserName.Text, License = newLicense,
                        Pasword = tbNewUserPassoword.Text };

                    if (CurrentManeger.AddUser(newUser))
                        valid = true;
                }
            }

            if (valid)
                MessageBox.Show("Sucess");
            else
                MessageBox.Show("Fail");
        }

        private void bntRequest_Click(object sender, RoutedEventArgs e)
        {
            bool valid = false;
            Guid bookISBN;
            if(Guid.TryParse(tbBookRequestId.Text, out bookISBN))
            {
                Guid copyId;
                if (Guid.TryParse(tbCopyRequestId.Text, out copyId))
                {
                    int costomerId;
                    if (int.TryParse( tbCostumerRequestId.Text, out costomerId))
                    {
                        switch(lbTypeRequest.SelectedIndex)
                        {
                            case 0:
                                if (CurrentManeger.RequestItem(bookISBN, copyId, costomerId))
                                    valid = true;
                                break;
                            case 1:
                                if (CurrentManeger.ReturnItem(bookISBN, copyId, costomerId))
                                    valid = true;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }

            if (valid)
                MessageBox.Show("Sucess");
            else
                MessageBox.Show("Fail");
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            CurrentManeger.SaveToData(CurrentManeger);
        }

        private void TabItem_ContextMenuClosing_1(object sender, ContextMenuEventArgs e)
        {
            tbNewUserId.Text = "";
            tbNewUserName.Text = "";
            tbNewUserPassoword.Text = "";
            cbNewUserLicense.SelectedIndex = 0;
        }

        private void TabItem_ContextMenuClosing_2(object sender, ContextMenuEventArgs e)
        {
            tbBookRequestId.Text = "";
            tbCopyRequestId.Text = "";
            tbCostumerRequestId.Text = "";
            lbTypeRequest.SelectedIndex = 0;
        }

        private void TabItem_ContextMenuClosing_3(object sender, ContextMenuEventArgs e)
        {
            tbSearch.Text = "";
            cbSearch.SelectedIndex = 0;
        }
    }
}
