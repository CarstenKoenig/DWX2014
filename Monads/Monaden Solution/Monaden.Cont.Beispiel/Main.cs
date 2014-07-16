using System;
using System.Windows.Forms;

namespace Monaden.Cont.Beispiel
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            Continuation.Controls.verzögertesHallo();
            Continuation.Controls.aktion(eingabeGruppe, Aktivieren, Nachricht, Ok);
        }
    }
}
