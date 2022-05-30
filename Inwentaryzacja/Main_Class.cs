using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace Inwentaryzacja
{
    public static class Main_Class
    {
        public static List<Asortyment> asortymenty;
       
        public static System.Windows.Forms.DataGridView mainDataGridView;
        public static Boolean ZmianaDanych = false;

        public static void LadujAsortyment()
        {
            FileInfo sourcefile = new FileInfo("Asortyment.dat");
            StreamReader reader = sourcefile.OpenText();
            if (reader.EndOfStream)
            {
                reader.Close();
                return;
            }

            string[] tabWiersz;
            asortymenty = new List<Asortyment>();
            Asortyment asortyment;
            Boolean error = false;
            do
            {
                tabWiersz = reader.ReadLine().Split('\t');
                if ((tabWiersz.Length == 3) || (tabWiersz.Length == 4))
                {
                    asortyment = new Asortyment(tabWiersz[0].Trim(), tabWiersz[1].Trim(), tabWiersz[2].Trim());
                    if (tabWiersz.Length == 4)
                        asortyment.Status = tabWiersz[3];
                    asortymenty.Add(asortyment);
                }
                else
                {
                    error = true;
                    break;
                }

            } while (!(reader.EndOfStream));
            if(error)
            {
                asortymenty = null;
                System.Windows.Forms.MessageBox.Show("Nieprawidłowy format bazy Asortymentów", "Błąd !", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                System.Windows.Forms.Application.Exit();
            }
            reader.Close();
        }

        public static void DodajPozDoSpisu(string Symbol, String Nazwa, String Cena, String Ilosc, System.Windows.Forms.DataGridView dataGridView)
        {
            int newRowIndex = dataGridView.Rows.Add();
            dataGridView[0, newRowIndex].Value = (newRowIndex + 1).ToString();
            dataGridView[1, newRowIndex].Value = Symbol;
            dataGridView[2, newRowIndex].Value = Nazwa;
            dataGridView[3, newRowIndex].Value = Cena;
            dataGridView[4, newRowIndex].Value = Ilosc;
            dataGridView.Rows[newRowIndex].Selected = true;
            dataGridView.FirstDisplayedScrollingRowIndex = newRowIndex;
        }


        public static int SprawdzAsortyment()
        {
            Boolean error = false;
            int exitCode = 0;
            StreamReader reader = null;
            String zawartoscPliku;
            if (!File.Exists("Asortyment.dat"))
            {
                error = true;
                exitCode = -1;
            }
            if (!error)
            {
                reader = new StreamReader("Asortyment.dat", Encoding.UTF8);
                if (reader.EndOfStream)
                {
                    error = true;
                    exitCode = -2;
                }
            }
            if (!error)
            {
                zawartoscPliku = reader.ReadToEnd();

            }

            return exitCode;
        }

        /*-----Zlicz spis inwentaryzacyjny--------------------------
        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.RowCount == 0)
                return;
            List<SpisInwentaryzacyjnyPozycja> SpisInwentL = new List<SpisInwentaryzacyjnyPozycja>();
            foreach (DataGridViewRow wiersz in dataGridView1.Rows)
            {
                SpisInwentaryzacyjnyPozycja SpisInwentP = new SpisInwentaryzacyjnyPozycja();
                SpisInwentP.Symbol = wiersz.Cells["Symbol"].Value.ToString();
                SpisInwentP.Nazwa = wiersz.Cells["Nazwa"].Value.ToString();
                SpisInwentP.CenaJakoText = wiersz.Cells["Cena"].Value.ToString();
                SpisInwentL.Add(SpisInwentP);
            }
            List<SpisInwentaryzacyjnyPozycja> SpisInwLiZlicz = SpisInwentaryzacyjny.ZliczWgSymbolu(SpisInwentL);
            //---------Zapis do pliku ----------------------------------------------
            if (saveFileDialog2.ShowDialog() == DialogResult.OK)
            {
                Cursor = Cursors.WaitCursor;
                StreamWriter writer = new StreamWriter(saveFileDialog2.OpenFile());
                string wiersz = "";
                foreach (SpisInwentaryzacyjnyPozycja spinp in SpisInwLiZlicz)
                {
                    wiersz = spinp.Symbol + '\t' +
                        spinp.Nazwa + '\t' +
                        spinp.IloscJakoText + '\t' +
                        spinp.CenaJakoText + '\t' +
                        spinp.WartoscJakoText;
                    writer.WriteLine(wiersz);
                }
                writer.Close();
                Cursor = Cursors.Default;
            }
        }
        */
    }
}
