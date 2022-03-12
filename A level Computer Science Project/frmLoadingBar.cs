using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace A_level_Computer_Science_Project
{
    public partial class frmLoadingBar : Form
    {
        public frmLoadingBar()
        {
            InitializeComponent();
        }

        private void frmLoadingBar_Load(object sender, EventArgs e)
        {
            timer1.Start();
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
