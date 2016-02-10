using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfferingSolutions.Datashaper.Share.Factories;
using OfferingSolutions.Datashaper.Share.Models;

namespace OfferingSolutions.Datashaper.Tests
{
    [TestClass]
    public class DatashaperTests
    {
        private readonly ParentClass _parentClass;

        public DatashaperTests()
        {
            ParentClassFactory parentClassFactory = new ParentClassFactory();
            _parentClass = parentClassFactory.CreateParentClass();
        }

        [TestMethod]
        public void List_of_Strings_Stays_The_Same_After_Datashaping()
        {
            List<string> listOfStrings = new List<string> { "Id", "Title", "Date" };
            int countbefore = listOfStrings.Count;
            Datashaper.CreateDataShapedObject(_parentClass, listOfStrings);
            int countAfter = listOfStrings.Count;

            Assert.AreEqual(countAfter, countbefore);
        }

        [TestMethod]
        public void List_of_Strings_Stays_The_Same_After_Datashaping_With_Child_Classes()
        {
            List<string> listOfStrings = new List<string> { "Id", "Title", "Date", "childClass.Description", "childClass.ismodified" };
            int countbefore = listOfStrings.Count;
            Datashaper.CreateDataShapedObject(_parentClass, listOfStrings);
            int countAfter = listOfStrings.Count;

            Assert.AreEqual(countAfter, countbefore);
        }

        [TestMethod]
        public void Datashaper_Gets_Correct_Properties_From_Parent_Class()
        {
            List<string> listOfStrings = new List<string> { "Id", "Title", "Date" };
            object dataShapedObject = Datashaper.CreateDataShapedObject(_parentClass, listOfStrings);

            IDictionary<string, object> shapedObject = (IDictionary<string, object>)dataShapedObject;

            Assert.AreEqual(listOfStrings.Count, shapedObject.Keys.Count);

            IEnumerable<string> differenceQuery = listOfStrings.Except(shapedObject.Keys);

            Assert.IsTrue(!differenceQuery.ToList().Any());
        }

        [TestMethod]
        public void Datashaper_Gets_Correct_Properties_From_Parent_And_Child_Class()
        {
            List<string> listOfStrings = new List<string> { "Id", "Title", "Date", "childClass.Description", "childClass.ismodified" };
            object dataShapedObject = Datashaper.CreateDataShapedObject(_parentClass, listOfStrings);

            IDictionary<string, object> shapedObject = (IDictionary<string, object>)dataShapedObject;

            Assert.AreEqual(4, shapedObject.Keys.Count);
            Assert.IsTrue(shapedObject.ContainsKey("childClass"));

            List<object> child = (List<object>)shapedObject["childClass"];

            Assert.IsTrue(child.Any());
            IDictionary<string, object> childObject = (IDictionary<string, object>)child.First();

            Assert.IsTrue(childObject.ContainsKey("Description"));
            Assert.IsTrue(childObject.ContainsKey("IsModified"));
        }

        [TestMethod]
        public void Datashaper_Gets_Correct_Properties_From_Parent_And_Child_Class_With_Child_Enumerable()
        {
            List<string> listOfStrings = new List<string> { "Id", "Title", "Date", "ChildClasses.Id", "ChildClasses.Description" };

            object dataShapedObject = Datashaper.CreateDataShapedObject(_parentClass, listOfStrings);

            IDictionary<string, object> shapedObject = (IDictionary<string, object>)dataShapedObject;

            Assert.AreEqual(4, shapedObject.Keys.Count);
            Assert.IsTrue(shapedObject.ContainsKey("ChildClasses"));

            List<object> child = (List<object>)shapedObject["ChildClasses"];

            Assert.AreEqual(_parentClass.ChildClasses.Count, child.Count);

            foreach (object o in child)
            {
                IDictionary<string, object> oObject = (IDictionary<string, object>)o;
                Assert.IsTrue(oObject.ContainsKey("Description"));
                Assert.IsTrue(oObject.ContainsKey("Id"));
                Assert.AreEqual(2, oObject.Keys.Count);
            }
        }

        [TestMethod]
        public void Datashaper_Gives_Correct_JSON()
        {
            List<string> listOfStrings = new List<string> { "Id", "Title", "childClass.Description", "Date", "ChildClasses.Id", "ChildClasses.Description" };

            object dataShapedObject = Datashaper.CreateDataShapedObject(_parentClass, listOfStrings);
            string serializeObject = JsonConvert.SerializeObject(dataShapedObject);
            JToken jToken = JToken.Parse(serializeObject);
            Assert.IsNotNull(jToken);
        }
    }
}
