using BookLib.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookLib.Models;

namespace Project2.Model
{
    class User : IUser
    {
        public int Id { get; set; }

        public eLicense License { get; set; }

        public string Name { get; set; }

        public string Pasword { get; set; }
    }
}
