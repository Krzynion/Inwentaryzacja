using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inwentaryzacja
{
    public class SpisInwentaryzacyjny
    {
        public static List<SpisInwentaryzacyjnyPozycja> ZliczWgSymbolu(List<SpisInwentaryzacyjnyPozycja>SpisInwentaryzacyjny)
        {
            List<SpisInwentaryzacyjnyPozycja> SpisInwentLkopia = SpisInwentaryzacyjny.ToList();
            for (int x = 0; x < SpisInwentLkopia.Count; x++)
            {
                SpisInwentLkopia[x] = SpisInwentLkopia[x].Kopia();
            }
            for(int z=0;z<SpisInwentLkopia.Count;z++)
            {
                for(int x=z+1;x<SpisInwentLkopia.Count;x++)
                {
                    if(SpisInwentLkopia[z].Symbol==SpisInwentLkopia[x].Symbol)
                    {
                        SpisInwentLkopia[z].Ilosc++;
                        SpisInwentLkopia.RemoveAt(x);
                        x--;
                    }
                }
            }
            return SpisInwentLkopia;
        }
    }

    public class SpisInwentaryzacyjnyPozycja
    {
        private string symbol;
        private string nazwa;
        private int ilosc;
        private float cena;
        
        public SpisInwentaryzacyjnyPozycja()
        {
            symbol = "";
            nazwa = "";
            ilosc = 1;
        }
        public SpisInwentaryzacyjnyPozycja(string Symbol)
        {
            symbol = Symbol;
            nazwa = "";
            ilosc = 1;
        }
        public SpisInwentaryzacyjnyPozycja(string Symbol, string Nazwa)
        {
            symbol = Symbol;
            nazwa = Nazwa;
            ilosc = 1;
        }
        public SpisInwentaryzacyjnyPozycja(string Symbol, string Nazwa, int Ilosc)
        {
            symbol = Symbol;
            nazwa = Nazwa;
            ilosc = Ilosc;
        }
        public SpisInwentaryzacyjnyPozycja(string Symbol, string Nazwa, int Ilosc, float Cena)
        {
            symbol = Symbol;
            nazwa = Nazwa;
            ilosc = Ilosc;
            cena = Cena;
        }
        public SpisInwentaryzacyjnyPozycja(string Symbol, string Nazwa, int Ilosc, string Cena)
        {
            symbol = Symbol;
            nazwa = Nazwa;
            ilosc = Ilosc;
            cena = float.Parse(Cena);
        }
        public string Symbol
        {
            get
            {
                return symbol;
            }
            set
            {
                symbol = value;
            }
        }
        public string Nazwa
        {
            get
            {
                return nazwa;
            }
            set
            {
                nazwa = value;
            }
        }
        public int Ilosc
        {
            get
            {
                return ilosc;
            }
            set
            {
                ilosc = value;
            }
        }
        public float Cena
        {
            get
            {
                return cena;
            }
            set
            {
                cena = value;
            }
        }
        public float Wartosc
        {
            get
            {
                float wartosc = ilosc * cena;
                return wartosc;
            }
        }
        public string IloscJakoText
        {
            get
            {
                return ilosc.ToString();
            }
            set
            {
                ilosc = Int32.Parse(value);
            }
        }
        public string CenaJakoText
        {
            get
            {
                return cena.ToString("F");
            }
            set
            {
                cena = float.Parse(value);
            }
        }
        public string WartoscJakoText
        {
            get
            {
                float wartosc = ilosc * cena;
                return wartosc.ToString("F");
            }
        }
        public SpisInwentaryzacyjnyPozycja Kopia()
        {
            SpisInwentaryzacyjnyPozycja spisInPoz = new SpisInwentaryzacyjnyPozycja(symbol, nazwa, ilosc, cena);
            return spisInPoz;
        }
                
    }
}
