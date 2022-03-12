using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace A_level_Computer_Science_Project
{
    public partial class frmChoiceBox : Form
    {
        public int ReturnValue1 { get; set; }
        string headerText;
        string subheaderText;
        string opt1header;
        string opt1body;
        string opt2header;
        string opt2body;
        string opt3header;
        string opt3body;
        public frmChoiceBox(string header, string subheader, string opt1h, string opt1b, string opt2h, string opt2b, string opt3h, string opt3b)
        {
            InitializeComponent();
            headerText = header;
            subheaderText = subheader;
            opt1header = opt1h;
            opt1body = opt1b;
            opt2header = opt2h;
            opt2body = opt2b;
            opt3header = opt3h;
            opt3body = opt3b;
        }

        private void frmChoiceBox_Load(object sender, EventArgs e)
        {
            lblHeader.Text = headerText;
            lblSubheading.Text = subheaderText;
            lblChoice1.Text = opt1header;
            lblChoice1Desc.Text = opt1body;
            lblChoice2.Text = opt2header;
            lblChoice2Desc.Text = opt2body;
            lblChoice3.Text = opt3header;
            lblChoice3Desc.Text = opt3body;

        }

        private void btnChoice1_Click(object sender, EventArgs e)
        {
            
            this.ReturnValue1 = 1;
            this.DialogResult = DialogResult.OK;
            this.Close();
            
        }

        private void btnChoice2_Click(object sender, EventArgs e)
        {
            this.ReturnValue1 = 2;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnChoice3_Click(object sender, EventArgs e)
        {
            this.ReturnValue1 = 3;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnRandomChoice_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();
            int random = rnd.Next(1, 4);
            this.ReturnValue1 = random;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
