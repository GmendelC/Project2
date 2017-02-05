using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLib.Contracts
{
    interface ILoggerList<T>
    {
        LinkedList<List<T>> LogList { get; set; }

        void Add(List<T> list);
    }
}
