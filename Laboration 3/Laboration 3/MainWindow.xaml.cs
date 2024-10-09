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
        private string _selectedPassTyp;
        private string _selectedPassTid;
        private string _selectedPassTid2;
        private Användare _selectedAnvändare;

        public MainWindow()
        {
            InitializeComponent();
            PassLista = new List<Pass>
            {
                { new Pass( "Yoga A", "Flexibilitet", "17:00", 18) },
                { new Pass( "Yoga B", "Flexibilitet", "18:15", 14) },
                { new Pass( "Spinning A", "Kondition", "17:00", 22) },
                { new Pass( "Spinning B", "Kondition", "18:30", 26) },
                { new Pass( "Zumba A", "Dans", "18:15", 20) },
                { new Pass( "Zumba B", "Dans", "17:30", 18) }
            };

            AnvändarLista = new List<Användare>
            {
                { new Användare("Användare1") },
                { new Användare("Användare2") }
            };

            SelectedAnvändare = AnvändarLista.FirstOrDefault();

            BokadePass1 = SelectedAnvändare.BokadePass;

            PassTyper = PassLista.Select(p => p.PassTyp).Distinct().ToList();

            PassTider = PassLista.Select(p => p.Tid).OrderBy(p => p).Distinct().ToList();                              

            this.DataContext = this;
        }

        public List<Pass> PassLista { get; set; }

        public List<Pass> BokadePass1 { get; set; }

        public List<string> PassTyper { get; set; }

        public List<string> PassTider { get; set; }

        public List<Användare> AnvändarLista { get; set; }

        public Användare SelectedAnvändare
        {
            get { return _selectedAnvändare; }
            set
            {
                    _selectedAnvändare = value;
                    OnPropertyChanged(nameof(SelectedAnvändare));            
            }


        }

        public string SelectedPassTyp
        {
            get { return _selectedPassTyp; }
            set 
            {
                _selectedPassTyp = value;
                OnPropertyChanged(nameof(SelectedPassTyp));
            }
        }
        public string SelectedPassTid
        {
            get { return _selectedPassTid; }
            set
            {
                _selectedPassTid = value;
                OnPropertyChanged(nameof(SelectedPassTid));
            }
        }

        public string SelectedPassTid2
        {
            get { return _selectedPassTid2; }
            set
            {
                _selectedPassTid2 = value;
                OnPropertyChanged(nameof(SelectedPassTid2));
            }
        }

        public Pass SelectedPass
        {
            get { return _selectedPass; }
            set
            {
                _selectedPass = value;
                OnPropertyChanged(nameof(SelectedPass));
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

    }

    public class Pass : INotifyPropertyChanged
    {
        public string Namn { get; private set; }
        public string PassTyp { get; private set; }
        public string Tid { get; private set; }
        public int AntalPlatser { get; set; }

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
        public List<Pass> BokadePass = new List<Pass>();

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