namespace BirdAPI_lab4.DTOs
{
    public class BirdDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Species { get; set; }
    }

    public class CreateBirdDto
    {
        public string Name { get; set; }
        public string Species { get; set; }
    }

    public class UpdateBirdDto
    {
        public string Name { get; set; }
        public string Species { get; set; }
    }
}