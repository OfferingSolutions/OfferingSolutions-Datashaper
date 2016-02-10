using System;
using OfferingSolutions.Datashaper.Share.Models;

namespace OfferingSolutions.Datashaper.Share.Factories
{
    public class ParentClassFactory : FactoryBase
    {
        private readonly ChildClassFactory _childClassFactory;

        public ParentClassFactory()
        {
            _childClassFactory = new ChildClassFactory();
        }

        public ParentClass CreateParentClass()
        {
            ParentClass parentClass = new ParentClass
            {
                Id = GetRandomNumber(),
                Date = DateTime.Now,
                Title = GetRandomString(),
                UserId = GetGuid()
            };

            ChildClass childClass = _childClassFactory.CreateChildClass(parentClass);

            parentClass.ChildClass = childClass;

            for (int i = 0; i < 3; i++)
            {
                parentClass.ChildClasses.Add(_childClassFactory.CreateChildClass(parentClass));
            }

            return parentClass;
        }

    }
}
