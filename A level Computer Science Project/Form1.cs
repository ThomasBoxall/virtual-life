using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace A_level_Computer_Science_Project
{
    
    public partial class frmMainMenu : Form
    {
        public frmMainMenu()
        {
            InitializeComponent();
        }

        private void btnNewGame_Click(object sender, EventArgs e)
        {
            frmNewGame fNewGame = new frmNewGame();
            fNewGame.Show();
            this.Hide();
        }

        private void btnShowSaved_Click(object sender, EventArgs e)
        {
            frmShowSavedGames fShowSavedGames = new frmShowSavedGames();
            fShowSavedGames.Show();
            this.Hide();
        }

        private void frmMainMenu_Load(object sender, EventArgs e)
        {

        }

        private void btnSettings_Click(object sender, EventArgs e)
        {

            
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            frmAbout fAbout = new frmAbout();
            fAbout.Show();
        }
    }
}
