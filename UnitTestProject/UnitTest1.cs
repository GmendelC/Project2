using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BookLib.Models;
using BookLib;
using System.Collections.Generic;
using System.Linq;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestSubcategory()
        {
            Assert.IsTrue (SubcategoryManeger.SubValid(eCategory.Cook, eSubcategory.Meaty));
            Assert.IsFalse(SubcategoryManeger.SubValid(eCategory.Cook, eSubcategory.Medicin));
        }
        [TestMethod]
        public void TestAllcopy()
        {
            Book b1 = new Book("aaa",new string[] { "aaa" }, DateTime.Now, eCategory.Cook,
                eSubcategory.Meaty, new List<AbstractCopy> { new BookCopy(DateTime.Now, "aaa") });
            
            Assert.IsNotNull ( b1.GetAllCoppy());
            var lt = new List<AbstractCopy> { new BookCopy(DateTime.Now, "aaa") };
            Assert.AreEqual(b1.GetAllCoppy(), lt);
        }

        [TestMethod]
        public void TestItemColectionSearch()
        {
            ItemColection ic = GetItemColection();

            var list =ic.GetAll();

            var get = ic.GetByAutor("aaa");

            var exepcted = from i in list
                           where i.Autors.Select(a => a.ToLower()).Contains("aaa")
                           orderby i.Name
                           select i;

            Assert.Equals(get, exepcted.ToList<AbstractItem>());

            get = ic.GetByLikeAutor("a");

            exepcted = from i in list
                       where i.Autors.Select(a => a.ToLower()).Where<string>(j => j.Contains("a")).Count<string>() > 0
                       orderby i.Name
                       select i;

            Assert.Equals(get, exepcted.ToList<AbstractItem>());


            get = ic.GetByName("aaa");

            exepcted = from i in list
                       where i.Autors.Select(a => a.ToLower()).Where<string>(j => j.Contains("a")).Count<string>() > 0
                       orderby i.Name
                       select i;

            Assert.Equals(get, exepcted.ToList<AbstractItem>());


            get = ic.GetByLikeName("a");

            exepcted = from i in list
                       where i.Autors.Select(a => a.ToLower()).Where<string>(j => j.Contains("a")).Count<string>() > 0
                       orderby i.Name
                       select i;

            Assert.Equals(get, exepcted.ToList<AbstractItem>());


            get = ic.GetByType(typeof(Jornal));

            exepcted = list.Where(ai => ai.GetType()== typeof(Jornal)).OrderBy(i => i.Name);

            Assert.Equals(get, exepcted.ToList<AbstractItem>());

            var getOne = ic.GetByISBN(list[0].ISBN);

            Assert.Equals(getOne, list[0]);
        }

        [TestMethod]
        public void TestGetByCategory()
        {
            ItemColection ic = GetItemColection();

            var get = ic.GetByCategory(eCategory.Cook, eSubcategory.Meaty);

            var list = ic.GetAll();
            var exepcted = from i in list
                           where i.CatgoryItem == eCategory.Cook &
                           i.SubcategoryItem == eSubcategory.Meaty
                           orderby i.Name
                           select i;

            Assert.Equals(get, exepcted.ToList<AbstractItem>());

            get = ic.GetByCategory(eCategory.Cook);

            exepcted = from i in list
                       where i.CatgoryItem == eCategory.Cook
                       orderby i.Name
                       select i;
            Assert.Equals(get, exepcted.ToList<AbstractItem>());

            get = ic.GetByCategory(eSubcategory.None);

            exepcted = from i in list
                       where i.SubcategoryItem == eSubcategory.None
                       orderby i.Name
                       select i;
            Assert.Equals(get, exepcted.ToList<AbstractItem>());

        }

        [TestMethod]
        public static ItemColection GetItemColection()
        {
            ItemColection ic = new ItemColection();
            ic.Add(new Book("aaa", new string[] { "aaa" },
                DateTime.Now, eCategory.Cook, eSubcategory.Meaty,
                new List<AbstractCopy> { new BookCopy(DateTime.Now, "aaa") }));

            ic.Add(new Jornal("bbb", new string[] { "bbb" },
                DateTime.Now, eCategory.Cook, eSubcategory.None, "bbb",
                new List<AbstractCopy> { new JornalCopy() }));

            ic.Add(new Jornal("ccc", new string[] { "ccc" },
                DateTime.Now, eCategory.Study, eSubcategory.None, "ccc",
                new List<AbstractCopy> { new JornalCopy() }));

            var b1 = new Jornal("ccc", new string[] { "ccc" },
                DateTime.Now, eCategory.Study, eSubcategory.None, "ccc",
                new List<AbstractCopy> { new JornalCopy() });

            b1.AddCopy(new JornalCopy());
            return ic;
        }


    }
}
