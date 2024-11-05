using System.ComponentModel;

namespace Domain
{
    public sealed class Pokemon
    {
        public int Id { get; set; }
        [DisplayName("Número")]
        public int Number { get; set; }
        public string Name { get; set; }
        [DisplayName("Descripción")]
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public Element Type { get; set; }
        public Element Weakness { get; set; }
        public bool Active { get; set; }
    }
}
