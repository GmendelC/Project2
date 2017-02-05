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
    public class JornalCopy : AbstractCopy, ISerializable
    {
        public JornalCopy(Guid id) : base(id)
        {
        }
        public JornalCopy() : this(Guid.NewGuid())
        {
        }
        public JornalCopy(SerializationInfo info, StreamingContext context) :base (info, context)
        {
        }

        static public JornalCopy DeSerialize(SerializationInfo info, StreamingContext context)
        {
            return new JornalCopy(info, context);
        }
    }
}