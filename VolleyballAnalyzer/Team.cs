using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolleyballAnalyzer
{
    public class Team
    {
        public Team(string name, IList<Player> players, CourtSide side, int setsWon)
        {
            Name = name;
            Players = players;
            Side = side;
            SetsWon = setsWon;
        }

        public string Name { get; set; }
        public IList<Player> Players { get; set; }
        public CourtSide Side { get; set; }

        public int SetsWon { get; set; }
    }
}
