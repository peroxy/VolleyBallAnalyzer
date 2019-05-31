namespace VolleyballAnalyzer
{
        public class PlayerSwitch
        {
            public PlayerSwitch(Move courtPlayer, Move reserve)
            {
                CourtPlayer = courtPlayer;
                Reserve = reserve;
            }

            public Move CourtPlayer { get; set; }
            public Move Reserve { get; set; }
        }
}
