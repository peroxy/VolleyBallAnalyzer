using System;
using System.Collections.Generic;
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
    /// Interaction logic for TeamPage.xaml
    /// </summary>
    public partial class TeamPage : Page
    {
        public List<Player> PlayersFirstTeam { get; set; } = new List<Player>();
        public List<Player> PlayersSecondTeam { get; set; } = new List<Player>();

        private readonly string[] _names = { "Janez", "Nejc", "Matic", "Boris", "Peter", "Boštjan", "Jože", "Marko", "Jan", "Anže", "Aljaž", "Robert", "Tilen", "Žiga", "Klemen"};
        private readonly string[] _surnames = { "Novak", "Jenko", "Drinovec", "Horvat", "Kovačič", "Mlakar", "Vidmar", "Kos", "Oman", "Valjavec", "Kejžar", "Jerina", "Babnik", "Kalan" };

        public TeamPage()
        {
            InitializeComponent();
            FirstTeamDataGrid.ItemsSource = PlayersFirstTeam;
            SecondTeamDataGrid.ItemsSource = PlayersSecondTeam;

            FirstTeamName.Text = "Triglav";
            SecondTeamName.Text = "Jesenice";

            for (int? i = 1; i <= 10; i++)
            {
                PlayersFirstTeam.Add(new Player(_names.PickRandom(), _surnames.PickRandom(), i.Value+10, i < 7 ? i : null));
                PlayersSecondTeam.Add(new Player(_names.PickRandom(), _surnames.PickRandom(), i.Value+20, i < 7 ? i : null));
            }
        }

        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
            if (PlayersFirstTeam.Count < 6 || PlayersSecondTeam.Count < 6)
            {
                MessageBox.Show("Prosim vnesite vsaj 6 igralcev!", "Napaka!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (PlayersFirstTeam.Count(x => x.StartingZone != null) != 6 ||
                PlayersSecondTeam.Count(x => x.StartingZone != null) != 6 || 
                PlayersFirstTeam.Count != PlayersFirstTeam.Distinct().Count() ||
                PlayersSecondTeam.Count != PlayersSecondTeam.Distinct().Count())
            {
                MessageBox.Show("Vsaka ekipa mora imeti natanko 6 unikatnih začetnih pozicij (con)!", "Napaka!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (PlayersFirstTeam.Any(x => x.StartingZone != null && x.StartingZone < 1 || x.StartingZone > 6) || PlayersSecondTeam.Any(x => x.StartingZone != null && x.StartingZone < 1 || x.StartingZone > 6))
            {
                MessageBox.Show("Začetne pozicije (cone) igralcev so lahko samo vrednosti od 1 do 6!", "Napaka!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(FirstTeamName.Text) || string.IsNullOrWhiteSpace(SecondTeamName.Text))
            {
                MessageBox.Show("Prosim vnesite ime ekipe!", "Napaka!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            NavigationService.Navigate(new GamePage(FirstTeamName.Text, PlayersFirstTeam, SecondTeamName.Text, PlayersSecondTeam));
        }

        private void ExitButtonClick(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
