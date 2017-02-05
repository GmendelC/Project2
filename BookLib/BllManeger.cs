using BookLib.Contracts;
using BookLib.Models;
using BookLib.Models.ToUx;
using BookLib.Models.User;
using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BookLib
{
    [Serializable]
    public class BllManeger: ISerializable
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

        internal User CurrentUser { get { return (User)getUser(_currentUserInd); } }

        public LinkedList<List<AbstractItem>> LogList{ get ; set ;}

        private string getUserName(int Ind)
        {
            // retun user name or null if wasn"t
            IUser current = getUser(Ind);
            return (current != null) ? current.Name : null;
        }

        private IUser getUser(int ind)
        {
            // retun user or null if wasn"t
            IUser current = null;

            if (ind > 0 && ind < _users.Count)
                current = _users[ind];

            return current;
        }

        public BllManeger(ItemColection libery, List<User> users)
        {
            _mylibery = libery;
            _users = users;
            LogList = new LinkedList<List<AbstractItem>>();
            _users = new List<User>();
        }
        protected BllManeger(SerializationInfo info, StreamingContext context)
        {
            _mylibery = (ItemColection)info.GetValue("_mylibery", typeof(ItemColection));
            _users = (List<User>)info.GetValue("_users", typeof(List<User>));
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("_mylibery", _mylibery, typeof(ItemColection));
            info.AddValue("_users", _users, typeof(List<User>));
        }


        static public BllManeger Deserialize(SerializationInfo info, StreamingContext context)
        {
            return new BllManeger(info, context);
        }
        static public BllManeger GetBLData()
        {
            var data = DBData.DeSerialize<BllManeger>();
            return data;
        }

        public void SaveToData(BllManeger data)
        {
            DBData.Serialize(data);
        }

        #region UserLogic

        private User GetUserByName(string userName)
        {
            return _users.Find(u => u.Name == userName);
        }

        private bool EmptyUsers() { return _users.Count < 1; }
        public IBoolResult EnterUser(string userName, string password)
        {
            User loginUser = GetUserByName(userName);

            if (loginUser != null && loginUser.Pasword == password)
                return new BoolResult()
                {
                    Result = true,
                    Message = $"Hello {loginUser.Name},\n your licence is {loginUser.License.ToString()}. "
                };
            else
            {
                if (EmptyUsers())
                    return new BoolResult() { Result = true, Message = "Hello, isn't Users please add master user. " };
                else
                    return new BoolResult() { Result = false, Message = "Invalid User or Password" };
            }
        }
        public bool AddUser(IUser newUser)
        {
            User temp = GetUser(newUser);

            return AddUser(temp);
        }
        private bool AddUser(User newUser)
        {
            // return if is new user

            if (EmptyUsers()|| ItemManegerLisence())
            {
                if (_users == null)// can't use null list 
                    _users = new List<User>();

                if (!_users.Contains(newUser))
                {
                    _users.Add(newUser);
                    return true;
                }
            }
            return false;
        }

        public bool UpdateUser(IUser newUser)
        {
            User temp = GetUser(newUser);

            return UpdateUser(temp);
        }

        

        private bool UpdateUser(User updatedUser)
        {
            if (_users != null && MasterLisence() && _users.Contains(updatedUser))
            {
                int ind = _users.IndexOf(updatedUser);
                _users[ind] = updatedUser;
                return true;
            }
            else
            return false;
        }

        public bool RemoveUser(IUser oldUser)
        {
            User temp = GetUser(oldUser);
            return RemoveUser(temp);
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
        private static User GetUser(IUser newUser)
        {
            User temp = null;

            if (newUser != null)
                temp = new User(newUser.Id, newUser.Name, newUser.Pasword, newUser.License);
            return temp;
        }
        #endregion

        #region ItemLogic
        public bool AddItem(EnItem newItem)
        {
            AbstractItem temp = GetAbstractItem(newItem);
            return AddItem(temp);
        }
        private bool AddItem(AbstractItem newItem)
        {
            // only for lisence user

            if (ItemManegerLisence())
                return _mylibery.Add(newItem);
            else
            return false;
        }

        public bool RemoveItem(EnItem newItem)
        {
            AbstractItem temp = GetAbstractItem(newItem);
            return RemoveItem(temp);
        }

        private static AbstractItem GetAbstractItem(EnItem newItem)
        {
            AbstractItem temp = null;
            if (newItem.Plubisher == null)
            {
                if (newItem.ISBN == null)
                    temp = new Book(newItem.Name, newItem.Autors,
                                    newItem.FirstEdition, newItem.CatgoryItem,
                                    newItem.SubcategoryItem,
                                    EnItem.EnCopy.ToBll(newItem.Copys));
                else
                    temp = new Book(newItem.ISBN.Value, newItem.Name, newItem.Autors,
                                   newItem.FirstEdition, newItem.CatgoryItem,
                                   newItem.SubcategoryItem,
                                   EnItem.EnCopy.ToBll(newItem.Copys));
            }
            else
            {
                if (newItem.Plubisher == null)
                    temp = new Jornal(newItem.Name, newItem.Autors,
                                    newItem.FirstEdition, newItem.CatgoryItem,
                                    newItem.SubcategoryItem,
                                    newItem.Plubisher,
                                    EnItem.EnCopy.ToBll(newItem.Copys));
                else
                    temp = new Jornal(newItem.ISBN.Value, newItem.Name, newItem.Autors,
                                    newItem.FirstEdition, newItem.CatgoryItem,
                                    newItem.SubcategoryItem,
                                    newItem.Plubisher,
                                    EnItem.EnCopy.ToBll(newItem.Copys));
            }

            return temp;
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
            if (ItemRequestLisence())
                return _mylibery[item] != null & _mylibery[item][copy] != null && _mylibery[item][copy].Request(requestId);

            return false;
        }

        public bool ReturnItem(Guid item, Guid copy, int requestId)
        {
            if (ItemRequestLisence())
                return _mylibery[item] != null & _mylibery[item][copy] != null && _mylibery[item][copy].Return();

            return false;
        } 
        #endregion

        #region Search
        public List<EnItem> GetItemsAll()
        {
            List<AbstractItem> tmpList = _mylibery.GetAll();
            return ToEnItemList(tmpList);
        }

        private static List<EnItem> ToEnItemList(List<AbstractItem> tmpList)
        {
            List<EnItem> resList = new List<EnItem>();

            foreach (var item in tmpList)
            {
                resList.Add(new EnItem(item));
            }

            return resList;
        }

        public List<EnItem> GetByCategory(eCategory category, eSubcategory subCategory)
        {
            List<AbstractItem> tmpList = _mylibery.GetByCategory(category, subCategory);
            return ToEnItemList(tmpList);
        }

        public List<EnItem> GetByCategory(eCategory category)
        {
            List<AbstractItem> tmpList = _mylibery.GetByCategory(category);
            return ToEnItemList(tmpList);
        }

        public List<EnItem> GetByCategory(eSubcategory subCategory)
        {
            List<AbstractItem> tmpList = _mylibery.GetByCategory(subCategory);
            return ToEnItemList(tmpList);
        }

        public List<EnItem> GetByName(string name)
        {
            List<AbstractItem> tmpList = _mylibery.GetByName(name);
            return ToEnItemList(tmpList);
        }

        public List<EnItem> GetByLikeName(string name)
        {
            List<AbstractItem> tmpList = _mylibery.GetByLikeName(name);
            return ToEnItemList(tmpList);
        }

        public List<EnItem> GetByAutor(string autor)
        {
            List<AbstractItem> tmpList = _mylibery.GetByAutor(autor);
            return ToEnItemList(tmpList);
        }

        public List<EnItem> GetByLikeAutor(string autor)
        {
            List<AbstractItem> tmpList = _mylibery.GetByLikeAutor(autor);
            return ToEnItemList(tmpList);
        }

        public List<EnItem> GetByType(Type type)
        {
            List<AbstractItem> tmpList = _mylibery.GetByType(type);
            return ToEnItemList(tmpList);
        }

        public List<EnItem> GetUnionItems(ISearch search)
        {
            List<AbstractItem> tmpList = GetUnionItems(search.Subcategory, search.Category, search.ThisType, search.Name, search.Autor);
            return ToEnItemList(tmpList);
        }
        public List<AbstractItem> GetUnionItems(eSubcategory subCategory, eCategory? categrory = null,
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

        public List<EnItem> GetItems(ISearch search)
        {
            List<AbstractItem> tmpList = GetItems(search.Subcategory, search.Category, search.ThisType, search.Name, search.Autor);
            return ToEnItemList(tmpList);
        }
        public List<AbstractItem> GetItems(eSubcategory subCategory, eCategory? categrory = null,
                                                 Type type = null, string likeName = null,
                                                 string likeAutor = null)
        {
            // return union interset of not nul parameters
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


        #endregion
        
    }
}
