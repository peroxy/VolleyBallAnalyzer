using System;
using System.Linq;

namespace VolleyballAnalyzer
{
        public class Move
        {
            public Move(Player player, Action action, CourtSide side, int setNumber, int zone, string teamName)
            {
                Player = player;
                Action = action;
                Side = side;
                SetNumber = setNumber;
                Zone = zone;
                TeamName = teamName;
            }

            public Move(Player player, CourtSide side, int zone)
            {
                Player = player;
                Side = side;
                Zone = zone;
            }

            public string TeamName { get; set; }
            public Player Player { get; set; }
            public CourtSide Side { get; set; }
            public Action Action { get; set; }
            public int SetNumber { get; set; }
            public int Zone { get; set; }

            public bool IsSuccess
            {
                get
                {
                    switch (Action)
                    {
                        case Action.NapadSuccess:
                        case Action.SprejemSuccess:
                        case Action.ServisSuccess:
                        case Action.PodajaSuccess:
                        case Action.BlokSuccess:
                            return true;
                        case Action.NapadFail:
                        case Action.SprejemFail:
                        case Action.ServisFail:
                        case Action.PodajaFail:
                        case Action.BlokFail:
                        case Action.Prestop:
                        case Action.Mreza:
                        case Action.Drugo:
                            return false;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

            public override string ToString()
            {
                if (Player.JerseyNumber == 0)
                {
                    return $"{Player.Name} - {Action.GetDescription()} - Cona {Zone}";
                }

                if (Action == Action.Menjava)
                {
                    return $"{Player.Name.First()}. {Player.Surname} ({Player.JerseyNumber}) - {Action.GetDescription()}";
                }

                return $"{Player.Name.First()}. {Player.Surname} ({Player.JerseyNumber}) - {Action.GetDescription()} - Cona {Zone}";
            }
        }
}
