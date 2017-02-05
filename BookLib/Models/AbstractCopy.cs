using BookLib.Contracts;
using BookLib.Models;
using System;
using System.Runtime.Serialization;

namespace BookLib.Models
{
    [Serializable]
    abstract public class AbstractCopy: ISerializable
    {
        public Guid CopyId { get; set; }
        public eStatus CopyStatus { get; set; }
        public DateTime? RequestDate { get; set; }
        public int KeeperId { get; set; }

        public AbstractCopy(Guid id)
        {
            this.CopyId = id;
            this.CopyStatus = eStatus.In;
        }

        protected AbstractCopy(SerializationInfo info, StreamingContext context)
        {
            CopyId = (Guid)info.GetValue("CopyId", typeof(Guid));
            CopyStatus = (eStatus)info.GetValue("CopyStatus", typeof(eStatus));
            RequestDate = (DateTime?) info.GetValue("RequestDate",typeof(DateTime?));
            KeeperId = info.GetInt32("KeeperId");
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("CopyId", CopyId, typeof(Guid));
            info.AddValue("CopyStatus", CopyStatus, typeof(eStatus));
            info.AddValue("RequestDate", RequestDate, typeof(DateTime?));
            info.AddValue("KeeperId", KeeperId, typeof(Int32));
        }

        public bool Request(int keeperId)
        {
            if (CopyStatus == eStatus.In)
            {
                CopyStatus = eStatus.Out;
                RequestDate = DateTime.Now;
                KeeperId = keeperId;
                return true;
            }
            return false;
        }

        public bool Return()
        {
            CopyStatus = eStatus.In;
            RequestDate = default(DateTime);
            KeeperId = 0;
            return true;
        }

        public override bool Equals(object obj)
        {
            AbstractCopy other = obj as AbstractCopy;
            if (other == null)
                return false;

            return this.CopyId == other.CopyId;
        }
    }
}
