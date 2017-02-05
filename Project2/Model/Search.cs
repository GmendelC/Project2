using BookLib.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookLib.Models;

namespace Project2.Model
{
    class Search : ISearch
    {
        public string Autor { get; set; }
        

        public eCategory? Category { get; set; }

        public string Name { get; set; }
        public eSubcategory Subcategory { get; set; }
        public Type ThisType { get; set; }
    }
}
