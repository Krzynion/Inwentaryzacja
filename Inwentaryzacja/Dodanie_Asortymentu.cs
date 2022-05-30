using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Inwentaryzacja
{
    public partial class Dodanie_Asortymentu : Form
    {
        private string SymbolAsortymentu;
        private string NazwaAsortymentu;
        private string CenaAsortymentu;
        private int exitCode = 0;
        public Dodanie_Asortymentu()
        {
            InitializeComponent();
            
        }
        public Dodanie_Asortymentu(string Symbol)
        {
            InitializeComponent();
            textBox1.Text = Symbol;
            textBox2.Select();
            
        }
        public string Symbol
        {
            get
            {
                return this.SymbolAsortymentu;
            }
        }
        public string Nazwa
        {
            get
            {
                return this.NazwaAsortymentu;
            }
        }
        public string Cena
        {
            get
            {
                return this.CenaAsortymentu;
            }
        }
        public int ExitCode
        {
            get
            {
                return this.exitCode;
            }
        }

        //-----------Dodaj i Zapisz nowy Asortyment ------------------------------
        private void button1_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            string symbolT = textBox1.Text.Trim();
            string nazwaT = textBox2.Text.Trim();
            string cenaT = textBox3.Text.Trim();
            //-------------- Sprawdzenie czy asortyment istnieje -----------------------
            foreach (Asortyment asort in Main_Class.asortymenty)
            {
                if (asort.Symbol==symbolT)
                {
                    Cursor = Cursors.Default;
                    MessageBox.Show("Asortyment o takim Symbolu już istnieje", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox1.Focus();
                    exitCode = -1;
                    return;
                }
            }

            //-----------------------------------------------------------------------------
            if(nazwaT.Length==0)
            {
                Cursor = Cursors.Default;
                MessageBox.Show("Pole Nazwa nie może być puste", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox2.Focus();
                exitCode = -1;
                return;
            }
            try
            {
                float cenaF = float.Parse(cenaT);
                if (cenaF < 0)
                    throw new System.FormatException("Liczba w polu Cena nie może być ujemna");
                cenaT = cenaF.ToString("F");

            }
            catch(System.Exception ex)
            {
                Cursor = Cursors.Default;
                MessageBox.Show("Niepoprawna watość w polu Cena\r\n"+ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox3.Focus();
                exitCode = -1;
                return;
            }
            StreamWriter writer = new StreamWriter("Asortyment.dat", true);
            writer.WriteLine(symbolT + '\t' + nazwaT + "\t"+cenaT+"\tN");
            writer.Close();
            Main_Class.LadujAsortyment();
            SymbolAsortymentu = symbolT;
            NazwaAsortymentu = nazwaT;
            CenaAsortymentu = cenaT;
            Cursor = Cursors.Default;
            exitCode = 1;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}