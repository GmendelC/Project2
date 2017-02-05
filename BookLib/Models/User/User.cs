using BookLib.Contracts;
using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BookLib.Models.User
{
    [Serializable]
    public class User : ISerializable, IUser
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public eLicense License { get; set; }
        public string Pasword { get; set; }

        public User(int id, string name, string pasword, eLicense license)
        {
            this.Name = name;
            this.Id = id;
            this.Pasword = pasword;
            this.License = license;
        }
        protected User(SerializationInfo info, StreamingContext context)
        {
            Name = (string)info.GetValue("Name", typeof(string));
            Pasword = (string)info.GetValue("Pasword", typeof(string));
            Id = info.GetInt32("Id");
            License = (eLicense)info.GetValue("License", typeof(eLicense));
        }

        static public User Deserialize(SerializationInfo info, StreamingContext context)
        {
            return new User(info, context);
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name", Name, typeof(string));
            info.AddValue("Pasword", Pasword, typeof(string));
            info.AddValue("Id", Id, typeof(int));
            info.AddValue("License", License, typeof(eLicense));
        }
        public override bool Equals(object obj)
        {
            User other = obj as User;
            if (other == null)
                return false;

            return this.Id == other.Id;
        }

       
    }
}
