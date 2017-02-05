using BookLib.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BookLib
{
    [Serializable]
    public class ItemColection: ISerializable
    {
        readonly List<AbstractItem> _itemList;
        public int BookCount { get { return GetByType(typeof(Book)).Count; } }
        public int JornalCount { get { return GetByType(typeof(Jornal)).Count; } }
        public int BookCopysCount { get { return GetCopysCount(GetByType(typeof(Book))); } }
        public int JornalCopysCount { get { return GetCopysCount(GetByType(typeof(Jornal))); } }
        public int Count { get { return _itemList.Count; } }
        public int CopysCount { get { return GetCopysCount(_itemList); } }

        public AbstractItem this[Guid iSBN]
        {
            get { return GetByISBN(iSBN); }
        }

        public static int GetCopysCount(IEnumerable<AbstractItem> items )
        {
           return items.Select(i => i.CopysCount).Sum();
        }

        public ItemColection()
        {
            _itemList = new List<AbstractItem>();
        }

        protected ItemColection(SerializationInfo info, StreamingContext context)
        {
            _itemList = (List<AbstractItem>)info.GetValue("_itemList", typeof(List<AbstractItem>));
        }

        static public ItemColection Deserialize(SerializationInfo info, StreamingContext context)
        {
            return new ItemColection(info, context);
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("_itemList", _itemList, typeof(List<AbstractItem>));
        }
        #region ItemLogic
        public bool Add(AbstractItem newItem)
        {
            // if have this book add new copys and return false.
            // if was not add new book return true.

            if (_itemList.Contains(newItem))
            {
                int i = _itemList.IndexOf(newItem);// have this book add the copys
                foreach (var item in newItem.GetAllCoppy())
                {
                    _itemList[i].AddCopy(item);
                }

                return false;
            }

            _itemList.Add(newItem);

            return true;

        }

        public bool Remove(AbstractItem oldItem)
        {
            // retun false if have not this book.
            // remove book if have not copys.
            // update the counters

            if (_itemList.Contains(oldItem))
            {
                // retun false if have not this book

                int i = _itemList.IndexOf(oldItem);// have this book remove the copys
                foreach (var item in oldItem.GetAllCoppy())
                {
                    _itemList[i].RemoveCopy(item);
                }


                if (_itemList[i].CopysCount == 0)// no copys remove book
                {
                    _itemList.RemoveAt(i);
                }
                return true;
            }
            return false;
        }

        public bool Upadate(AbstractItem updateItem)
        {
            // remove and add this item
                bool remove = Remove(updateItem);
                return remove && Add(updateItem);
        }
        #endregion

        #region Search
        public AbstractItem GetByISBN(Guid isbn)
        {
            return _itemList.Find(i => i.ISBN == isbn);
        }

        public List<AbstractItem> GetAll()
        {
            return _itemList;
        }

        public List<AbstractItem> GetByName(string name)
        {
            name.ToLower();

            var Items = from i in _itemList
                        where i.Name.ToLower() == name
                        orderby i.FirstEdition
                        select i;

            return Items.ToList<AbstractItem>();
        }

        public List<AbstractItem> GetByLikeName(string name)
        {
            name.ToLower();

            var Items = from i in _itemList
                        where i.Name.ToLower().Contains(name)
                        orderby i.Name
                        select i;

            return Items.ToList<AbstractItem>();
        }
        public List<AbstractItem> GetByCategory(eCategory category, eSubcategory subcategory)
        {
            var Items = from i in _itemList
                        where i.CatgoryItem == category & i.SubcategoryItem == subcategory
                        orderby i.Name
                        select i;

            return Items.ToList<AbstractItem>();
        }

        public List<AbstractItem> GetByCategory(eCategory category)
        {
            var Items = from i in _itemList
                        where i.CatgoryItem == category
                        orderby i.Name
                        select i;

            return Items.ToList<AbstractItem>();
        }

        public List<AbstractItem> GetByCategory(eSubcategory subcategory)
        {
            var Items = from i in _itemList
                        where i.SubcategoryItem == subcategory
                        orderby i.Name
                        select i;

            return Items.ToList<AbstractItem>();
        }

        public List<AbstractItem> GetByAutor(string autor)
        {
            autor.ToLower();

            var Items = from i in _itemList
                        where i.AutorsLower.Contains<string>(autor)
                        orderby i.Name
                        select i;

            return Items.ToList<AbstractItem>();
        }

        public List<AbstractItem> GetByLikeAutor(string autor)
        {
            autor.ToLower();

            var items = from i in _itemList
                        where i.AutorsLower.Where<string>(j => j.Contains(autor)).Count<string>() > 0
                        orderby i.Name
                        select i;

            return items.ToList<AbstractItem>();
        }

        public List<AbstractItem> GetByType(Type type)
        {
            return _itemList.Where<AbstractItem>(
                i => i.GetType().Equals(type)).OrderBy(i => i.Name).ToList<AbstractItem>();
        } 
        #endregion

     
    }
}
