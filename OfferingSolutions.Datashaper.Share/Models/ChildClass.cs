namespace OfferingSolutions.Datashaper.Share.Models
{
    public class ChildClass
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool IsModified { get; set; }

        public virtual ParentClass ParentClass { get; set; }
        public int ParentClassId { get; set; }
    }
}