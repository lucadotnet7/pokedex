namespace Domain
{
    public sealed class Element
    {
        public int Id { get; set; }
        public string Description { get; set; }

        public override string ToString()
        {
            return Description;
        }
    }
}
