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

        private string _selectedPassTid;
        public string SelectedPassTid
        {
            get { return _selectedPassTid; }
            set
            {
                _selectedPassTid = value;
                OnPropertyChanged(nameof(SelectedPassTid));
            }
        }

        private string _selectedPassTid2;
        public string SelectedPassTid2
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

        public MainWindow()
        {
            InitializeComponent();
            PassLista = new ObservableCollection<Pass>
            {
                { new Pass( "Yoga A", "Flexibilitet", "17:00", 18) },
                { new Pass( "Yoga B", "Flexibilitet", "18:15", 14) },
                { new Pass( "Spinning A", "Kondition", "17:00", 22) },
                { new Pass( "Spinning B", "Kondition", "18:30", 26) },
                { new Pass( "Zumba A", "Dans", "18:15", 20) },
                { new Pass( "Zumba B", "Dans", "17:30", 18) }
            };

            AnvändarLista = new ObservableCollection<Användare>
            {
                { new Användare("Användare1") },
                { new Användare("Användare2") }
            };

            SelectedAnvändare = AnvändarLista.FirstOrDefault();

            BokadePass1 = SelectedAnvändare.BokadePass;

            PassTyper = PassLista.Select(p => p.PassTyp).Distinct().ToList();

            PassTider = PassLista.Select(p => p.Tid).OrderBy(p => p).Distinct().ToList();

            SelectedPass = PassLista.FirstOrDefault();

            SelectedPass2 = BokadePass1.FirstOrDefault();

            SelectedPassTid = PassTider.FirstOrDefault();

            SelectedPassTid2 = PassTider.FirstOrDefault();

            SelectedPassTyp = PassTyper.FirstOrDefault();

            BH = new BokningsHantering();

            this.DataContext = this;
        }

        public BokningsHantering BH { get; set; }
        public ObservableCollection<Pass> PassLista { get; set; }
        public List<string> PassTyper { get; set; }

        public List<string> PassTider { get; set; }

        public ObservableCollection<Användare> AnvändarLista { get; set; }

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
        public string Tid { get; private set; }

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

        public Pass(string namn, string passTyp, string tid, int antalPlatser)
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