using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using System.IO;
using System.Threading;

namespace Inwentaryzacja
{
   
    public partial class Form1 : Form
    {
        
       // List<string> daneZpliku;
        private DialogResult saveFileDialog1_dr = DialogResult.None;
       

        public Form1()
        {
            InitializeComponent();
            //dataGridView1.Columns["Cena"].Visible = true;
            Main_Class.mainDataGridView = dataGridView1;
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            Refresh();
            Enabled = false;
            Komunikat komunikat = new Komunikat();
            komunikat.Start(Main_Class.LadujAsortyment, "Ładowanie bazy Asortymentu");
            timer1.Start();
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            string OdczTekst = textBox1.Text;
            if (OdczTekst.EndsWith("\r\n"))
            {
                OdczTekst = OdczTekst.Remove(OdczTekst.Length - 2);
                if (OdczTekst.Length > 0)
                {
                    List<Asortyment> listAsor = new List<Asortyment>();
                    foreach (Asortyment asorZtab in Main_Class.asortymenty)
                    {
                        if (asorZtab.Symbol.StartsWith(OdczTekst))
                            listAsor.Add(asorZtab);
                    }
                    //------------ Jezeli znaleziono tylko 1 pasujacy asortyment----------------------
                    if (listAsor.Count == 1)
                    {
                        Main_Class.DodajPozDoSpisu(listAsor[0].Symbol, listAsor[0].Nazwa,listAsor[0].CenaJakoText, "1", dataGridView1);
                        SystemSounds.Asterisk.Play();
                        if(!Main_Class.ZmianaDanych)
                            Main_Class.ZmianaDanych = true;
                    }
                    //-----------Wykonaj jeżeli nie znaleziono żadnego pasującego asortymetu--------------
                    if(listAsor.Count==0)
                    {
                        System.Threading.Thread wBlad2 = new System.Threading.Thread(new System.Threading.ThreadStart(DzwiekBledu2));
                        wBlad2.Start();
                        Dodanie_Asortymentu dodawanieAsortymentu = new Dodanie_Asortymentu(OdczTekst);
                        dodawanieAsortymentu.ShowDialog();
                        if(dodawanieAsortymentu.ExitCode==1)
                        {
                            Main_Class.DodajPozDoSpisu(dodawanieAsortymentu.Symbol, dodawanieAsortymentu.Nazwa, dodawanieAsortymentu.Cena, "1", dataGridView1);
                            if (!Main_Class.ZmianaDanych)
                                Main_Class.ZmianaDanych = true;
                        }
                    }
                    //--------Wykonaj jeżeli znaleziono więcej niż 1 pasujący asortyment-------------
                    if (listAsor.Count > 1)
                    {
                        System.Threading.Thread wBlad2 = new System.Threading.Thread(new System.Threading.ThreadStart(DzwiekBledu2));
                        wBlad2.Start();
                        FormWyborAsortymentu formWybAsort = new FormWyborAsortymentu(listAsor);
                        formWybAsort.ShowDialog();
                        if (formWybAsort.ExitCode == 1)
                        {
                            Main_Class.DodajPozDoSpisu(formWybAsort.Symbol, formWybAsort.Nazwa, formWybAsort.Cena,"1", dataGridView1);
                            if (!Main_Class.ZmianaDanych)
                                Main_Class.ZmianaDanych = true;
                        }
                    }
                }
                textBox1.Clear();
            }

            Cursor = Cursors.Default;
        }

   //-------------- Otwarcie Spisu Inwentaryzacyjnego z pliku -------------------------------------------------------

        private void button1_Click(object sender, EventArgs e)
        {            
            if((dataGridView1.RowCount>0)&&(Main_Class.ZmianaDanych))
            {
                DialogResult x = MessageBox.Show("Spis Inwentaryzacyjny zawiera niezapisane dane. Otwarcie pliku z Innym Spisem Inwentaryzacyjnym spowoduje usunięcie istniejącego Spisu. " +
                                  "Czy nadal chcesz otworzyć plik ze Spisem Inwentaryzacyjnym ?",
                                  "Spis Inwentaryzacyjny zawiera dane", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,MessageBoxDefaultButton.Button2);
                if (x != DialogResult.Yes)
                {
                    textBox1.Focus();
                    return;
                }                    
            }
            DialogResult dr = openFileDialog1.ShowDialog();
            if(dr!=DialogResult.OK)
            {
                textBox1.Focus();
                return;
            }
            this.Cursor = Cursors.WaitCursor;
            StreamReader reader = new StreamReader(openFileDialog1.OpenFile());
            if (reader.EndOfStream)
            {
                reader.Close();
                this.Cursor = Cursors.Default;
                MessageBox.Show("Brak danych w pliku: "+openFileDialog1.FileName, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox1.Focus();
                return;
            }
            dataGridView1.Rows.Clear();
            String[] wierszZpliku;
            do
            {
                string wierszT = reader.ReadLine().Trim();
                wierszZpliku = wierszT.Split('\t');
                if(wierszZpliku.Length!=4)
                {
                    reader.Close();
                    this.Cursor = Cursors.Default;
                    dataGridView1.Rows.Clear();
                    MessageBox.Show("Niepoprawny format pliku", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox1.Focus();
                    Main_Class.ZmianaDanych = false;
                    return;
                }
                Main_Class.DodajPozDoSpisu(wierszZpliku[0], wierszZpliku[1], wierszZpliku[2], wierszZpliku[3], dataGridView1);
            }
            while (!(reader.EndOfStream));
            reader.Close();
            textBox1.Focus();
            Main_Class.ZmianaDanych = false;
            this.Cursor = Cursors.Default;
        }
        //-------------- Zapisanie Spisu Inwentaryzacyjnego do pliku -------------------------
        private void button2_Click(object sender, EventArgs e)
        {
            saveFileDialog1_dr = saveFileDialog1.ShowDialog();
            if (saveFileDialog1_dr == DialogResult.OK)
            {
                Cursor = Cursors.WaitCursor;
                StreamWriter writer = new StreamWriter(saveFileDialog1.OpenFile());
                foreach (DataGridViewRow wiersz in dataGridView1.Rows)
                {
                    DataGridViewCellCollection rowCells = wiersz.Cells;
                    writer.WriteLine(rowCells["Symbol"].Value.ToString() + '\t'
                                    + rowCells["Nazwa"].Value.ToString() + '\t'
                                    + rowCells["Cena"].Value.ToString() + '\t'
                                    + rowCells["Ilosc"].Value.ToString());
                }
                writer.Close();
                Main_Class.ZmianaDanych = false;
                Cursor = Cursors.Default;
            }

            textBox1.Focus();
        }
    //-----------------------------------------------------------------------------------------------
       
        void DzwiekBledu2()
        {
            SystemSounds.Hand.Play();
            System.Threading.Thread.Sleep(400);
            SystemSounds.Hand.Play();
        }

     //------------------ Usunięcie pozycji ze Spisu Inwentaryzacyjnego ---------------------------------------

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.RowCount == 0)
                MessageBox.Show("Spis Inwentaryzacyjny nie zawiera żadnych danych do usunięcia", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                DialogResult dr = MessageBox.Show("Czy napewno chcesz usunąć pozycję: " + dataGridView1.SelectedRows[0].Cells["Symbol"].Value.ToString()
                                 + " " + dataGridView1.SelectedRows[0].Cells["Nazwa"].Value.ToString()+" ?", "Pytanie ?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (dr == DialogResult.Yes)
                {
                    dataGridView1.Rows.Remove(dataGridView1.SelectedRows[0]);
                    if((dataGridView1.RowCount>0)&&(dataGridView1.SelectedRows.Count==0))
                        dataGridView1.Rows[dataGridView1.RowCount-1].Selected=true;
                    for (int x = 0; x < dataGridView1.RowCount; x++)
                        dataGridView1[0, x].Value = (x + 1).ToString();
                    Main_Class.ZmianaDanych = true;
                }
            }
            textBox1.Focus();
        }

   //--------------------------------------------------------------------------------------------------------------

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Main_Class.ZmianaDanych)
            {
                DialogResult dr = MessageBox.Show("Czy zapisać Spis Inwentaryzacyjny ?", "czy zapisać ?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question,MessageBoxDefaultButton.Button3);
                if (dr == DialogResult.Yes)
                {
                    button2_Click(this, EventArgs.Empty);
                    if(saveFileDialog1_dr==DialogResult.Cancel)
                    {
                        textBox1.Focus();
                        e.Cancel = true;
                    }
                }
                if (dr==DialogResult.Cancel)
                {
                    textBox1.Focus();
                    e.Cancel = true;
                }
            }
        }

            

        

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            Enabled = true;
            Cursor = Cursors.Default;

        }

   //------------- Czyszczenie Spisu Inwentaryzacyjnego -----------------------------------------------

        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.RowCount > 0)
            {
                DialogResult dr = DialogResult.Yes;
                if (Main_Class.ZmianaDanych)
                    dr = MessageBox.Show("Spis Inwentaryzacyjny nie został zapisany.\r\nCzy napewno chcesz wyczyścić całą zawartość Spisu Inwentaryzacyjnego ?", "Ostrzeżenie !"
                    , MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (dr == DialogResult.Yes)
                {
                    dataGridView1.Rows.Clear();
                    Main_Class.ZmianaDanych = false;
                }
            }
            textBox1.Focus();
        }

    //--------------------------------------------------------------------------------------------------

    }


    
}
