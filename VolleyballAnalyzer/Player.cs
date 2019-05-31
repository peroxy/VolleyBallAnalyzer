namespace VolleyballAnalyzer
{
    public class Player
    {
        public Player(string name, string surname, int jerseyNumber, int? startingZone)
        {
            Name = name;
            Surname = surname;
            JerseyNumber = jerseyNumber;
            StartingZone = startingZone;
        }

        public Player()
        {
        }

        public string Name { get; set; }
        public string Surname { get; set; }
        public int JerseyNumber { get; set; }

        public int? StartingZone { get; set; }
    }
}