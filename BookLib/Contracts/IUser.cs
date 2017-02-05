using BookLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLib.Contracts
{
    public interface IUser
    {
        string Name { get; set; }

        int Id { get; set; }

        string Pasword { get; set; }

        eLicense License { get; set; }
    }
}
