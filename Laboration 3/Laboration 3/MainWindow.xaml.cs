using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Media;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Laboration_3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged //Hantera förändringar av värdet på fält.
    {
        private Pass _selectedPass; //Så att användarens val av pass sparas i bokningsfliken.
        public Pass SelectedPass //Så att användarens val av pass ändras och hämtas i bokningsfliken.
        {
            get { return _selectedPass; }
            set
            {
                _selectedPass = value;
                OnPropertyChanged(nameof(SelectedPass));
            }
        }

        private Pass _selectedPass2; //Så att användarens val av pass sparas i avbokningsfliken.
        public Pass SelectedPass2 //Så att användarens val av pass ändras och hämtas i avbokningsfliken.
        {
            get { return _selectedPass2; }
            set
            {
                _selectedPass2 = value;
                OnPropertyChanged(nameof(SelectedPass2));
            }
        }

        private string _selectedPassTyp; //Så att användarens val av passtyp sparas i bokningsfliken.
        public string SelectedPassTyp //Så att användarens val av passtyp ändras och hämtas i bokningsfliken.
        {
            get { return _selectedPassTyp; }
            set
            {
                _selectedPassTyp = value;
                OnPropertyChanged(nameof(SelectedPassTyp));
            }
        }

        private TimeSpan _selectedPassTid; //Så att användarens val av tidigaste tid för pass start sparas i bokningsfliken.
        public TimeSpan SelectedPassTid //Så att användarens val av tidigaste tid för pass start ändras och hämtas i bokningsfliken.
        {
            get { return _selectedPassTid; }
            set
            {
                _selectedPassTid = value;
                OnPropertyChanged(nameof(SelectedPassTid));
            }
        }

        private TimeSpan _selectedPassTid2; //Så att användarens val av senaste tid för pass start sparas i bokningsfliken.
        public TimeSpan SelectedPassTid2 //Så att användarens val av senaste tid för pass start ändras och hämtas i bokningsfliken.
        {
            get { return _selectedPassTid2; }
            set
            {
                _selectedPassTid2 = value;
                OnPropertyChanged(nameof(SelectedPassTid2));
            }
        }

        private Användare _selectedAnvändare; //Så att vald användare sparas.
        public Användare SelectedAnvändare //Så att vald användare ändras och hämtas.
        {
            get { return _selectedAnvändare; }
            set
            {
                _selectedAnvändare = value;
                OnPropertyChanged(nameof(SelectedAnvändare));
                BokadePass1 = _selectedAnvändare?.BokadePass ?? new ObservableCollection<Pass>(); //Så att listan i avbokningsfönstret uppdateras till aktuell användares bokade pass.
            }


        }

        private ObservableCollection<Pass> _bokadePass1; //Så att användarens bokade pass sparas.
        public ObservableCollection<Pass> BokadePass1 //Så att användarens bokade pass ändras och hämtas.
        {
            get { return _bokadePass1; }
            set
            {
                _bokadePass1 = value;
                OnPropertyChanged(nameof(BokadePass1));
            }
        }

        private ObservableCollection<Pass> _passLista; //Så att alla pass sparas.
        public ObservableCollection<Pass> PassLista //Så att listan över alla pass ändras och hämtas vid filtrering.
        {
            get { return _passLista; } 
            set
            {
                _passLista = value;
                OnPropertyChanged(nameof(PassLista));
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            PassLista = new ObservableCollection<Pass> //Fyller PassLista med tillgänliga pass.
            {
                { new Pass( "Yoga A", "Flexibilitet", TimeSpan.Parse("17:00"), 18) },
                { new Pass( "Yoga B", "Flexibilitet", TimeSpan.Parse("18:15"), 14) },
                { new Pass( "Spinning A", "Kondition", TimeSpan.Parse("17:15"), 22) },
                { new Pass( "Spinning B", "Kondition", TimeSpan.Parse("18:30"), 26) },
                { new Pass( "Zumba A", "Dans", TimeSpan.Parse("18:15"), 20) },
                { new Pass( "Zumba B", "Dans", TimeSpan.Parse("17:30"), 18) },
                { new Pass( "Push", "Styrka", TimeSpan.Parse("17:00"), 2) },
                { new Pass( "Pull", "Styrka", TimeSpan.Parse("18:00"), 2) },
                { new Pass( "Legs", "Styrka", TimeSpan.Parse("17:30"), 2) }
            };

            OriginalPassLista = PassLista; //När filtrering resetar vid byte av användare så ska den orignella listan finnas tillgänlig att hämta.

            AnvändarLista = new List<Användare> //Fyller AnvändarLista med tillgänliga användare.
            {
                { new Användare("Användare1") },
                { new Användare("Användare2") },
                { new Användare("Användare3") }
            };

            SelectedAnvändare = AnvändarLista.FirstOrDefault(); //Sätter vald användare till den första i listan.

            BokadePass1 = SelectedAnvändare.BokadePass; //Sätter listan av pass i avbokningsfliken till användarens bokade pass
                                                        //vilket nu blir tom men hade man exempelvis hämtat från en databas hade det varit rätt lista från början.

            PassTyper = PassLista.Select(p => p.PassTyp).Distinct().ToList(); //Så filtreringsalternativen för passtyp är en av varje existerande.
            PassTyper.Add("Alla"); //Så att alternativet finns om användaren vill se alla passtyper.

            PassTider = PassLista.Select(p => p.Tid).OrderBy(p => p).Distinct().ToList(); //Så filtreringsalternativen för tidigaste starttid är en av varje existerande.
            PassTider.Insert(0, TimeSpan.Parse("00:01")); //Så att alternativet finns om användaren inte vill filtrera på tidigaste starttid. 
                                                          //Alternativet sätts in sist för att ha en fortsatt sorterad lista.

            PassTider2 = PassLista.Select(p => p.Tid).OrderBy(p => p).Distinct().ToList(); //Så filtreringsalternativen för senaste starttid är en av varje existerande.
            PassTider2.Add(TimeSpan.Parse("23:59")); //Så att alternativet finns om användaren inte vill filtrera på senaste starttid.

            SelectedPass = PassLista.FirstOrDefault(); //Sätter valda pass till det första i pass listan i bokningsfliken

            SelectedPass2 = BokadePass1.FirstOrDefault(); //Sätter valda pass till det första i pass listan i avbokningsfliken
                                                          //vilket nu blir tom men hade man exempelvis hämtat från en databas hade det varit rätt lista från början.

            SelectedPassTid = PassTider.FirstOrDefault(); //Sätter tidigaste start tid i filtreringen i bokningsfliken till 00:01.

            SelectedPassTid2 = PassTider2[PassTider2.Count - 1]; //Sätter senaste start tid i filtreringen i bokningsfliken  till 23:59.

            SelectedPassTyp = PassTyper[PassTyper.Count - 1]; //Sätter passtyp vid filtrering i bokningsfliken till "Alla".

            BH = new BokningsHantering(); //Så det går att kalla på boknings- och filtreringsmetoder.

            this.DataContext = this; //Så att programmet använder denna code behind.
        }

        private ObservableCollection<Pass> OriginalPassLista { get; set; } //Så att pass listan innan filtrering sparas.
        private BokningsHantering BH { get; set; } //För att kunna kalla på filtrerings och boknings/avbokningsmetoder.
        public List<string> PassTyper { get; set; } //Denna och nedre två variabler för att spara listorna vid filtrering.
        public List<TimeSpan> PassTider { get; set; }
        public List<TimeSpan> PassTider2 { get; set; }
        public List<Användare> AnvändarLista { get; set; } //För att spara användar listan.

        public void FiltreraPass_Click(object sender, EventArgs e) //För att filtrera pass med logik från BokningsHanteringsklassen vid klick av filtreringsknappen.
        {
            PassLista = BH.FiltreraPass(OriginalPassLista, SelectedPassTid, SelectedPassTid2, SelectedPassTyp);
        }

        public void ResetPassLista_MouseDown(object sender, MouseEventArgs e) //För att ställa om filtreringen och pass listan i bokningsfliken till original vid byte av användare.
        {
            PassLista = OriginalPassLista;            
            SelectedPass = PassLista.FirstOrDefault();
            SelectedPassTid = PassTider.FirstOrDefault();
            SelectedPassTid2 = PassTider2[PassTider2.Count - 1];
            SelectedPassTyp = PassTyper[PassTyper.Count - 1];
        }
        public async void BokaPass_Click(object sender, RoutedEventArgs r) //För att användare ska kunna boka pass med logik från BokningsHantering vid klick av bokningsknappen
                                                                           //men även hantering ifall användare försöker boka in sig på ett fullbokat pass eller ett de redan bokat.
                                                                           
        {
            if (SelectedAnvändare.BokadePass.Contains(SelectedPass))
            {
                MessageBox.Show($"Du är redan inbokad på {SelectedPass.Namn}");
            }
            else if (SelectedPass.AntalPlatser == 0)
            {
                MessageBox.Show("Detta passet är fullbokat!");
            }
            else
            {
                Image img = new Image();  //Koden fram till nästa kommentar är den roliga delen av passbokningen vid val av styrkepass.
                if (SelectedPass.Namn == "Push") //Så img har en bildfil i sig vid val av ett av tre styrkepass.
                {
                    img.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Push1.PNG"));
                }
                else if (SelectedPass.Namn == "Pull")
                {
                    img.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Pull.PNG"));
                }
                else if (SelectedPass.Namn == "Legs")
                {
                    img.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Legs.PNG"));
                }     
                
                if(SelectedPass.Namn == "Push" || SelectedPass.Namn == "Pull" || SelectedPass.Namn == "Legs") //Så img läggs till i gridet Picture efter att ett av dessa tre passen valts.
                {
                    Picture.Children.Add(img);
                }

                await Task.Delay(50); // Så bilden läggs till innan koden fortsätter.

                if (Picture.Children.Contains(img)) //Så att ljudfilen spelas endast om bild finns i gridet.
                {
                    SoundPlayer player = new SoundPlayer(Application.GetResourceStream(new Uri("pack://application:,,,/Resources/Tevvez.WAV")).Stream); //Så det finns en ljudfil att spela.
                    player.PlaySync(); //Så att ljudfilen spelar klart innan koden fortsätter.
                } //Slutkommentar.

                SelectedPass.AntalPlatser = BH.SubtraheraPlats(SelectedPass); //Så antal platser uppdateras korrekt.
                SelectedAnvändare.BokadePass = BH.BokaPass(SelectedAnvändare, SelectedPass); //Så att passet bokas av användaren.
                MessageBox.Show($"Du har nu bokat {SelectedPass.Namn}. Välkommen in kl {SelectedPass.Tid}!");
                if (Picture.Children.Contains(img)) //Tar bort bilden från skärmen.
                {
                    Picture.Children.Remove(img);
                } 

            }
        }

        public void AvbokaPass_Click(object sender, EventArgs e) //Så användare kan avboka pass med logik från BokningsHanteringsklassen samt hantering
                                                                 //ifall listan är tom eller inget pass är markerat.
        {
            if(!BokadePass1.Any())
            {
                MessageBox.Show("Du har inget pass att avboka.");
            }
            else if(SelectedPass2 == null)
            {
                MessageBox.Show("Du måste markera ett pass för att kunna avboka.");
            }
            else
            {
                SelectedPass2.AntalPlatser = BH.AdderaPlats(SelectedPass2);
                MessageBox.Show($"Du har nu avbokat {SelectedPass2.Namn}.");
                SelectedAnvändare.BokadePass = BH.AvbokaPass(SelectedAnvändare, SelectedPass2);
            }                     
        }


        public event PropertyChangedEventHandler PropertyChanged;  //Så att fält uppdateras korrekt vid förändring.
        protected void OnPropertyChanged(string propertyName) //Så att fält uppdateras korrekt vid förändring.
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class BokningsHantering //Logik för filtrering, bokning och avbokning.
    {
        public ObservableCollection<Pass> BokaPass(Användare A, Pass P)
        {

            A.BokadePass.Add(P);
            return A.BokadePass;
        }

        public ObservableCollection<Pass> AvbokaPass(Användare A, Pass P)
        {
            A.BokadePass.Remove(P);
            return A.BokadePass;
        }

        public ObservableCollection<Pass> FiltreraPass(ObservableCollection<Pass> P, TimeSpan tS, TimeSpan tS2, String T)
        {
            ObservableCollection<Pass> Result;
            if (T == "Alla")
            {
                Result = new ObservableCollection<Pass>(P.Where(p => p.Tid >= tS && p.Tid <= tS2));
            }
            else
            {
                Result = new ObservableCollection<Pass>(P.Where(p => p.Tid >= tS && p.Tid <= tS2 && p.PassTyp == T));
            }          
            return Result;
        }
        public int SubtraheraPlats(Pass P)
        {
            P.AntalPlatser--;
            return P.AntalPlatser;
        }

        public int AdderaPlats(Pass P)
        {
                P.AntalPlatser++;
                return P.AntalPlatser;                       
        }
    }

    public class Pass : INotifyPropertyChanged //Klass för pass med INotifyPropertyChanged för att hantera när antal platser förändras vid bokning och avbokning.
    {
        public string Namn { get; private set; }
        public string PassTyp { get; private set; }
        public TimeSpan Tid { get; private set; }

        private int _antalPlatser;
        public int AntalPlatser
        {
            get { return _antalPlatser; }
            set
            {
                if (_antalPlatser != value)
                {
                    _antalPlatser = value;
                    OnPropertyChanged(nameof(AntalPlatser));
                }
            }
        }

        public Pass(string namn, string passTyp, TimeSpan tid, int antalPlatser)
        {
            Namn = namn;
            PassTyp = passTyp;
            Tid = tid;
            AntalPlatser = antalPlatser;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }

    public class Användare : INotifyPropertyChanged //Klass för användare med INotifyPropertyChanged för att hantera när BokadePass listan förändras vid bokning och avbokning.
    {
        public string Namn { get; private set; }
        public ObservableCollection<Pass> BokadePass = new ObservableCollection<Pass>();

        public Användare(string namn)
        {
            Namn = namn;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}