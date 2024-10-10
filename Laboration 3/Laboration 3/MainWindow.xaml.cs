using System.Collections.ObjectModel;
using System.ComponentModel;
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
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private Pass _selectedPass;
        public Pass SelectedPass
        {
            get { return _selectedPass; }
            set
            {
                _selectedPass = value;
                OnPropertyChanged(nameof(SelectedPass));
            }
        }

        private Pass _selectedPass2;
        public Pass SelectedPass2
        {
            get { return _selectedPass2; }
            set
            {
                _selectedPass2 = value;
                OnPropertyChanged(nameof(SelectedPass2));
            }
        }

        private string _selectedPassTyp;
        public string SelectedPassTyp
        {
            get { return _selectedPassTyp; }
            set
            {
                _selectedPassTyp = value;
                OnPropertyChanged(nameof(SelectedPassTyp));
            }
        }

        private TimeSpan _selectedPassTid;
        public TimeSpan SelectedPassTid
        {
            get { return _selectedPassTid; }
            set
            {
                _selectedPassTid = value;
                OnPropertyChanged(nameof(SelectedPassTid));
            }
        }

        private TimeSpan _selectedPassTid2;
        public TimeSpan SelectedPassTid2
        {
            get { return _selectedPassTid2; }
            set
            {
                _selectedPassTid2 = value;
                OnPropertyChanged(nameof(SelectedPassTid2));
            }
        }

        private Användare _selectedAnvändare;
        public Användare SelectedAnvändare
        {
            get { return _selectedAnvändare; }
            set
            {
                _selectedAnvändare = value;
                OnPropertyChanged(nameof(SelectedAnvändare));
                BokadePass1 = _selectedAnvändare?.BokadePass ?? new ObservableCollection<Pass>();
            }


        }

        private ObservableCollection<Pass> _bokadePass1;
        public ObservableCollection<Pass> BokadePass1
        {
            get { return _bokadePass1; }
            set
            {
                _bokadePass1 = value;
                OnPropertyChanged(nameof(BokadePass1));
            }
        }

        private ObservableCollection<Pass> _passLista;
        public ObservableCollection<Pass> PassLista 
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
            PassLista = new ObservableCollection<Pass>
            {
                { new Pass( "Yoga A", "Flexibilitet", TimeSpan.Parse("17:00"), 18) },
                { new Pass( "Yoga B", "Flexibilitet", TimeSpan.Parse("18:15"), 14) },
                { new Pass( "Spinning A", "Kondition", TimeSpan.Parse("17:00"), 22) },
                { new Pass( "Spinning B", "Kondition", TimeSpan.Parse("18:30"), 26) },
                { new Pass( "Zumba A", "Dans", TimeSpan.Parse("18:15"), 20) },
                { new Pass( "Zumba B", "Dans", TimeSpan.Parse("17:30"), 18) }
            };

            OriginalPassLista = PassLista;

            AnvändarLista = new ObservableCollection<Användare>
            {
                { new Användare("Användare1") },
                { new Användare("Användare2") }
            };

            SelectedAnvändare = AnvändarLista.FirstOrDefault();

            BokadePass1 = SelectedAnvändare.BokadePass;

            PassTyper = PassLista.Select(p => p.PassTyp).Distinct().ToList();
            PassTyper.Add("Alla");

            PassTider = PassLista.Select(p => p.Tid).OrderBy(p => p).Distinct().ToList();
            PassTider.Insert(0, TimeSpan.Parse("00:01"));

            PassTider2 = PassLista.Select(p => p.Tid).OrderBy(p => p).Distinct().ToList();
            PassTider2.Add(TimeSpan.Parse("23:59"));

            SelectedPass = PassLista.FirstOrDefault();

            SelectedPass2 = BokadePass1.FirstOrDefault();

            SelectedPassTid = PassTider.FirstOrDefault();

            SelectedPassTid2 = PassTider2[PassTider2.Count - 1];

            SelectedPassTyp = PassTyper[PassTyper.Count - 1];

            BH = new BokningsHantering();

            this.DataContext = this;
        }

        private ObservableCollection<Pass> OriginalPassLista { get; set; }
        public BokningsHantering BH { get; set; }
        public List<string> PassTyper { get; set; }
        public List<TimeSpan> PassTider { get; set; }
        public List<TimeSpan> PassTider2 { get; set; }
        public ObservableCollection<Användare> AnvändarLista { get; set; }

        public void FiltreraPass_Click(object sender, EventArgs e)
        {
            PassLista = BH.FiltreraPass(OriginalPassLista, SelectedPassTid, SelectedPassTid2, SelectedPassTyp);
        }

        public void ResetPassLista_MouseDown(object sender, MouseEventArgs e)
        {
            PassLista = OriginalPassLista;
            PassTyper = PassLista.Select(p => p.PassTyp).Distinct().ToList();
            PassTyper.Add("Alla");
            PassTider = PassLista.Select(p => p.Tid).OrderBy(p => p).Distinct().ToList();
            PassTider.Insert(0, TimeSpan.Parse("00:01"));
            PassTider2 = PassLista.Select(p => p.Tid).OrderBy(p => p).Distinct().ToList();
            PassTider2.Add(TimeSpan.Parse("23:59"));
            SelectedPass = PassLista.FirstOrDefault();
            SelectedPassTid = PassTider.FirstOrDefault();
            SelectedPassTid2 = PassTider2[PassTider2.Count - 1];
            SelectedPassTyp = PassTyper[PassTyper.Count - 1];
        }
        public void BokaPass_Click(object sender, EventArgs e)
        {
            if (SelectedAnvändare.BokadePass.Contains(SelectedPass))
            {
                MessageBox.Show($"Du är redan inbokad på {SelectedPass.Namn}");
            }
            else
            {
                SelectedPass.AntalPlatser = BH.SubtraheraPlats(SelectedPass);
                SelectedAnvändare.BokadePass = BH.BokaPass(SelectedAnvändare, SelectedPass);
                MessageBox.Show($"Du har nu bokat {SelectedPass.Namn}. Välkommen in kl{SelectedPass.Tid}");
            }
        }

        public void AvbokaPass_Click(object sender, EventArgs e)
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

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class BokningsHantering
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

    public class Pass : INotifyPropertyChanged
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
    public class Användare : INotifyPropertyChanged
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