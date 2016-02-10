using OfferingSolutions.Datashaper.Share.Models;

namespace OfferingSolutions.Datashaper.Share.Factories
{
    class ChildClassFactory : FactoryBase
    {
        public ChildClass CreateChildClass(ParentClass parentClass)
        {
            return new ChildClass()
            {
                Id = GetRandomNumber(),
                Description = GetRandomString(),
                IsModified = false,
                //ParentClass = parentClass,
                ParentClassId = parentClass.Id
            };
        }
    }
}
