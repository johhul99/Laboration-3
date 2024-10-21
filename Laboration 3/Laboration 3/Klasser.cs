using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboration_3
{
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
