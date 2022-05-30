using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inwentaryzacja
{
    public class Asortyment
    {
            private string symbol;
            private string nazwa;
            private float cena;
            private string status;
            public Asortyment()
            {
                symbol = "";
                nazwa = "";
                status = "";
            }
            public Asortyment(string Symbol, string Nazwa, float Cena)
            {
                symbol = Symbol;
                nazwa = Nazwa;
                cena = Cena;
                status = "";
            }
            public Asortyment(string Symbol, string Nazwa, string CenaJakoText)
            {
                symbol = Symbol;
                nazwa = Nazwa;
                cena = float.Parse(CenaJakoText);
                status = "";
            }
            public Asortyment(string Symbol, string Nazwa, float Cena, string Status)
            {
                symbol = Symbol;
                nazwa = Nazwa;
                status = Status;
                cena = Cena;
            }
            public Asortyment(string Symbol, string Nazwa, string CenaJakoText, string Status)
            {
                symbol = Symbol;
                nazwa = Nazwa;
                status = Status;
                cena = float.Parse(CenaJakoText);
            }

            public string Symbol
            {
                get
                {
                    return this.symbol;
                }
                set
                {
                    this.symbol = value;
                }
            }
            public string Nazwa
            {
                get
                {
                    return this.nazwa;
                }
                set
                {
                    this.nazwa = value;
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
            public string Status
            {
                get
                {
                    return this.status;
                }
                set
                {
                    this.status = value;
                }
            }
            public string CenaJakoText
            {
                get
                {
                    string cena_s = cena.ToString("F");
                    return cena_s;
                }
                set
                {
                    string cena_s = value;
                    cena = float.Parse(cena_s);
                }
            }
        }
    
}
