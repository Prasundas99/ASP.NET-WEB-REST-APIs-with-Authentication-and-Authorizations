namespace SuperHeroApi
{
    public class SuperHeros
    {
        public int Id { get; set; }
        public String Name { get; set; } = string.Empty;

        public String FirstName { get; set; } = string.Empty;
        public String LastName { get; set; } = string.Empty;
        public String Place { get; set;} = string.Empty;

        public String HeroName { get; set; } = string.Empty;

        public DateTime CreatedTimestamp { get; set; } = DateTime.Now;
    }
}