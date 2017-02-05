using BookLib.Contracts;
using BookLib.Models;
using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace BookLib
{
    [Serializable]
    public class BookCopy : AbstractCopy, ISerializable
    {
        public DateTime Edition { get; set; }
        public string Plubisher { get; set; }

        public BookCopy(Guid id, DateTime edition, string plubisher) : base(id)
        {
            this.Edition = edition;
            this.Plubisher = plubisher;
        }
        public BookCopy(DateTime edition, string plubisher) : this(Guid.NewGuid(), edition, plubisher)
        {
        }

        protected BookCopy(SerializationInfo info, StreamingContext context) :base( info,  context)
        {
            Edition =info.GetDateTime("Edition");
            Plubisher = (string)info.GetValue("Edition", typeof(string));
        }

        static public BookCopy Deserialize(SerializationInfo info, StreamingContext context)
        {
            return new BookCopy(info, context);
        }
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("Edition", Edition, typeof(DateTime));
            info.AddValue("Plubisher", Plubisher, typeof(string));
        }
    }
}