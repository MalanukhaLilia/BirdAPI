using BirdAPI_lab4.Models;

namespace BirdAPI_lab4.DTOs
{
    public class EggDto
    {
        public int Id { get; set; }
        public EggSize Size { get; set; }
        public int BirdId { get; set; }
    }

    public class CreateEggDto
    {
        public EggSize Size { get; set; }
        public int BirdId { get; set; }
    }

    public class UpdateEggDto
    {
        public EggSize Size { get; set; }
        public int BirdId { get; set; }
    }
}