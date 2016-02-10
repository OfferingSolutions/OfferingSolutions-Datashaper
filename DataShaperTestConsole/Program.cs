using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json;
using OfferingSolutions.Datashaper;
using OfferingSolutions.Datashaper.Share.Factories;
using OfferingSolutions.Datashaper.Share.Models;

namespace DataShaperTestConsole
{

    class Program
    {
        static void Main(string[] args)
        {
            ParentClassFactory parentClassFactory = new ParentClassFactory();

            List<string> listOfStrings = new List<string> { "Id", "Title", "Date", "ChildClasses.id", "ChildClasses.IsModified", "ChildClasses.Description" };

            List<ParentClass> parents = new List<ParentClass>()
            {
                parentClassFactory.CreateParentClass(),
                parentClassFactory.CreateParentClass()
                //...
            };

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var enumerable = parents.Select(x => Datashaper.CreateDataShapedObject(x, listOfStrings));
            stopwatch.Stop();

            string serializeObject = JsonConvert.SerializeObject(enumerable, Formatting.Indented);

            Console.WriteLine(serializeObject);
            Console.WriteLine("Needed " + stopwatch.Elapsed.TotalSeconds + " seconds");
            Console.ReadLine();
        }
    }
}
