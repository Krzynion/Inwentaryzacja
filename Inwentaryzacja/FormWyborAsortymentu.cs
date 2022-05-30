using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inwentaryzacja
{
    public partial class FormWyborAsortymentu : Form
    {
        private string SymbolAs;
        private string NazwaAs;
        private string CenaAs;
        private int exitCode = 0;

        public FormWyborAsortymentu(List<Asortyment>Asortyment)
        {
            InitializeComponent();
            if (Asortyment.Count == 0)
                return;
            dataGridView1.RowCount = Asortyment.Count;
            for(int x = 0;x<Asortyment.Count;x++)
            {
                dataGridView1.Rows[x].Cells["dgv_Symbol"].Value = Asortyment[x].Symbol;
                dataGridView1.Rows[x].Cells["dgv_Nazwa"].Value = Asortyment[x].Nazwa;
                dataGridView1.Rows[x].Cells["dgv_Cena"].Value = Asortyment[x].CenaJakoText;
            }
            this.SymbolAs = "";
            this.NazwaAs = "";
            this.CenaAs = "";
        }

        public string Symbol
        {
            get
            {
                return this.SymbolAs;
            }
        }
        public string Nazwa
        {
            get
            {
                return this.NazwaAs;
            }
        }
        public string Cena
        {
            get
            {
                return this.CenaAs;
            }
        }
        public int ExitCode
        {
            get
            {
                return this.exitCode;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection wybrane_wiersze = dataGridView1.SelectedRows;
            if (wybrane_wiersze.Count != 1)
                return;
            this.SymbolAs = wybrane_wiersze[0].Cells["dgv_Symbol"].Value.ToString();
            this.NazwaAs = wybrane_wiersze[0].Cells["dgv_Nazwa"].Value.ToString();
            this.CenaAs = wybrane_wiersze[0].Cells["dgv_Cena"].Value.ToString();
            this.exitCode = 1;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
