using BookLib.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLib.Models.ToUx
{
    class BoolResult : IBoolResult
    {
        public string Message { get; set; }
        public bool Result { get; set; }
    }
}
