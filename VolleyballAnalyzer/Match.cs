using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolleyballAnalyzer
{
    public class Match
    {
        public Match(Team firstTeam, Team secondTeam, IList<Move> matchMoves)
        {
            FirstTeam = firstTeam;
            SecondTeam = secondTeam;
            MatchMoves = matchMoves;
        }

        public Team FirstTeam { get; set; }
        public Team SecondTeam { get; set; }

        public IList<Move> MatchMoves { get; set; }
    }
}
