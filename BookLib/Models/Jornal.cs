using BookLib;
using BookLib.Contracts;
using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BookLib.Models
{
    [Serializable]
    public class Jornal : AbstractItem, ISerializable
    {
        public string Plubisher { get; set; }

        public Jornal(Guid isbn, string name, string[] autors, DateTime firstEdition,
            eCategory catgoryItem, eSubcategory subcategoryItem, string plubisher, List<AbstractCopy> copys) :
            base(isbn, name, autors, firstEdition, catgoryItem, subcategoryItem, copys)
        {
            this.Plubisher = plubisher;
        }
        public Jornal(string name, string[] autors, DateTime firstEdition,
          eCategory catgoryItem, eSubcategory subcategoryItem, string plubisher, List<AbstractCopy> copys)
            : this(Guid.NewGuid(), name, autors, firstEdition, catgoryItem, subcategoryItem, plubisher, copys)
        {
        }

        protected Jornal(SerializationInfo info, StreamingContext context): base( info,  context)
        {
            Plubisher = (string)info.GetValue("Plubisher", typeof(string));
        }

        static public Jornal Deseralize(SerializationInfo info, StreamingContext context)
        {
            return new Jornal(info, context);
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("Plubisher", Plubisher, typeof(string));
        }

        public override bool AddCopy(AbstractCopy copy)
        {
            if (copy is JornalCopy)
                return base.AddCopy(copy);

            return false;
        }

        public override bool RemoveCopy(AbstractCopy copy)
        {
            if (copy is JornalCopy)
                return base.RemoveCopy(copy);

            return false;
        }
    }
}
