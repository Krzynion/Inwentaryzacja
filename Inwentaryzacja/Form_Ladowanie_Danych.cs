using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Inwentaryzacja
{
    public partial class Form_Ladowanie_Danych : Form
    {
        List<string> daneZpliku;
        int licznik_danychZpliku;
        DataGridView dataGridView1;
        List<Asortyment> asortymenty;
        Thread w1;
        Thread w2;
        Thread w3;
        Thread w4;
        Thread w5;

        public Form_Ladowanie_Danych()
        {
            InitializeComponent();
            

        }

        public void start(DataGridView dataGridView, List<Asortyment> ListaAsortyment, List<string> DaneZpliku)
        { 
            dataGridView1 = dataGridView;
            asortymenty = ListaAsortyment;
            daneZpliku = DaneZpliku;
            dataGridView1.RowCount = daneZpliku.Count;
            licznik_danychZpliku = 0;
            w1 = new Thread(new ThreadStart(LadujDaneDoSpisu));
            w2 = new Thread(new ThreadStart(LadujDaneDoSpisu));
            w3 = new Thread(new ThreadStart(LadujDaneDoSpisu));
            w4 = new Thread(new ThreadStart(LadujDaneDoSpisu));
            w5 = new Thread(new ThreadStart(closeForm));
            w1.Start();
            w2.Start();
            w3.Start();
            w4.Start();
            w5.Start();
            ShowDialog();
        }


       
        
        void LadujDaneDoSpisu()
        {
            Boolean czyjest;
            int nr_wiersza;
            while (licznik_danychZpliku < daneZpliku.Count)
            {
                nr_wiersza = Interlocked.Increment(ref licznik_danychZpliku);
                nr_wiersza--;
                czyjest = false;
                foreach (Asortyment asortWiersz in asortymenty)
                {
                    if (asortWiersz.Symbol == daneZpliku[nr_wiersza])
                    {
                        dataGridView1["Lp", nr_wiersza].Value = (nr_wiersza + 1).ToString();
                        dataGridView1["Symbol", nr_wiersza].Value = daneZpliku[nr_wiersza];
                        dataGridView1["Nazwa", nr_wiersza].Value = asortWiersz.Nazwa;
                        dataGridView1["Cena", nr_wiersza].Value = asortWiersz.CenaJakoText;
                        czyjest = true;
                        break;
                    }
                }
                if (!czyjest)
                {
                    dataGridView1["Lp", nr_wiersza].Value = (nr_wiersza + 1).ToString();
                    dataGridView1["Symbol", nr_wiersza].Value = daneZpliku[nr_wiersza];
                    dataGridView1["Nazwa", nr_wiersza].Value = "";
                    dataGridView1["Cena", nr_wiersza].Value = "";
                }
                
            }
            

            
        }
        
        void closeForm()
        {
            
            w1.Join();
            w2.Join();
            w3.Join();
            w4.Join();
            Invoke(new Action(delegate() { Close(); })) ;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int procent =(licznik_danychZpliku*100)/daneZpliku.Count;
            label2.Text=procent.ToString() + " %";
        }
    }
}
