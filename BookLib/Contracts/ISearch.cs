using BookLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLib.Contracts
{
    public interface ISearch
    {
        eSubcategory Subcategory { get; set; }
        eCategory? Category { get; set; }
        Type ThisType { get; set; }
        string Name { get; set; }
        string Autor { get; set; }

    }
}
