using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
    /// Interaction logic for AnalysisPage.xaml
    /// </summary>
    public partial class AnalysisPage : Page
    {
        private readonly Match _match;

        public AnalysisPage(Match match)
        {
            _match = match;
            InitializeComponent();

            LeftTeamNameLabel.Content = match.FirstTeam.Name;
            RightTeamNameLabel.Content = match.SecondTeam.Name;

            LeftTeamAnalysisListView.Items.Add($"Število doseženih setov: {match.FirstTeam.SetsWon}");
            LeftTeamAnalysisListView.Items.Add($"Število doseženih točk: {match.MatchMoves.Count(x => x.TeamName == match.SecondTeam.Name && !x.IsSuccess)}");
            LeftTeamAnalysisListView.Items.Add($"Število napak: {match.MatchMoves.Count(x => x.TeamName == match.FirstTeam.Name && !x.IsSuccess)}");
            LeftTeamAnalysisListView.Items.Add($"Število uspešnih potez: {match.MatchMoves.Count(x => x.TeamName == match.FirstTeam.Name && x.IsSuccess)}");
            LeftTeamAnalysisListView.Items.Add($"Število uspešnih napadov: {match.MatchMoves.Count(x => x.TeamName == match.FirstTeam.Name && x.Action == Action.NapadSuccess)}");
            LeftTeamAnalysisListView.Items.Add($"Število neuspešnih napadov: {match.MatchMoves.Count(x => x.TeamName == match.FirstTeam.Name && x.Action == Action.NapadFail)}");
            LeftTeamAnalysisListView.Items.Add($"Število uspešnih podaj: {match.MatchMoves.Count(x => x.TeamName == match.FirstTeam.Name && x.Action == Action.PodajaSuccess)}");
            LeftTeamAnalysisListView.Items.Add($"Število neuspešnih podaj: {match.MatchMoves.Count(x => x.TeamName == match.FirstTeam.Name && x.Action == Action.PodajaFail)}");
            LeftTeamAnalysisListView.Items.Add($"Število uspešnih sprejemov: {match.MatchMoves.Count(x => x.TeamName == match.FirstTeam.Name && x.Action == Action.SprejemSuccess)}");
            LeftTeamAnalysisListView.Items.Add($"Število neuspešnih sprejemov: {match.MatchMoves.Count(x => x.TeamName == match.FirstTeam.Name && x.Action == Action.SprejemFail)}");
            LeftTeamAnalysisListView.Items.Add($"Število uspešnih servisov: {match.MatchMoves.Count(x => x.TeamName == match.FirstTeam.Name && x.Action == Action.ServisSuccess)}");
            LeftTeamAnalysisListView.Items.Add($"Število neuspešnih servisov: {match.MatchMoves.Count(x => x.TeamName == match.FirstTeam.Name && x.Action == Action.ServisFail)}");
            LeftTeamAnalysisListView.Items.Add($"Število uspešnih blokov: {match.MatchMoves.Count(x => x.TeamName == match.FirstTeam.Name && x.Action == Action.BlokSuccess)}");
            LeftTeamAnalysisListView.Items.Add($"Število neuspešnih blokov: {match.MatchMoves.Count(x => x.TeamName == match.FirstTeam.Name && x.Action == Action.BlokFail)}");
            LeftTeamAnalysisListView.Items.Add($"Število menjav: {match.MatchMoves.Count(x => x.TeamName == match.FirstTeam.Name && x.Action == Action.Menjava)}");
            LeftTeamAnalysisListView.Items.Add($"Število ostalih napak: {match.MatchMoves.Count(x => x.TeamName == match.FirstTeam.Name && x.Action == Action.Drugo || x.Action == Action.Mreza || x.Action == Action.Prestop)}");

            RightTeamAnalysisListView.Items.Add($"Število doseženih setov: {match.SecondTeam.SetsWon}");
            RightTeamAnalysisListView.Items.Add($"Število doseženih točk: {match.MatchMoves.Count(x => x.TeamName == match.FirstTeam.Name && !x.IsSuccess)}");
            RightTeamAnalysisListView.Items.Add($"Število napak: {match.MatchMoves.Count(x => x.TeamName == match.SecondTeam.Name && !x.IsSuccess)}");
            RightTeamAnalysisListView.Items.Add($"Število uspešnih potez: {match.MatchMoves.Count(x => x.TeamName == match.SecondTeam.Name && x.IsSuccess)}");
            RightTeamAnalysisListView.Items.Add($"Število uspešnih napadov: {match.MatchMoves.Count(x => x.TeamName == match.SecondTeam.Name && x.Action == Action.NapadSuccess)}");
            RightTeamAnalysisListView.Items.Add($"Število neuspešnih napadov: {match.MatchMoves.Count(x => x.TeamName == match.SecondTeam.Name && x.Action == Action.NapadFail)}");
            RightTeamAnalysisListView.Items.Add($"Število uspešnih podaj: {match.MatchMoves.Count(x => x.TeamName == match.SecondTeam.Name && x.Action == Action.PodajaSuccess)}");
            RightTeamAnalysisListView.Items.Add($"Število neuspešnih podaj: {match.MatchMoves.Count(x => x.TeamName == match.SecondTeam.Name && x.Action == Action.PodajaFail)}");
            RightTeamAnalysisListView.Items.Add($"Število uspešnih sprejemov: {match.MatchMoves.Count(x => x.TeamName == match.SecondTeam.Name && x.Action == Action.SprejemSuccess)}");
            RightTeamAnalysisListView.Items.Add($"Število neuspešnih sprejemov: {match.MatchMoves.Count(x => x.TeamName == match.SecondTeam.Name && x.Action == Action.SprejemFail)}");
            RightTeamAnalysisListView.Items.Add($"Število uspešnih servisov: {match.MatchMoves.Count(x => x.TeamName == match.SecondTeam.Name && x.Action == Action.ServisSuccess)}");
            RightTeamAnalysisListView.Items.Add($"Število neuspešnih servisov: {match.MatchMoves.Count(x => x.TeamName == match.SecondTeam.Name && x.Action == Action.ServisFail)}");
            RightTeamAnalysisListView.Items.Add($"Število uspešnih blokov: {match.MatchMoves.Count(x => x.TeamName == match.SecondTeam.Name && x.Action == Action.BlokSuccess)}");
            RightTeamAnalysisListView.Items.Add($"Število neuspešnih blokov: {match.MatchMoves.Count(x => x.TeamName == match.SecondTeam.Name && x.Action == Action.BlokFail)}");
            RightTeamAnalysisListView.Items.Add($"Število menjav: {match.MatchMoves.Count(x => x.TeamName == match.SecondTeam.Name && x.Action == Action.Menjava)}");
            RightTeamAnalysisListView.Items.Add($"Število ostalih napak: {match.MatchMoves.Count(x => x.TeamName == match.SecondTeam.Name && x.Action == Action.Drugo || x.Action == Action.Mreza || x.Action == Action.Prestop)}");
        }

        private void ButtonNewGameClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new TeamPage());
        }

        private void ButtonExportClick(object sender, RoutedEventArgs e)
        {
            var sb = new StringBuilder();
            sb.AppendLine("TeamName;PlayerNumber;PlayerName;PlayerSurname;CourtSide;MoveType;SetNumber;Zone;");
            foreach (var move in _match.MatchMoves)
            {
                sb.AppendLine($"{move.TeamName};{move.Player.JerseyNumber};{move.Player.Name};{move.Player.Surname};{move.Side};{move.Action};{move.SetNumber};{move.Zone};");
            }

            string fileName = $"{System.IO.Path.GetTempPath()}{Guid.NewGuid()}.csv";
            File.WriteAllText(fileName, sb.ToString());
            Process.Start(fileName);
        }
    }
}
