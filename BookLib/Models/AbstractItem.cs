using BookLib.Contracts;
using BookLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BookLib.Models
{
    [Serializable]
   public abstract class AbstractItem: ISerializable
    {
        public Guid ISBN { get; set; }
        public string Name { get; set; }
        public string[] Autors { get; set; }
        public DateTime FirstEdition { get; set; }
        public eCategory CatgoryItem { get; set; }

        public AbstractCopy this[Guid id]
        {
            get { return GetCopy(id);}
        }

        public string[] AutorsLower {
            get{  return Autors.Select<string, string>(i => i.ToLower()).ToArray(); }
        }
        public int CopysCount { get { return _copys.Count; } }

        private eSubcategory _subcategoryItem;
        public eSubcategory SubcategoryItem
        {
            get { return _subcategoryItem; }
            set
            {
                if (SubcategoryManeger.SubValid(CatgoryItem, value))
                    _subcategoryItem = value;
                else
                    throw new FormatException("subcategory not valid");
            }
        }


        protected readonly Dictionary<Guid, AbstractCopy> _copys;

        public AbstractItem(Guid isbn, string name, string[] autors, DateTime firstEdition,
            eCategory catgoryItem, eSubcategory subcategoryItem, List<AbstractCopy> copys)
        {
            this.ISBN = isbn;
            this.Name = name;
            this.Autors = autors;
            this.FirstEdition = firstEdition;
            this.CatgoryItem = catgoryItem;
            this.SubcategoryItem = subcategoryItem;
            this._copys = new Dictionary<Guid, AbstractCopy>();
            AddCopys(copys);
        }

        protected AbstractItem(SerializationInfo info, StreamingContext context)
        {
            ISBN = (Guid)info.GetValue("ISBN", typeof(Guid));
            Name = (string)info.GetValue("Name", typeof(string));
            Autors = (string[])info.GetValue("Autors", typeof(string[]));
            FirstEdition = info.GetDateTime("FirstEdition");
            CatgoryItem = (eCategory)info.GetValue("CatgoryItem",typeof(eCategory));
            SubcategoryItem = (eSubcategory)info.GetValue("SubcategoryItem", typeof(eSubcategory));
            _copys = (Dictionary<Guid, AbstractCopy>)info.GetValue("_copys",typeof(Dictionary<Guid, AbstractCopy>));
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ISBN", ISBN, typeof(Guid));
            info.AddValue("Name", Name, typeof(string));
            info.AddValue("Autors", Autors, typeof(string[]));
            info.AddValue("FirstEdition", FirstEdition, typeof(DateTime));
            info.AddValue("CatgoryItem", CatgoryItem, typeof(eCategory));
            info.AddValue("SubcategoryItem", SubcategoryItem, typeof(eSubcategory));
            info.AddValue("_copys", _copys, typeof(Dictionary<Guid, AbstractCopy>));
        }

        public void AddCopys(List<AbstractCopy> copys)
        {
            foreach (var item in copys)
            {
                AddCopy(item);
            }
        }

       
        public AbstractCopy GetCopy(Guid key)
        {
            if (_copys.ContainsKey(key))
                return _copys[key];

            return null;
        }
        public AbstractCopy GetCopy()
        {
            foreach (var item in _copys)
            {
                if (item.Value != null && item.Value.CopyStatus == eStatus.In)
                {
                    return item.Value;
                }
            }
            return null;
        }

        public AbstractCopy GetFirstCopy()
        {
            foreach (var item in _copys)
            {
                return item.Value;
            }
            return null;
        }
        public virtual bool RemoveCopy(AbstractCopy copy)
        {
            return RemoveCopy(copy.CopyId);
        }

        public bool RemoveCopy(Guid key)
        {
            if (_copys.ContainsKey(key))
            {
                _copys.Remove(key);
                return true;
            }
            return false;
        }

        public virtual bool AddCopy(AbstractCopy copy)
        {
            if (!_copys.ContainsKey(copy.CopyId))
            {
                _copys.Add(copy.CopyId, copy);
                return true;
            }
            return false;
        }

        public List<AbstractCopy> GetAllCoppy()
        {
            var l = _copys.Select(i => i.Value);
            return l.ToList<AbstractCopy>();
        }

        public override bool Equals(object obj)
        {
            AbstractItem other = obj as AbstractItem;
            if (other == null)
                return false;

            return this.ISBN == other.ISBN;
        }

    }
}
