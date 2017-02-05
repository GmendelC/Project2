using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public static class DBData
    {
        public const string FilePath = "DBData.osl";

        /// <summary>
        ///  Serializes the object to "DBData.osl" File.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        public static void Serialize<T>(this T obj)
        {
            using (Stream stream = File.Open(FilePath, FileMode.Create))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(stream, obj);
            }
        }

        /// <summary>
        ///  Deserializes the specified stream into an object.
        ///  From the "DBData.osl" File.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T DeSerialize<T>()
        {
            using (Stream stream = File.Open(FilePath, FileMode.OpenOrCreate))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                T res = (T)binaryFormatter.Deserialize(stream);
                return res;
            }

        }

    }

}
