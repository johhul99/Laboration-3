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
        private Pass _pass;
        private Användare _anv;
        private BokningsHantering _bh;

        public Pass pass
        {
            get { return _pass; }
            set
            {
                _pass = value;
                OnPropertyChanged(nameof(pass));
            }
        }

        public Användare Anv
        {
            get { return _anv; }
            set
            {
                _anv = value;
                OnPropertyChanged(nameof(Anv));
            }
        }

        public BokningsHantering BH
        {
            get { return _bh; }
            set
            {
                _bh = value;
                OnPropertyChanged(nameof(BH));
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            List<Pass> PassLista = new List<Pass>
            {
                { new Pass( "Yoga A", "Flexibilitet", "18:00", 18) },
                { new Pass( "Yoga B", "Flexibilitet", "19:00", 14) },
                { new Pass( "Spinning A", "Kondition", "17:15", 22) },
                { new Pass( "Spinning B", "Kondition", "19:30", 26) },
                { new Pass( "Zumba A", "Dans", "17:45", 20) },
                { new Pass( "Zumba B", "Dans", "18:00", 18) }
            };

            this.DataContext = this;
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

    public class Pass
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

    }
    public class Användare
    {
        public string Namn { get; private set; }
        public List<Pass> BokadePass = new List<Pass>();

        public Användare(string namn)
        {
            Namn = namn;
        }
    }
}