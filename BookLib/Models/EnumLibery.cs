using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLib.Models
{
    [Serializable]
    public enum eLicense { Master, Admint, Enployee, Visitor}
    [Serializable]
    public enum eStatus { In, Out, Damage }
    [Serializable]
    public enum eCategory { Study, Cook, Reading, News, Excursions }
    [Serializable]
    public enum eSubcategory
    {
        None,
        Math, Science, History, Medicin, Computer,
        Meaty, Milky,
        Drama, Romance, Mystery, Action, Children, Comics,
        Water, Land
    }
    public static class SubcategoryManeger
    {
        public static Dictionary<eCategory, List<eSubcategory>> SubcategoryDictionary =
             new Dictionary<eCategory, List<eSubcategory>>
             {
                {eCategory.Study, new List<eSubcategory> { eSubcategory.None,
                    eSubcategory.Science, eSubcategory.Medicin, eSubcategory.Computer,
                    eSubcategory.History, eSubcategory.Math} },
                 {eCategory.Cook, new List<eSubcategory> { eSubcategory.None, eSubcategory.Meaty,
                     eSubcategory.Milky} },
                 {eCategory.Reading, new List<eSubcategory> { eSubcategory.None, eSubcategory.Action,
                     eSubcategory.Children, eSubcategory.Comics, eSubcategory.Drama, eSubcategory.Mystery,
                     eSubcategory.Romance,} },
                 {eCategory.News, new List<eSubcategory> {eSubcategory.None } },
                 {eCategory.Excursions, new List<eSubcategory> {eSubcategory.None, eSubcategory.Water, eSubcategory.Land} }
             };

        public static bool SubValid(eCategory key, eSubcategory value)
        {
            return SubcategoryDictionary[key].Contains(value);
        }
    }
}
