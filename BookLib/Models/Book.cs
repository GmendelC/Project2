using BookLib.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BookLib.Models
{
    [Serializable]
    public class Book : AbstractItem, ISerializable
    {
        public Book(Guid isbn, string name, string[] Authors, DateTime firstEdition, eCategory catgoryItem,
            eSubcategory subcategoryItem, List<AbstractCopy> copys)
            : base(isbn, name, Authors, firstEdition, catgoryItem, subcategoryItem, copys)
        {
        }
        public Book(string name, string[] autors, DateTime firstEdition,
           eCategory catgoryItem, eSubcategory subcategoryItem, List<AbstractCopy> copys) 
            :this(Guid.NewGuid(), name, autors, firstEdition, catgoryItem, subcategoryItem , copys)
        {
        }

        protected Book(SerializationInfo info, StreamingContext context) : base(info, context)
        { }
        static public Book Deserialize(SerializationInfo info, StreamingContext context)
        {
            return new Book(info, context);
        }
        public override bool AddCopy(AbstractCopy copy)
        {
            if(copy is BookCopy)
                return base.AddCopy(copy);

            return false;
        }

        public override bool RemoveCopy(AbstractCopy copy)
        {
            if (copy is BookCopy)
                return base.RemoveCopy(copy);

            return false;
        }
    }
}
