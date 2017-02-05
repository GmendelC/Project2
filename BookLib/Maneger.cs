using BookLib.Contracts;
using BookLib.Models;
using BookLib.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLib
{
    public class Maneger : ILoggerList<AbstractItem>
    {
        ItemColection _mylibery;
        public int BookCount { get { return _mylibery.BookCount; } }
        public int JornalCount { get { return _mylibery.JornalCount; } }
        public int BookCopysCount { get { return _mylibery.BookCopysCount; } }
        public int JornalCopysCount { get { return _mylibery.BookCopysCount; } }
        public int Count { get { return _mylibery.BookCopysCount; } }
        public int CopysCount { get { return _mylibery.BookCopysCount; } }

        List<User> _users;
        public int _currentUserInd = -1;

        public string CurrentUserName { get { return getUserName(_currentUserInd); } }

        public User CurrentUser { get { return getUser(_currentUserInd); } }

        public LinkedList<List<AbstractItem>> LogList{ get ; set ;}

        private string getUserName(int Ind)
        {
            // retun user name or null if wasn"t
            User current = getUser(Ind);
            if (current != null)
                return current.Name;

            return null;
        }

        private User getUser(int ind)
        {
            // retun user or null if wasn"t
            User res = null;

            if (ind > 0 && ind < _users.Count)
                res = _users[ind];

            return res;
        }

        public Maneger(ItemColection libery, List<User> users)
        {
            _mylibery = libery;
            _users = users;
            LogList = new LinkedList<List<AbstractItem>>();
        }

        public static Maneger CreateFromData()
        {
            ItemColection items = new ItemColection();
            List<User> users = new List<User>();
            List<string> strUsers = GetUsers();

            foreach (var item in strUsers)
            {
                User newUser = User.StaticSerializ(item);
                users.Add(newUser);
            }

            List<string> strBooks = GetBooks();

            foreach (var item in strBooks)
            {
                Book newBook = Book.StaticSerializ(item);
                items.Add(newBook);
            }

            List<string> strJornals = GetJornals();

            foreach (var item in strBooks)
            {
                Jornal newBook = Jornal.StaticSerializ(item);
                items.Add(newBook);
            }
            Maneger newManeger = new Maneger(items, users);

            return  newManeger;
        }
         public bool AddUser(User newUser)
        {
            // return if is new user

            if (_users == null || ItemManegerLisence())
            {
                if (_users == null)// can't use null list 
                    _users = new List<User>();

                if(!_users.Contains(newUser))
                {
                    _users.Add(newUser);
                    return true;
                }
            }
            return false;
        }

        public bool UpdateUser(User updatedUser)
        {
            if (_users != null && MasterLisence() && _users.Contains(updatedUser))
            {
                int ind = _users.IndexOf(updatedUser);
                _users[ind] = updatedUser;
                return true;
            }

            return false;
        }

        public bool RemoveUser(User oldUser)
        {
            if (_users != null && MasterLisence() && _users.Remove(oldUser))
                return true;

            return false;
        }

        private bool MasterLisence()
        {
            return CurrentUser != null && CurrentUser.License == eLicense.Master;
        }

        private bool ItemManegerLisence()
        {
            return CurrentUser != null && CurrentUser.License != eLicense.Visitor
                            && CurrentUser.License != eLicense.Enployee;
        }

        private bool ItemRequestLisence()
        {
            return CurrentUser != null && CurrentUser.License != eLicense.Visitor;
        }

        public bool AddItem(AbstractItem newItem )
        {
            // only for lisence user

            if (ItemManegerLisence())
                return _mylibery.Add(newItem);

            return false;
        }

        

        public bool RemoveItem(AbstractItem oldItem)
        {
            // only for lisence user
            if (ItemManegerLisence())
                return _mylibery.Remove(oldItem);

            return false;
        }

        public AbstractItem GetItemByISBN(Guid isbn)
        {
            return _mylibery.GetByISBN(isbn);
        }

        public bool RequestItem(Guid item, Guid copy, int requestId)
        {
            if(ItemRequestLisence())
            return _mylibery[item] != null & _mylibery[item][copy] != null && _mylibery[item][copy].Request(requestId);

            return false;
        }

        public bool ReturnItem(Guid item, Guid copy, int requestId)
        {
            if (ItemRequestLisence())
                return _mylibery[item] != null & _mylibery[item][copy] != null && _mylibery[item][copy].Return();

            return false;
        }

        
        public List<AbstractItem> GetItemsAll()
        {
            return _mylibery.GetAll();
        }

        public List<AbstractItem> GetByCategory(eCategory category, eSubcategory subCategory)
        {
            return _mylibery.GetByCategory(category, subCategory);
        }

        public List<AbstractItem> GetByCategory(eCategory category)
        {
            return _mylibery.GetByCategory(category);
        }

        public List<AbstractItem> GetByCategory( eSubcategory subCategory)
        {
            return _mylibery.GetByCategory(subCategory);
        }

        public List<AbstractItem> GetByName(string name)
        {
            return _mylibery.GetByName(name);
        }

        public List<AbstractItem> GetByLikeName(string name)
        {
            return _mylibery.GetByLikeName(name);
        }

        public List<AbstractItem> GetByAutor(string autor)
        {
            return _mylibery.GetByAutor(autor);
        }

        public List<AbstractItem> GetByLikeAutor(string autor)
        {
            return _mylibery.GetByLikeAutor(autor);
        }

        public List<AbstractItem> GetByType(Type type)
        {
            return _mylibery.GetByType(type);
        }

        public List<AbstractItem> GetItems(eSubcategory subCategory, eCategory? categrory = null,
            Type type= null, string likeName = null, string likeAutor = null)
        {
            // return ainterset of not nul parameters
            // no parameters return null.

            List<AbstractItem> res = null;

            if (type != null)
            {
                res = _mylibery.GetByType(type);
            }
      
            if ( subCategory != eSubcategory.None)
            {
                if (res == null)
                    res = _mylibery.GetByCategory(subCategory);
                else
                    res = res.Union<AbstractItem>
                        (_mylibery.GetByCategory(subCategory)).ToList<AbstractItem>();
            }

            if (categrory != null)
            {
                if (res == null)
                    res = _mylibery.GetByCategory(categrory.Value);
                else
                    res = res.Union<AbstractItem>
                        (_mylibery.GetByCategory(categrory.Value)).ToList<AbstractItem>();
            }

            if (categrory != null)
            {
                if (res == null)
                    res = _mylibery.GetByCategory(categrory.Value);
                else
                    res = res.Union<AbstractItem>
                        (_mylibery.GetByCategory(categrory.Value)).ToList<AbstractItem>();
            }

            if (likeName != null)
            {
                if (res == null)
                    res = _mylibery.GetByLikeName(likeName);
                else
                    res = res.Union<AbstractItem>
                        (_mylibery.GetByLikeName(likeName)).ToList<AbstractItem>();
            }

            if (likeAutor != null)
            {
                if (res == null)
                    res = _mylibery.GetByLikeAutor(likeAutor);
                else
                    res = res.Union<AbstractItem>
                        (_mylibery.GetByLikeAutor(likeAutor)).ToList<AbstractItem>();
            }
            return res;
        }

        public List<AbstractItem> GetAllItems(eSubcategory subCategory, eCategory? categrory = null,
    Type type = null, string likeName = null, string likeAutor = null)
        {
            // return ainterset of not nul parameters
            // no parameters return null.

            List<AbstractItem> res = null;

            if (type != null)
            {
                res = _mylibery.GetByType(type);
            }

            if (subCategory != eSubcategory.None)
            {
                if (res == null)
                    res = _mylibery.GetByCategory(subCategory);
                else
                    res = res.Intersect<AbstractItem>
                        (_mylibery.GetByCategory(subCategory)).ToList<AbstractItem>();
            }

            if (categrory != null)
            {
                if (res == null)
                    res = _mylibery.GetByCategory(categrory.Value);
                else
                    res = res.Intersect<AbstractItem>
                        (_mylibery.GetByCategory(categrory.Value)).ToList<AbstractItem>();
            }

            if (categrory != null)
            {
                if (res == null)
                    res = _mylibery.GetByCategory(categrory.Value);
                else
                    res = res.Intersect<AbstractItem>
                        (_mylibery.GetByCategory(categrory.Value)).ToList<AbstractItem>();
            }

            if (likeName != null)
            {
                if (res == null)
                    res = _mylibery.GetByLikeName(likeName);
                else
                    res = res.Intersect<AbstractItem>
                        (_mylibery.GetByLikeName(likeName)).ToList<AbstractItem>();
            }

            if (likeAutor != null)
            {
                if (res == null)
                    res = _mylibery.GetByLikeAutor(likeAutor);
                else
                    res = res.Intersect<AbstractItem>
                        (_mylibery.GetByLikeAutor(likeAutor)).ToList<AbstractItem>();
            }
            return res;
        }

        public void Add(List<AbstractItem> list)
        {
            throw new NotImplementedException();
        }
    }
}
