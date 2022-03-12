using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace A_level_Computer_Science_Project
{
    public partial class frmNewGame : Form
    {
        //setup random generation
        public Random rnd = new Random();

        public MainCharacter mainCharacter = new MainCharacter(); //creating new public object of type Person called mainCharacter
        public string saveLocation = null;

        //ingame date
        public DateTime inGameDate = DateTime.Today.Date;

        //Create array of event object called eventArray
        public int numberOfEvents = 1000;
        public Event[] eventArray = new Event[1000];
        public int nextEvent = 0;

        //create array of GenericFamilyMember objects called familyArray
        public int numberOfFamily = 30;
        public GenericFamilyMember[] familyArray = new GenericFamilyMember[30];
        public int nextFamily = 0;

        //Setup the control class
        public ControlClass controlClass = new ControlClass();

        public Score mainCharacterScores = new Score();

        //partner
        public Partner partner = new Partner();
        

        public frmNewGame()
        {
            
            InitializeComponent();
            txtSaveLocation.Clear();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //first have to check that the user has actually specified a location to save into
           // MessageBox.Show(txtSaveLocation.Text);
            if(saveLocation != null)
            {
                //user hasn specified path, cary on with the saving.
                //display the loading bar
                frmLoadingBar fLoadingBar = new frmLoadingBar();
                fLoadingBar.Show();

                //save the user entered information into the main character object
                mainCharacter.FirstName = txtFirstName.Text;
                mainCharacter.LastName = txtLastName.Text;
                mainCharacter.Gender = cboGender.Text;
                mainCharacter.Sexuality = txtSexuality.Text;
                mainCharacter.Age = 0;
                mainCharacter.LivingStatus = true;
                mainCharacter.DateOfBirth = DateTime.Today.Date;
                mainCharacter.HairColour = cboHairColour.Text;
                mainCharacter.EyeColour = cboEyeColour.Text;
                mainCharacter.InEducation = false;
                mainCharacter.InJob = false;
                mainCharacter.JobCurrentDuration = 0;

                //Populate control class
                controlClass.Filepath = saveLocation;
                controlClass.InGameDate = DateTime.Today;
                controlClass.NextEvent = nextEvent;
                controlClass.NumberOfEvents = numberOfEvents;
                controlClass.NextFamily = nextFamily;
                controlClass.NumberOfFamily = numberOfFamily;

                //create event objects needed for game
                for (int i = 0; i < controlClass.NumberOfEvents; i++)
                {
                    eventArray[i] = new Event();
                }




                //Now create family members
                for (int i = 0; i < controlClass.NumberOfFamily; i++)
                {
                    GenericFamilyMember tempFamily = new GenericFamilyMember(); //make new instance of the object
                    tempFamily.populateGenericFamilyMember(i, mainCharacter); //populate that object using the 'constructor' in the GenericFamilyMemberclass
                    familyArray[i] = tempFamily; //transfer the temp object into the array.
                    if (familyArray[i].FirstName != null)
                    {
                        controlClass.NextFamily = i + 1;
                    }
                }

                eventArray[controlClass.NextEvent].Category = "Life Event";
                eventArray[controlClass.NextEvent].DateHappened = DateTime.Today;
                eventArray[controlClass.NextEvent].Description = "You were born to parents " + familyArray[0].FirstName + " and " + familyArray[1].FirstName;
                controlClass.NextEvent++;

                //setup the scores object for the main (use random generation to calculate values)
                mainCharacterScores.EducationScore = 0;
                mainCharacterScores.JobScore = 0;
                mainCharacterScores.HappinessScore = rnd.Next(0, 101);
                mainCharacterScores.MedicalScore = 1000;
                mainCharacterScores.LifeScore = 0;

                //generate the happinessscoreplus value
                controlClass.HappinessScorePlus = generateHappinessScorePlusVal();

                //load the next form - main game screen
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
                fLoadingBar.Hide();

            }
            else
            {
                MessageBox.Show("Please specify a save location");
            }


            
        }


        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*  SaveFileDialog saveFileDialog1 = new SaveFileDialog();
              saveFileDialog1.Filter = "json files (*.json)|*.json";
              saveFileDialog1.FilterIndex = 1;
              saveFileDialog1.RestoreDirectory = true;
              saveFileDialog1.AddExtension = true;


              if (saveFileDialog1.ShowDialog() == DialogResult.OK)
              {
                  saveLocation = saveFileDialog1.FileName;
                  txtSaveLocation.Text = saveLocation;
              }
            */

            //now try using a folderbrowserdialog
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            folderDlg.ShowNewFolderButton = true;
            // Show the FolderBrowserDialog.  
            DialogResult result = folderDlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtSaveLocation.Text = folderDlg.SelectedPath;
                Environment.SpecialFolder root = folderDlg.RootFolder;
                saveLocation = folderDlg.SelectedPath.ToString();
            }



        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            frmMainMenu fMainMenu = new frmMainMenu();
            fMainMenu.Show();
            this.Hide();
        }


        private void btnSettings_Click(object sender, EventArgs e)
        {


        }

        private void btnRandom_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();
            int gender = rnd.Next(1, 3);
            switch (gender)
            {
                case 1:
                    //male
                    txtFirstName.Text = Person.genMFN();
                    cboGender.Text = "Male";
                    break;
                case 2:
                    //female
                    txtFirstName.Text = Person.genFFN();
                    cboGender.Text = "Female";
                    break;
                default:
                    txtFirstName.Text = "Sam";
                    break;
            }
            txtLastName.Text = Person.genLN();
            int hair = rnd.Next(1,5);
            int eye = rnd.Next(1, 6);

            switch (hair)
            {
                case 1:
                    cboHairColour.Text = "Blonde";
                    break;
                case 2:
                    cboHairColour.Text = "Brown";
                    break;
                case 3:
                    cboHairColour.Text = "Red";
                    break;
                case 4:
                    cboHairColour.Text = "Black";
                    break;
                default:
                    cboHairColour.Text = "Blonde";
                    break;
            }

            switch (eye)
            {
                case 1:
                    cboEyeColour.Text = "Brown";
                    break;
                case 2:
                    cboEyeColour.Text = "Blue";
                    break;
                case 3:
                    cboEyeColour.Text = "Green";
                    break;
                case 4:
                    cboEyeColour.Text = "Gray";
                    break;
                case 5:
                    cboEyeColour.Text = "Amber";
                    break;
                default:
                    cboEyeColour.Text = "Brown";
                    break;
            }
            
        }

        public int generateHappinessScorePlusVal()
        {
            //generate the happinessScorePlus value.
            //random chance that this is a low plus score initially, 
            Random rnd = new Random();
            int randomChance = rnd.Next(1, 30);
            int val = 10;
            if (randomChance == 4)
            {
                //goes up slowly
                val = rnd.Next(1, 10);
            }
            else
            {
                //goes up quickly
                val = rnd.Next(11, 30);
            }
            return val;
        }
    }
}