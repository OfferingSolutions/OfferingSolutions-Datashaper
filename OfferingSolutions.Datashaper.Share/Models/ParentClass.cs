using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace OfferingSolutions.Datashaper.Share.Models
{
    public class ParentClass
    {
        public ParentClass()
        {
            ChildClasses = new Collection<ChildClass>();
            ChildClass = new ChildClass();
        }

        public int Id { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }

        public ICollection<ChildClass> ChildClasses { get; set; }
        public ChildClass ChildClass { get; set; }
    }
}