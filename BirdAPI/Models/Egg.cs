namespace BirdAPI_lab4.Models
{
    public class Egg
    {
        public int Id { get; set; }
        public EggSize Size { get; set; }
        public int BirdId { get; set; }

        public Bird Bird { get; set; }
    }

    public enum EggSize
    {
        S,
        M,
        L,
        XL
    }
}
