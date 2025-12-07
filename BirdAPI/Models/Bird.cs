namespace BirdAPI_lab4.Models
{
    public class Bird
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Species { get; set; }

        public ICollection<Egg> Eggs { get; set; }
    }
}
