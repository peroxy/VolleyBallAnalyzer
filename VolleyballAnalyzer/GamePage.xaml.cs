using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VolleyballAnalyzer
{
    /// <summary>
    /// Interaction logic for GamePage.xaml
    /// </summary>
    public partial class GamePage : Page
    {
        private readonly string _firstTeamName;
        private readonly IList<Player> _firstTeamPlayers;
        private CourtSide _firstTeamSide = CourtSide.Left;
        private int _firstTeamScore = 0;
        private int _firstTeamSetScore = 0;

        private readonly string _secondTeamName;
        private readonly IList<Player> _secondTeamPlayers;
        private CourtSide _secondTeamSide = CourtSide.Right;
        private int _secondTeamScore = 0;
        private int _secondTeamSetScore = 0;

        private int _currentSet = 1;
        private CourtSide _currentServeSide;

        private IList<Move> MatchMoves = new List<Move>();

        public GamePage(string firstTeamName, IList<Player> firstTeamPlayers, string secondTeamName,
            IList<Player> secondTeamPlayers)
        {
            _firstTeamName = firstTeamName;
            _firstTeamPlayers = firstTeamPlayers;
            _secondTeamName = secondTeamName;
            _secondTeamPlayers = secondTeamPlayers;

            InitializeComponent();

            SetZoneNumbers(firstTeamPlayers, _firstTeamSide);
            SetZoneNumbers(secondTeamPlayers, _secondTeamSide);

            LeftTeamLabel.Content = _firstTeamName;
            RightTeamLabel.Content = _secondTeamName;
            ScoreLabel.Content = $"{_firstTeamScore} - {_secondTeamScore}";
        }

        private void RotatePlayers(IList<Player> players)
        {
            foreach (Player player in players)
            {
                if (player.StartingZone != null)
                {
                    if (player.StartingZone == 1)
                    {
                        player.StartingZone = 6;
                    }
                    else
                    {
                        player.StartingZone = player.StartingZone - 1;
                    }
                }
            }
        }

        private void SetZoneNumbers(IList<Player> players, CourtSide side)
        {
            if (side == CourtSide.Left)
            {
                LeftZoneOne.Content = GetPlayer(players, 1).JerseyNumber;
                LeftZoneTwo.Content = GetPlayer(players, 2).JerseyNumber;
                LeftZoneThree.Content = GetPlayer(players, 3).JerseyNumber;
                LeftZoneFour.Content = GetPlayer(players, 4).JerseyNumber;
                LeftZoneFive.Content = GetPlayer(players, 5).JerseyNumber;
                LeftZoneSix.Content = GetPlayer(players, 6).JerseyNumber;
            }
            else
            {
                RightZoneOne.Content = GetPlayer(players, 1).JerseyNumber;
                RightZoneTwo.Content = GetPlayer(players, 2).JerseyNumber;
                RightZoneThree.Content = GetPlayer(players, 3).JerseyNumber;
                RightZoneFour.Content = GetPlayer(players, 4).JerseyNumber;
                RightZoneFive.Content = GetPlayer(players, 5).JerseyNumber;
                RightZoneSix.Content = GetPlayer(players, 6).JerseyNumber;
            }
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point clickPoint = e.GetPosition(CourtImage);
            var zone = GetZone(clickPoint);

            var courtSide = GetCourtSide(clickPoint);
            var players = _firstTeamSide == courtSide
                ? _firstTeamPlayers.Where(x => x.StartingZone != null).OrderBy(x => x.JerseyNumber)
                : _secondTeamPlayers.Where(x => x.StartingZone != null).OrderBy(x => x.JerseyNumber);

            var menu = new ContextMenu();
            MenuItem menuItem;
            foreach (var player in players)
            {
                menuItem = new MenuItem
                {
                    Header = $"{player.JerseyNumber} - {player.Name.First()}. {player.Surname}",
                    Tag = new Move(player, courtSide, zone)
                };
                menuItem.Click += MenuItem_Click;
                menu.Items.Add(menuItem);
            }

            menuItem = new MenuItem
            {
                Header = "Brez igralca",
                Tag = new Move(new Player("Brez igralca", "", 0, null), courtSide, zone),
                Background = new SolidColorBrush(Colors.OrangeRed),
            };
            menuItem.Click += MenuItem_Click;
            menu.Items.Add(menuItem);

            menu.IsOpen = true;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var move = (Move) ((MenuItem) sender).Tag;
            var menu = new ContextMenu();
            string teamName = _firstTeamSide == move.Side ? _firstTeamName : _secondTeamName;
            menu.Items.Add(new MenuItem
            {
                Header = "Servis - uspeh",
                Tag = new Move(move.Player, Action.ServisSuccess, move.Side, _currentSet, move.Zone, teamName),
                Background = new SolidColorBrush(Colors.LawnGreen)
            });
            menu.Items.Add(new MenuItem
            {
                Header = "Sprejem - uspeh",
                Tag = new Move(move.Player, Action.SprejemSuccess, move.Side, _currentSet, move.Zone, teamName),
                Background = new SolidColorBrush(Colors.LawnGreen)
            });
            menu.Items.Add(new MenuItem
            {
                Header = "Podaja - uspeh",
                Tag = new Move(move.Player, Action.PodajaSuccess, move.Side, _currentSet, move.Zone, teamName),
                Background = new SolidColorBrush(Colors.LawnGreen)
            });
            menu.Items.Add(new MenuItem
            {
                Header = "Napad - uspeh",
                Tag = new Move(move.Player, Action.NapadSuccess, move.Side, _currentSet, move.Zone, teamName),
                Background = new SolidColorBrush(Colors.LawnGreen)
            });
            menu.Items.Add(new MenuItem
            {
                Header = "Blok - uspeh",
                Tag = new Move(move.Player, Action.BlokSuccess, move.Side, _currentSet, move.Zone, teamName),
                Background = new SolidColorBrush(Colors.LawnGreen)
            });
            menu.Items.Add(new MenuItem
            {
                Header = "Servis - napaka",
                Tag = new Move(move.Player, Action.ServisFail, move.Side, _currentSet, move.Zone, teamName),
                Background = new SolidColorBrush(Colors.OrangeRed)
            });
            menu.Items.Add(new MenuItem
            {
                Header = "Sprejem - napaka",
                Tag = new Move(move.Player, Action.SprejemFail, move.Side, _currentSet, move.Zone, teamName),
                Background = new SolidColorBrush(Colors.OrangeRed)
            });
            menu.Items.Add(new MenuItem
            {
                Header = "Podaja - napaka",
                Tag = new Move(move.Player, Action.PodajaFail, move.Side, _currentSet, move.Zone, teamName),
                Background = new SolidColorBrush(Colors.OrangeRed)
            });
            menu.Items.Add(new MenuItem
            {
                Header = "Napad - napaka",
                Tag = new Move(move.Player, Action.NapadFail, move.Side, _currentSet, move.Zone, teamName),
                Background = new SolidColorBrush(Colors.OrangeRed)
            });
            menu.Items.Add(new MenuItem
            {
                Header = "Blok - napaka",
                Tag = new Move(move.Player, Action.BlokFail, move.Side, _currentSet, move.Zone, teamName),
                Background = new SolidColorBrush(Colors.OrangeRed)
            });
            menu.Items.Add(new MenuItem
            {
                Header = "Prestop",
                Tag = new Move(move.Player, Action.Prestop, move.Side, _currentSet, move.Zone, teamName),
                Background = new SolidColorBrush(Colors.OrangeRed)
            });
            menu.Items.Add(new MenuItem
            {
                Header = "Mreža",
                Tag = new Move(move.Player, Action.Mreza, move.Side, _currentSet, move.Zone, teamName),
                Background = new SolidColorBrush(Colors.OrangeRed)
            });
            menu.Items.Add(new MenuItem
            {
                Header = "Druga napaka",
                Tag = new Move(move.Player, Action.Drugo, move.Side, _currentSet, move.Zone, teamName),
                Background = new SolidColorBrush(Colors.OrangeRed),
            });
            menu.Items.Add(new MenuItem
            {
                Header = "Menjava igralca",
                Tag = new Move(move.Player, Action.Menjava, move.Side, _currentSet, move.Zone, teamName),
                Background = new SolidColorBrush(Colors.LightSkyBlue),
            });

            foreach (MenuItem menuItem in menu.Items)
            {
                if (menuItem.Header.ToString() == "Menjava igralca")
                {
                    menuItem.Click += MenuItemSwitch_Click;
                }
                else
                {
                    menuItem.Click += MenuItemAction_Click;
                }
            }

            menu.IsOpen = true;
        }

        private void MenuItemSwitch_Click(object sender, RoutedEventArgs e)
        {
            var move = (Move) ((MenuItem) sender).Tag;
            var players = _firstTeamSide == move.Side
                ? _firstTeamPlayers.Where(x => x.StartingZone == null).OrderBy(x => x.JerseyNumber)
                : _secondTeamPlayers.Where(x => x.StartingZone == null).OrderBy(x => x.JerseyNumber);

            var menu = new ContextMenu();
            MenuItem menuItem;
            foreach (var player in players)
            {
                menuItem = new MenuItem
                {
                    Header = $"{player.JerseyNumber} - {player.Name.First()}. {player.Surname}",
                    Tag = new PlayerSwitch(move,
                        new Move(player, move.Action, move.Side, move.SetNumber, move.Zone, move.TeamName))
                };
                menuItem.Click += PerformSwitch_Click;
                menu.Items.Add(menuItem);
            }

            menu.IsOpen = true;
        }

        private void PerformSwitch_Click(object sender, RoutedEventArgs e)
        {
            var playerSwitch = (PlayerSwitch) ((MenuItem) sender).Tag;
            if (_firstTeamSide == playerSwitch.CourtPlayer.Side)
            {
                _firstTeamPlayers.First(x => x.JerseyNumber == playerSwitch.Reserve.Player.JerseyNumber).StartingZone =
                    playerSwitch.CourtPlayer.Player.StartingZone;
                _firstTeamPlayers.First(x => x.JerseyNumber == playerSwitch.CourtPlayer.Player.JerseyNumber)
                    .StartingZone = null;
                SetZoneNumbers(_firstTeamPlayers, _firstTeamSide);
            }
            else
            {
                _secondTeamPlayers.First(x => x.JerseyNumber == playerSwitch.Reserve.Player.JerseyNumber).StartingZone =
                    playerSwitch.CourtPlayer.Player.StartingZone;
                _secondTeamPlayers.First(x => x.JerseyNumber == playerSwitch.CourtPlayer.Player.JerseyNumber)
                    .StartingZone = null;
                SetZoneNumbers(_secondTeamPlayers, _secondTeamSide);
            }

            MatchMoves.Add(playerSwitch.CourtPlayer);
            MatchMoves.Add(playerSwitch.Reserve);
            MatchMovesListView.Items.Add(new ListViewItem
            {
                Content = playerSwitch.CourtPlayer.ToString(),
                Tag = playerSwitch.CourtPlayer
            });
            MatchMovesListView.Items.Add(new ListViewItem
            {
                Content = playerSwitch.Reserve.ToString(),
                Tag = playerSwitch.Reserve
            });
        }

        private void MenuItemAction_Click(object sender, RoutedEventArgs e)
        {
            var move = (Move) ((MenuItem) sender).Tag;

            MatchMoves.Add(move);

            MatchMovesListView.Items.Add(new ListViewItem {Content = move.ToString(), Tag = move});

            if (move.Action == Action.ServisSuccess || move.Action == Action.ServisFail)
            {
                _currentServeSide = move.Side;
            }

            if (!move.IsSuccess)
            {
                if (move.Side == _firstTeamSide)
                {
                    _secondTeamScore++;
                }
                else
                {
                    _firstTeamScore++;
                }

                if (move.Side == CourtSide.Left)
                {
                    RightTeamLabel.FontWeight = FontWeights.Bold;
                    LeftTeamLabel.FontWeight = FontWeights.Normal;
                }
                else
                {
                    RightTeamLabel.FontWeight = FontWeights.Normal;
                    LeftTeamLabel.FontWeight = FontWeights.Bold;
                }

                ScoreLabel.Content = _firstTeamSide == CourtSide.Left
                    ? $"{_firstTeamScore} - {_secondTeamScore}"
                    : $"{_secondTeamScore} - {_firstTeamScore}";

                if (_firstTeamScore >= 25 && (_firstTeamScore - _secondTeamScore) > 1)
                {
                    _firstTeamSetScore++;
                    EndSet();
                }
                else if (_secondTeamScore >= 25 && (_secondTeamScore - _firstTeamScore) > 1)
                {
                    _secondTeamSetScore++;
                    EndSet();
                }

                if (move.Side == _currentServeSide)
                {
                    if (_firstTeamSide == move.Side)
                    {
                        RotatePlayers(_secondTeamPlayers);
                        SetZoneNumbers(_secondTeamPlayers,
                            move.Side == CourtSide.Left ? CourtSide.Right : CourtSide.Left);
                    }
                    else
                    {
                        RotatePlayers(_firstTeamPlayers);
                        SetZoneNumbers(_firstTeamPlayers,
                            move.Side == CourtSide.Left ? CourtSide.Right : CourtSide.Left);
                    }
                }
            }
        }

        private void EndSet()
        {
            if (_firstTeamSide == CourtSide.Left)
            {
                SetScoreLabel.Content = $"{_firstTeamSetScore} - {_secondTeamSetScore}";
            }
            else
            {
                SetScoreLabel.Content = $"{_secondTeamSetScore} - {_firstTeamSetScore}";
            }

            if (_firstTeamSetScore >= 3 || _secondTeamSetScore >= 3)
            {
                NavigationService.Navigate(new AnalysisPage(new Match(
                    new Team(_firstTeamName, _firstTeamPlayers, _firstTeamSide, _firstTeamSetScore),
                    new Team(_secondTeamName, _secondTeamPlayers, _secondTeamSide, _secondTeamSetScore), MatchMoves)));
                return;
            }

            _firstTeamSide = _firstTeamSide == CourtSide.Left ? CourtSide.Right : CourtSide.Left;
            _secondTeamSide = _secondTeamSide == CourtSide.Left ? CourtSide.Right : CourtSide.Left;
            _firstTeamScore = 0;
            _secondTeamScore = 0;

            _currentSet++;
            SetZoneNumbers(_firstTeamPlayers, _firstTeamSide);
            SetZoneNumbers(_secondTeamPlayers, _secondTeamSide);

            if (_firstTeamSide == CourtSide.Left)
            {
                LeftTeamLabel.Content = _firstTeamName;
                RightTeamLabel.Content = _secondTeamName;
                ScoreLabel.Content = $"{_firstTeamScore} - {_secondTeamScore}";
                SetScoreLabel.Content = $"{_firstTeamSetScore} - {_secondTeamSetScore}";
            }
            else
            {
                LeftTeamLabel.Content = _secondTeamName;
                RightTeamLabel.Content = _firstTeamName;
                ScoreLabel.Content = $"{_secondTeamScore} - {_firstTeamScore}";
                SetScoreLabel.Content = $"{_secondTeamSetScore} - {_firstTeamSetScore}";
            }
        }

        private int GetZone(Point location)
        {
            if (location.X < 400 && location.Y < 255)
            {
                return 5;
            }

            if (location.X < 400 && location.Y < 410)
            {
                return 6;
            }

            if (location.X < 400 && location.Y < 655)
            {
                return 1;
            }

            if (location.X >= 400 && location.X < 527 && location.Y < 255)
            {
                return 4;
            }

            if (location.X >= 400 && location.X < 527 && location.Y < 410)
            {
                return 3;
            }

            if (location.X >= 400 && location.X < 527 && location.Y < 655)
            {
                return 2;
            }

            if (location.X >= 527 && location.X < 655 && location.Y < 255)
            {
                return 2;
            }

            if (location.X >= 527 && location.X < 655 && location.Y < 410)
            {
                return 3;
            }

            if (location.X >= 527 && location.X < 655 && location.Y < 655)
            {
                return 4;
            }

            if (location.X >= 655 && location.Y < 255)
            {
                return 1;
            }

            if (location.X >= 655 && location.Y < 410)
            {
                return 6;
            }

            if (location.X >= 655 && location.Y < 655)
            {
                return 5;
            }

            throw new InvalidOperationException("No zone detected, fix it!");
        }

        private Player GetPlayer(IList<Player> team, int zone)
        {
            return team.First(x => x.StartingZone == zone);
        }

        private CourtSide GetCourtSide(Point location)
        {
            return location.X > 528 ? CourtSide.Right : CourtSide.Left;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var failures = new List<Action>
            {
                Action.BlokFail,
                Action.Drugo,
                Action.Mreza,
                Action.NapadFail,
                Action.PodajaFail,
                Action.SprejemFail
            };
            for (int i = 0; i < 23; i++)
            {
                MenuItemAction_Click(new MenuItem()
                {
                    Tag = new Move(_firstTeamPlayers.Where(x => x.StartingZone != null).PickRandom(),
                        Action.ServisSuccess, _firstTeamSide, _currentSet, new Random().Next(7), _firstTeamName)
                }, null);
                MenuItemAction_Click(new MenuItem()
                {
                    Tag = new Move(_secondTeamPlayers.Where(x => x.StartingZone != null).PickRandom(),
                        Action.SprejemSuccess, _secondTeamSide, _currentSet, new Random().Next(7), _secondTeamName)
                }, null);
                MenuItemAction_Click(new MenuItem()
                {
                    Tag = new Move(_secondTeamPlayers.Where(x => x.StartingZone != null).PickRandom(),
                        Action.PodajaSuccess, _secondTeamSide, _currentSet, new Random().Next(7), _secondTeamName)
                }, null);
                MenuItemAction_Click(new MenuItem()
                {
                    Tag = new Move(_secondTeamPlayers.Where(x => x.StartingZone != null).PickRandom(),
                        Action.NapadSuccess, _secondTeamSide, _currentSet, new Random().Next(7), _secondTeamName)
                }, null);
                MenuItemAction_Click(new MenuItem()
                {
                    Tag = new Move(_firstTeamPlayers.Where(x => x.StartingZone != null).PickRandom(),
                        failures.PickRandom(), _firstTeamSide, _currentSet, new Random().Next(7), _firstTeamName)
                }, null);

                MenuItemAction_Click(new MenuItem()
                {
                    Tag = new Move(_secondTeamPlayers.Where(x => x.StartingZone != null).PickRandom(),
                        Action.ServisSuccess, _secondTeamSide, _currentSet, new Random().Next(7), _secondTeamName)
                }, null);
                MenuItemAction_Click(new MenuItem()
                {
                    Tag = new Move(_firstTeamPlayers.Where(x => x.StartingZone != null).PickRandom(),
                        Action.SprejemSuccess, _firstTeamSide, _currentSet, new Random().Next(7), _firstTeamName)
                }, null);
                MenuItemAction_Click(new MenuItem()
                {
                    Tag = new Move(_firstTeamPlayers.Where(x => x.StartingZone != null).PickRandom(),
                        Action.PodajaSuccess, _firstTeamSide, _currentSet, new Random().Next(7), _firstTeamName)
                }, null);
                MenuItemAction_Click(new MenuItem()
                {
                    Tag = new Move(_firstTeamPlayers.Where(x => x.StartingZone != null).PickRandom(),
                        Action.NapadSuccess, _firstTeamSide, _currentSet, new Random().Next(7), _firstTeamName)
                }, null);
                MenuItemAction_Click(new MenuItem()
                {
                    Tag = new Move(_secondTeamPlayers.Where(x => x.StartingZone != null).PickRandom(),
                        failures.PickRandom(), _secondTeamSide, _currentSet, new Random().Next(7), _secondTeamName)
                }, null);
            }


            //_firstTeamSetScore++;
            //EndSet();
        }
    }
}
