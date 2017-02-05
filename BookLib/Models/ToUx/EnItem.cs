using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLib.Models.ToUx
{
    public class EnItem
    {
        public Guid? ISBN { get; set; }
        public string Name { get; set; }
        public string[] Autors { get; set; }
        public DateTime FirstEdition { get; set; }
        public eCategory CatgoryItem { get; set; }
        public eSubcategory SubcategoryItem { get; set; }
        public List<EnCopy> Copys { get; set; }
        public string Plubisher { get; set; }

        public EnItem()
        { }

        public EnItem(AbstractItem item)
        {
            this.ISBN = item.ISBN;
            this.Name = item.Name;
            this.Autors = item.Autors;
            this.FirstEdition = item.FirstEdition;
            this.CatgoryItem = item.CatgoryItem;
            this.SubcategoryItem = item.SubcategoryItem;

            if (item is Book)
            {
                this.Plubisher = null;
            }
            else
            {
                this.Plubisher = ((Jornal)item).Plubisher;
            }
            this.Copys = EnCopy.GenerateCopys(item.GetAllCoppy());
        }
        public class EnCopy
        {
            public Guid? CopyId { get; set; }
            public eStatus CopyStatus { get; set; }
            public DateTime? RequestDate { get; set; }
            public int KeeperId { get; set; }
            public DateTime? Edition { get; set; }
            public string Plubisher { get; set; }

            internal static List<EnCopy> GenerateCopys(List<AbstractCopy> Copys)
            {
                List<EnCopy> newList = new List<EnCopy>();

                foreach (var item in Copys)
                {
                    EnCopy newCopy = new EnCopy() {
                                                    CopyId = item.CopyId,
                                                    CopyStatus = item.CopyStatus,
                                                    KeeperId = item.KeeperId,
                                                    RequestDate = item.RequestDate};
                    if (item is BookCopy)
                    {
                        newCopy.Edition = ((BookCopy)item).Edition;
                        newCopy.Plubisher = ((BookCopy)item).Plubisher;
                    }
                    else
                    {
                        newCopy.Edition = null;
                        newCopy.Plubisher = null;
                    }
                    newList.Add(newCopy);
                }

                return newList;
            }
            public static List<AbstractCopy> ToBll(List<EnCopy> copys)
            {
                List<AbstractCopy> newList = new List<AbstractCopy>();
                foreach (var item in copys)
                {
                    AbstractCopy temp;
                    if (item.Plubisher != null)
                    {
                        if (item.CopyId == null)
                        {
                            temp = new BookCopy(item.Edition.Value, item.Plubisher);
                        }
                        else
                        {
                            temp = new BookCopy(item.CopyId.Value, item.Edition.Value, item.Plubisher);
                        }
                    }
                    else
                    {
                        if (item.CopyId == null)
                        {
                            temp = new JornalCopy();
                        }
                        else
                        {
                            temp = new JornalCopy(item.CopyId.Value);
                        }
                    }

                    temp.CopyStatus = item.CopyStatus;
                    temp.KeeperId = item.KeeperId;
                    temp.RequestDate = item.RequestDate;

                    newList.Add(temp);
                }
                return newList;
            }
        }
    }
}
