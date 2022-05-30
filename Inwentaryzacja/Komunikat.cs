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
    public delegate void del_Metoda();
    public partial class Komunikat : Form
    {
        private del_Metoda metoda;
        public Komunikat()
        {
            InitializeComponent();
                        
        }
        public void Start(del_Metoda MetodaDoWykonania, string TekstDoWyswietlenia)
        {
            label1.Text = TekstDoWyswietlenia;
            metoda = MetodaDoWykonania;
            ShowDialog();           
        }

        private void Komunikat_Shown(object sender, EventArgs e)
        {
            Refresh();
            Cursor = Cursors.WaitCursor;
            metoda();
            Cursor = Cursors.Default;
            Close();
        }
    }
}
