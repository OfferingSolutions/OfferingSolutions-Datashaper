using System;
using System.Linq;

namespace OfferingSolutions.Datashaper.Share.Factories
{
    public class FactoryBase
    {
        internal string GetRandomString()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(
                Enumerable.Repeat(chars, 8)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
        }

        internal int GetRandomNumber()
        {
            return new Random().Next(1000000);
        }

        internal string GetGuid()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
