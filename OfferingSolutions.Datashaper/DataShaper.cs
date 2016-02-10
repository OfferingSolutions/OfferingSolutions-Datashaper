using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace OfferingSolutions.Datashaper
{
    public static class Datashaper
    {
        public static object CreateDataShapedObject(object sourceObject, List<string> listOfFields)
        {
            if (!listOfFields.Any())
            {
                return sourceObject;
            }

            ExpandoObject objectToReturn = new ExpandoObject();

            List<string> listOfFieldsCopy = new List<string>(listOfFields);

            IEnumerable<string> listOfChildFields = GetListOfChildFields(listOfFieldsCopy);

            listOfFieldsCopy.RemoveRange(listOfChildFields);

            foreach (string field in listOfFieldsCopy)
            {
                PropertyInfo propertyinfo = GetProperty(sourceObject, field);

                object value = propertyinfo.GetValue(sourceObject, null);
                string propertyName = propertyinfo.Name;

                ((IDictionary<string, object>)objectToReturn).Add(propertyName, value);
            }

            if (listOfChildFields.Any())
            {
                List<string> listToWorkWith = listOfChildFields.ToList();

                List<string> listOfChildTypes = new List<string>();

                foreach (string s in listToWorkWith)
                {
                    string first = s.Split('.').First();

                    if (!listOfChildTypes.Contains(first))
                    {
                        listOfChildTypes.Add(first);
                    }
                }

                foreach (string childType in listOfChildTypes)
                {
                    if (!ExpandoObjectDoesContainChildType(objectToReturn, childType))
                    {
                        PropertyInfo propertyInfo =
                            sourceObject.GetType().GetProperties().FirstOrDefault(x => x.Name.ToLower() == childType.ToLower());

                        if (propertyInfo != null)
                        {
                            IEnumerable collectionToWorkWith;

                            if (PropertyDescribesACollection(propertyInfo))
                            {
                                collectionToWorkWith = GetCollectionToWorkWith(sourceObject, propertyInfo);
                            }
                            else
                            {
                                collectionToWorkWith = new List<object> { propertyInfo.GetValue(sourceObject, null) };
                            }

                            List<string> childProperties =
                             listToWorkWith.Where(x => x.Split('.')[0] == childType)
                             .Select(childPropertyWithDot => childPropertyWithDot.Split('.')[1]).ToList();

                            List<object> valueObjects = new List<object>();

                            foreach (object expense in collectionToWorkWith)
                            {
                                valueObjects.Add(CreateDataShapedObject(expense, childProperties));
                            }

                            ((IDictionary<String, Object>)objectToReturn).Add(childType, valueObjects);
                        }
                    }
                }

            }

            return objectToReturn;
        }

        private static IEnumerable GetCollectionToWorkWith(object sourceObject, PropertyInfo propertyInfo)
        {
            Type typeArg = propertyInfo.PropertyType.GetGenericArguments().First();

            IEnumerable<IEnumerable> collections = GetCollections(sourceObject);

            foreach (IEnumerable collection in collections)
            {
                Type myListElementType = collection.GetType().GetGenericArguments().FirstOrDefault();

                if (myListElementType != null && myListElementType == typeArg)
                {
                    return collection;
                }
            }

            return new List<object>();
        }

        private static bool PropertyDescribesACollection(PropertyInfo propertyInfo)
        {
            return typeof(IEnumerable).IsAssignableFrom(propertyInfo.PropertyType);
        }

        private static bool ExpandoObjectDoesContainChildType(ExpandoObject objectToReturn, string childType)
        {
            return ((IDictionary<string, object>)objectToReturn).ContainsKey(childType);
        }

        private static List<string> GetListOfChildFields(List<string> listOfFieldsCopy)
        {
            return listOfFieldsCopy.Where(x => x.Contains(".")).ToList();
        }

        public static IEnumerable<IEnumerable> GetCollections(object obj)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            Type type = obj.GetType();
            var res = new List<IEnumerable>();
            foreach (var prop in type.GetProperties())
            {
                if (typeof(IEnumerable).IsAssignableFrom(prop.PropertyType))
                {
                    var get = prop.GetGetMethod();
                    if (!get.IsStatic && get.GetParameters().Length == 0)
                    {
                        var collection = (IEnumerable)get.Invoke(obj, null);
                        if (collection != null) res.Add(collection);
                    }
                }
            }
            return res;
        }

        public static PropertyInfo GetProperty(object value, string field)
        {
            PropertyInfo property = value.GetType()
                .GetProperty(field, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (property == null)
            {
                throw new ArgumentNullException(field);
            }

            return property;
        }

        public static void RemoveRange<T>(this List<T> source, IEnumerable<T> rangeToRemove)
        {
            if (rangeToRemove == null | !rangeToRemove.Any())
            {
                return;
            }

            foreach (T item in rangeToRemove)
            {
                source.Remove(item);
            }
        }
    }
}
