using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;


namespace A_level_Computer_Science_Project
{
    public partial class frmShowSavedGames : Form
    {
        public MainCharacter mainCharacter = new MainCharacter(); //creating new public object of type Person called mainCharacter
        public Event[] eventArray = new Event[1000];
        public string filepath;
        public ControlClass controlClass = new ControlClass();
        public frmShowSavedGames()
        {
            InitializeComponent();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            openFile();
            //MessageBox.Show(filepath);
        }
        public void openFile()
        {
            
            //code below originally came from Microsoft docs - then it was mutilated by me!
            // fileContent = string.Empty;
            //controlClass.Filepath = string.Empty;
            
            //now try using a folderbrowserdialog
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            folderDlg.ShowNewFolderButton = true;
            // Show the FolderBrowserDialog.  
            DialogResult result = folderDlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                //txtSaveLocation.Text = folderDlg.SelectedPath;
                Environment.SpecialFolder root = folderDlg.RootFolder;
                string loadFilepath = folderDlg.SelectedPath.ToString();
                //MessageBox.Show(loadFilepath);
                filepath = loadFilepath;

                //Read data in from the file and deserialize it into the objects
                MainCharacter mainCharacter = JsonConvert.DeserializeObject<MainCharacter>(File.ReadAllText(loadFilepath + "\\mainCharacter.json"));
                //Note: Just do main character on its own here then do the rest when the data is actually passed across (due to accessibility issues) 
               
                
                populateForm(mainCharacter);

            }

        
        }
        public void populateForm(MainCharacter mainCharacter)
        {
            //Main character
            txtFirstName.Text = mainCharacter.FirstName;
            txtLastName.Text = mainCharacter.LastName;
            txtGender.Text = mainCharacter.Gender;
            txtSexuality.Text = mainCharacter.Sexuality;
            txtHairColour.Text = mainCharacter.HairColour;
            txtEyeColour.Text = mainCharacter.EyeColour;
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnLoadGame_Click(object sender, EventArgs e)
        {
            
            //Read data in from the file and deserialize it into the objects
            MainCharacter mainCharacter = JsonConvert.DeserializeObject<MainCharacter>(File.ReadAllText(filepath + "\\mainCharacter.json"));
            Event[] eventArray = JsonConvert.DeserializeObject<Event[]>(File.ReadAllText(filepath + "\\eventArray.json"));
            ControlClass controlClass = JsonConvert.DeserializeObject<ControlClass>(File.ReadAllText(filepath + "\\controlClass.json"));
            GenericFamilyMember[] familyArray = JsonConvert.DeserializeObject<GenericFamilyMember[]>(File.ReadAllText(filepath + "\\familyArray.json"));
            Score mainCharacterScores = JsonConvert.DeserializeObject<Score>(File.ReadAllText(filepath + "\\mainCharacterScores.json"));
            Partner partner = JsonConvert.DeserializeObject<Partner>(File.ReadAllText(filepath + "\\partner.json"));

            controlClass.Filepath = filepath;

            //MessageBox.Show(mainCharacterScores.LifeScore.ToString());

            frmMainGameScreen fMainGameScreen = new frmMainGameScreen(
                mainCharacter,
                eventArray,
                controlClass,
                familyArray,
                mainCharacterScores,
                partner
                ); //create new frmMainGameScreen with parameter mainCharacter
            fMainGameScreen.Show(); //show new main game screen.
            this.Hide(); //hide this form
            //fLoadingBar.Hide();
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            frmMainMenu fMainMenu = new frmMainMenu();
            fMainMenu.Show();
            this.Hide();
        }
    }
}
