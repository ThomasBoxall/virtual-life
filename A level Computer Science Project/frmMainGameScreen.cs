using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.IO;
using Newtonsoft.Json;

namespace A_level_Computer_Science_Project
{
    public partial class frmMainGameScreen : Form
    {
        MainCharacter mainCharacter; //setup the object to put main character into when its transferred
        Event[] eventArray;
        ControlClass controlClass;
        GenericFamilyMember[] familyArray;
        Score mainCharacterScores;
        public Job[] availableJobs = new Job[100];
        public Partner[] potentialPartners = new Partner[15];
        Partner partner = new Partner();

        public frmMainGameScreen(
            MainCharacter mainCharacterTransfer,
            Event[] eventArrayTransfer,
            ControlClass controlClassTransfer,
            GenericFamilyMember[] familyArrayTransfer,
            Score mainCharacterScoresTransfer,
            Partner partnerTransfer
            )
        {
            InitializeComponent();
            mainCharacter = mainCharacterTransfer; //Set the contents of mainCharacter to mainCharacterTransfer which has come from previous form
            eventArray = eventArrayTransfer;
            controlClass = controlClassTransfer;
            familyArray = familyArrayTransfer;
            mainCharacterScores = mainCharacterScoresTransfer;
            partner = partnerTransfer;
        }


        private void btnSaveToFile_Click(object sender, EventArgs e)
        {
            Job newJob = new Job();
            newJob.generateJob(1);
            MessageBox.Show(newJob.JobTitle);
            
        }

        public void saveToFile()
        {
            //taken from Stack Overflow - it seems to work, don't question it!
            File.WriteAllText(controlClass.Filepath + "\\mainCharacter.json", JsonConvert.SerializeObject(mainCharacter));
            File.WriteAllText(controlClass.Filepath + "\\eventArray.json", JsonConvert.SerializeObject(eventArray));
            File.WriteAllText(controlClass.Filepath + "\\controlClass.json", JsonConvert.SerializeObject(controlClass));
            File.WriteAllText(controlClass.Filepath + "\\familyArray.json", JsonConvert.SerializeObject(familyArray));
            File.WriteAllText(controlClass.Filepath + "\\mainCharacterScores.json", JsonConvert.SerializeObject(mainCharacterScores));
            File.WriteAllText(controlClass.Filepath + "\\partner.json", JsonConvert.SerializeObject(partner));
        }

        private void btnSaveAndHome_Click(object sender, EventArgs e)
        {
            frmLoadingBar fLoadingBar = new frmLoadingBar();
            fLoadingBar.Show();
            saveToFile();
            frmMainMenu fMainMenu = new frmMainMenu();
            fMainMenu.Show();
            fLoadingBar.Close();
            this.Hide();
            fLoadingBar.Close();
        }

        private void frmMainGameScreen_Load(object sender, EventArgs e)
        {
            saveToFile();
            //populate the main character info on the first tab
            populateForms();
            yearlyCheck();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public void populateMainCharacterInfo()
        {
            lblMPFirstName.Text = "First name: " + mainCharacter.FirstName;
            lblMPLastName.Text = "Last name: " + mainCharacter.LastName;
            lblMPGender.Text = "Gender: " + mainCharacter.Gender;
            lblMPSexuality.Text = "Sexuality: " + mainCharacter.Sexuality;
            lblMPAge.Text = "Age: " + mainCharacter.Age.ToString();
            lblMPDateOfBirth.Text = "Date Of Birth: " + mainCharacter.DateOfBirth.ToShortDateString();
        }
        public void populateEventBox()
        {
            txtEventBox.ResetText();
            for(int i=0; i< controlClass.NextEvent; i++)
            {
                //txtEventBox.Text = txtEventBox.Text + Environment.NewLine + eventArray[i].DateHappened.ToShortDateString() + " - " +  eventArray[i].Description;
                txtEventBox.AppendText(Environment.NewLine + eventArray[i].DateHappened.ToShortDateString() + " - " + eventArray[i].Description);
            }
        }

        public void populateForms()
        {
            //populate all forms, this can replace multiple lines in both age up and load.

            populateMainCharacterInfo();
            updateEducationPage();
            updateCrimes();
            updateInGameDate();
            populateDGVFamily();
            populateEventBox();
            updateCharCustomisationOptions();
            updateCurrentJobInformation();
            updatePartnerInformation();

            updateScoresPB(); // this will need to be the last function called so all scores have had a chance to update before they are displayed.
        }

        //============================AGE UP========================================
        private void btnAgeUp_Click(object sender, EventArgs e)
        {
            //Increase main character age by 1 and create event for it.
            mainCharacter.Age = mainCharacter.Age + 1;
            controlClass.InGameDate = controlClass.InGameDate.AddYears(1);
            eventArray[controlClass.NextEvent].DateHappened = controlClass.InGameDate;
            eventArray[controlClass.NextEvent].Description = "You turned " + mainCharacter.Age.ToString();
            eventArray[controlClass.NextEvent].Category = "Main character birthday";
            controlClass.NextEvent++;
            if(mainCharacter.InRelationship == true)
            {
                partner.Age = partner.Age + 1;
            }
            //decide if main character should die or not.
            bool returnedDeathMC = deathCheck(mainCharacter.Age);
            if(returnedDeathMC == true)
            {
                killMainCharacter();
            }
            else
            {
                //if maincharacter hasn't just died, progress with the aging up

                //first check for any key life events (can drive, able to go to school etc)
                lifeEvents();
                yearlyCheck();


                //Age up all familiy members
                ageUpFamilyArray();
                //check kill family members
                checkKillGenericFamilyMember();


                //education
                checkEducationStatus();
                generateMedicalScore();


                //job
                if(mainCharacter.InJob == true)
                {
                    //increase job duration
                    mainCharacter.JobCurrentDuration = mainCharacter.JobCurrentDuration + 1;
                    calculateSalary();
                }

                updateHappinessScore(0);
            }
            //some bits need to happen regardless of if the main charcter dead or not, as will need info from them.
            populateForms();


            saveToFile();
         }
        //==========================================================================
        public void updateInGameDate()
        {
            lblInGameDate.Text = "Date in game: " + controlClass.InGameDate.ToShortDateString();
        }

        public void populateDGVFamily()
        {
            //first have to clear it
            dgvFamily.Rows.Clear();
            dgvFamily.Refresh();

            //dgvFamily.DataSource = familyArray;

            //use code from openldbws app to populate dgvFamily

            //first, set var for which column contains which data
            int relationshipCol = 0;
            int firstNameCol = 1;
            int lastNameCol = 2;

            //now loop through familyArray and populate dgvFamily
            // can use a for loop as number of family members are in the controlClass

            for(int x=0; x<controlClass.NumberOfFamily; x++)
            {
                if (familyArray[x].FirstName != null)
                {
                    int i = dgvFamily.Rows.Add();
                    dgvFamily.Rows[i].Cells[relationshipCol].Value = familyArray[x].RelationshipToMain;
                    dgvFamily.Rows[i].Cells[firstNameCol].Value = familyArray[x].FirstName;
                    dgvFamily.Rows[i].Cells[lastNameCol].Value = familyArray[x].LastName;
                }
            }
        }

        private void dgvFamily_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           // MessageBox.Show(dgvFamily.CurrentCell.RowIndex.ToString());
            int currentIndex = dgvFamily.CurrentCell.RowIndex;
            //MessageBox.Show(familyArray[currentIndex].Age.ToString());

            //populate info on form
            lblFamRealtionshipToMain.Text = mainCharacter.FirstName + "'s " + familyArray[currentIndex].RelationshipToMain;
            lblFamFirstName.Text = "First Name: " + familyArray[currentIndex].FirstName;
            lblFamLastName.Text = "Last Name: " + familyArray[currentIndex].LastName;
            lblFamGender.Text = "Gender: " + familyArray[currentIndex].Gender;
            lblFamSexuality.Text = "Sexuality: " + familyArray[currentIndex].Sexuality;
            lblFamAge.Text = "Age: " + familyArray[currentIndex].Age.ToString();
            lblFamLivingStatus.Text = "Living Status: " + familyArray[currentIndex].LivingStatus.ToString();
            lblFamDateOfBirth.Text = "Date Of Birth: " + familyArray[currentIndex].DateOfBirth.ToShortDateString();
            lblFamInRelationship.Text = "In Relationship: " + familyArray[currentIndex].InRelationship.ToString();
            if(familyArray[currentIndex].LivingStatus == false)
            {
                //person is dead so can show death date and reason for death
                lblFamDateOfDeath.Text = "Date Of Death: " + familyArray[currentIndex].DateOfDeath.ToShortDateString();
                lblFamReasonForDeath.Text = "Reason For Death: " + familyArray[currentIndex].ReasonForDeath;
            }
            else
            {
                //person is not dead - don't show death date and reason for death
                lblFamDateOfDeath.Text = "Date Of Death: N/A (haven't died yet!)";
                lblFamReasonForDeath.Text = "Reason For Death: N/A (haven't died yet!)";
            }
        }
        
        private void ageUpFamilyArray()
        {
            //loop through all the members of the family array and add 1 to their age if livingstatus is true
            for(int x=0; x<controlClass.NumberOfFamily; x++)
            {
                if(familyArray[x].LivingStatus == true)
                {
                    //can age them up
                    familyArray[x].Age = familyArray[x].Age + 1;
                }
            }
        }


        public void killMainCharacter()
        {
            //very rough for now, will need to polish at some point

            disableClickables();
            mainCharacter.DateOfDeath = controlClass.InGameDate.AddDays(randomNumber(1,300)); 
            mainCharacter.LivingStatus = false;
            mainCharacter.ReasonForDeath = generateReasonForDeath();
            //generate event showing this
            eventArray[controlClass.NextEvent].Category = "Death";
            eventArray[controlClass.NextEvent].DateHappened = controlClass.InGameDate;
            eventArray[controlClass.NextEvent].Description = "You died due to " + mainCharacter.ReasonForDeath;
            controlClass.NextEvent++;
            MessageBox.Show(mainCharacter.FirstName + " has died a tragic death due to " + mainCharacter.ReasonForDeath, mainCharacter.FirstName + " has died", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            saveToFile();
        }

        public string generateReasonForDeath()
        {
            //Generate a reason for death

            string[] reasonsForDeath = { "a heart attack", "eating too many oreos", "being clawed at by cat" , "slipping on a banana peel", "falling in the otter enclosure at a zoo and being licked to death", "dropping a toaster into the bath", "being crushed by a falling grand piano", "being strangled by a fez tassel", "walking into a cactus", "getting hit by a train", "being clubbed by old lady in the street with her walker", "being crushed by a stampede of hungry campers" };
            Random rnd = new Random();
            int deathReasonIndex = rnd.Next(0, (reasonsForDeath.Length));
            string reasonForDeathReturn = reasonsForDeath[deathReasonIndex];
            return reasonForDeathReturn;
        }

        public void checkKillGenericFamilyMember()
        {
            //loop through all members of familyArray, passing their age into deathCheck. if return=true then kill, else move onto next.

            for(int x=0; x<controlClass.NextFamily; x++)
            {
                if(familyArray[x].LivingStatus == true)
                {
                    bool deathCheckReturn = deathCheck(familyArray[x].Age);
                    if(deathCheckReturn == true)
                    {
                        //RIP - put death code here
                        //MessageBox.Show("DEATH" + x.ToString());
                        familyArray[x].DateOfDeath = controlClass.InGameDate.AddDays(randomNumber(1, 300));
                        familyArray[x].LivingStatus = false;
                        familyArray[x].ReasonForDeath = generateReasonForDeath();
                        //Now gen event
                        eventArray[controlClass.NextEvent].Category = "Death";
                        eventArray[controlClass.NextEvent].DateHappened = controlClass.InGameDate;
                        eventArray[controlClass.NextEvent].Description = familyArray[x].FirstName + " (your " + familyArray[x].RelationshipToMain.ToLower() + ") died due to " + familyArray[x].ReasonForDeath;
                        controlClass.NextEvent++;
                        //now reduce mc happiness score
                        mainCharacterScores.HappinessScore = mainCharacterScores.HappinessScore - randomNumber(1, 100);
                        saveToFile();


                    }
                }
                
            }

        }

        public bool deathCheck(int age)
        {
            bool deathBool = false;
            int deathChance = 1;
            //for as main char gets older, their death chance increases
            /* 00-10 1/30 
             * 11-16 1/25
             * 17-30 1/15 (sig reduction as they are more reckless)
             * 31-50 1/25 (goes up as here should be finding love etc)
             * 51-67 1/20 (getting a bit older)
             * 68-80 1/15 (quite old here)
             * 81-100 1/8 (basically dead already)
             * >100 1/3 (might have a stroke of luck and survive for a bit longer
             */
            if (age <= 10)
            {
                deathChance = 30;
            }
            else if (age >= 11 && age <= 16)
            {
                deathChance = 25;
            }
            else if (age >= 17 && age <= 30)
            {
                deathChance = 15;
            }
            else if (age >= 31 && age <= 50)
            {
                deathChance = 25;
            }
            else if (age >= 51 && age <= 67)
            {
                deathChance = 20;
            }
            else if (age >= 68 && age <= 80)
            {
                deathChance = 15;
            }
            else if (age >= 81 && age <= 99)
            {
                deathChance = 8;
            }
            else if (age >= 100)
            {
                deathChance = 3;
            }
            //now generate random number based off of 1 and deathChance
            Random rnd = new Random();
            int randomReturn = rnd.Next(1, deathChance);
            if (randomReturn == 2)
            {
                //DEATH
                deathBool = true;
            }
            else
            {
                deathBool = false;
            }

            //return deathBool to main program where it will be processed.
            return deathBool;
        }

        public void updateScoresPB()
        {
            if (mainCharacterScores.EducationScore < prbEducationScore.Maximum && mainCharacterScores.EducationScore > prbEducationScore.Minimum)
            {
                prbEducationScore.Value = mainCharacterScores.EducationScore;
            }
            if(mainCharacterScores.HappinessScore < prbHappinessScore.Maximum && mainCharacterScores.HappinessScore > prbHappinessScore.Minimum)
            {
                prbHappinessScore.Value = mainCharacterScores.HappinessScore;
            }
            if (mainCharacterScores.MedicalScore < prbMedicalScore.Maximum && mainCharacterScores.MedicalScore > prbMedicalScore.Minimum)
            {
                prbMedicalScore.Value = mainCharacterScores.MedicalScore;
            }
            if (mainCharacterScores.LifeScore < prbLifeScore.Maximum && mainCharacterScores.LifeScore > prbLifeScore.Minimum)
            {
                prbLifeScore.Value = mainCharacterScores.LifeScore;
            }
            ttMainToolTip.SetToolTip(prbEducationScore, prbEducationScore.Value.ToString());
            ttMainToolTip.SetToolTip(prbHappinessScore, prbHappinessScore.Value.ToString());
            ttMainToolTip.SetToolTip(prbJobScore, prbJobScore.Value.ToString());
            ttMainToolTip.SetToolTip(prbLifeScore, prbLifeScore.Value.ToString());
            ttMainToolTip.SetToolTip(prbMedicalScore, prbMedicalScore.Value.ToString());
        }

        public void checkEducationStatus()
        {
            if(mainCharacter.InEducation == true)
            {
                int age = mainCharacter.Age;
                DateTime mcBirthday = mainCharacter.DateOfBirth;
                DateTime dateOfEvent;
                if (mcBirthday.DayOfYear < 245)
                {
                    //mc is born before 2nd sept therefor events need to be for this september
                    int yearOfEvent = controlClass.InGameDate.Year;
                    int monthOfEvent = 9;
                    int dayOfEvent = 1;
                    dateOfEvent = new DateTime(yearOfEvent, monthOfEvent, dayOfEvent);
                }
                else
                {
                    //mc is born after 2nd sept therfore events need to be for the following september
                    int yearOfEvent = (controlClass.InGameDate.Year) + 1;
                    int monthOfEvent = 9;
                    int dayOfEvent = 1;
                    dateOfEvent = new DateTime(yearOfEvent, monthOfEvent, dayOfEvent);
                }
                //need to get date of 1st september after mcs birthday
                switch (age)
                {
                    case 4:
                        //move into reception
                        string nameOfSchool = genNameOfSchool(1);
                        eventArray[controlClass.NextEvent].Category = "Education";
                        eventArray[controlClass.NextEvent].DateHappened = dateOfEvent;
                        eventArray[controlClass.NextEvent].Description = "Start infant school in Reception. Your parents chose " + nameOfSchool + " for you.";
                        controlClass.NextEvent++;
                        controlClass.EduScorePlus = 100;
                        calculateEducationScore();
                        controlClass.CurrentSchoolYear = 0;
                        break;
                    case 5:
                        //move into yr 1
                        eventArray[controlClass.NextEvent].Category = "Education";
                        eventArray[controlClass.NextEvent].DateHappened = dateOfEvent;
                        eventArray[controlClass.NextEvent].Description = "Move into Year 1 at infant school.";
                        controlClass.NextEvent++;
                        calculateEducationScore();
                        controlClass.CurrentSchoolYear = 1;
                        break;
                    case 6:
                        //move into yr 2
                        eventArray[controlClass.NextEvent].Category = "Education";
                        eventArray[controlClass.NextEvent].DateHappened = dateOfEvent;
                        eventArray[controlClass.NextEvent].Description = "Move into year 2 at infant school. This is your last year there. Your parents are frantically trying to find a place at Junior school for you.";
                        controlClass.NextEvent++;
                        calculateEducationScore();
                        controlClass.CurrentSchoolYear = 2;
                        break;
                    case 7:
                        //move into yr 3(need to pick new school here)
                        string nameOfSchool3 = genNameOfSchool(2);
                        eventArray[controlClass.NextEvent].Category = "Education";
                        eventArray[controlClass.NextEvent].DateHappened = dateOfEvent;
                        eventArray[controlClass.NextEvent].Description = "Start year 3 at Junior school. You make lots of friends very quickly. Your parents managed to find you a place at " + nameOfSchool3 + ".";
                        controlClass.NextEvent++;
                        calculateEducationScore();
                        controlClass.CurrentSchoolYear = 3;
                        break;
                    case 8:
                        //move into yr 4
                        eventArray[controlClass.NextEvent].Category = "Education";
                        eventArray[controlClass.NextEvent].DateHappened = dateOfEvent;
                        eventArray[controlClass.NextEvent].Description = "Move into year 4 at Junior School.";
                        controlClass.NextEvent++;
                        calculateEducationScore();
                        controlClass.CurrentSchoolYear = 4;
                        break;
                    case 9:
                        //move into yr 5
                        eventArray[controlClass.NextEvent].Category = "Education";
                        eventArray[controlClass.NextEvent].DateHappened = dateOfEvent;
                        eventArray[controlClass.NextEvent].Description = "Move into year 5 at Junior School.";
                        controlClass.NextEvent++;
                        calculateEducationScore();
                        controlClass.CurrentSchoolYear = 5;
                        break;
                    case 10:
                        //move into yr 6
                        eventArray[controlClass.NextEvent].Category = "Education";
                        eventArray[controlClass.NextEvent].DateHappened = dateOfEvent;
                        eventArray[controlClass.NextEvent].Description = "Move into year 6 at Junior School. You have SATS exams at the end of this year.";
                        controlClass.NextEvent++;
                        calculateEducationScore();
                        controlClass.CurrentSchoolYear = 6;
                        break;
                    case 11:
                        //move into yr 7
                        string nameOfSchool7 = genNameOfSchool(3);
                        eventArray[controlClass.NextEvent].Category = "Education";
                        eventArray[controlClass.NextEvent].DateHappened = dateOfEvent;
                        eventArray[controlClass.NextEvent].Description = "Start year 7 at Secondary School. You don't make many friends to begin with. You are lucky enough to be attending " + nameOfSchool7 + ", without that persuasive letter your dad sent, this wouldn't have happned.";
                        controlClass.NextEvent++;
                        calculateEducationScore();
                        controlClass.CurrentSchoolYear = 7;
                        break;
                    case 12:
                        //move into yr 8
                        eventArray[controlClass.NextEvent].Category = "Education";
                        eventArray[controlClass.NextEvent].DateHappened = dateOfEvent;
                        eventArray[controlClass.NextEvent].Description = "Start yer 8 at Secondary School. You now have lots of friends..";
                        controlClass.NextEvent++;
                        calculateEducationScore();
                        controlClass.CurrentSchoolYear = 8;
                        break;
                    case 13:
                        //more into yr 9
                        eventArray[controlClass.NextEvent].Category = "Education";
                        eventArray[controlClass.NextEvent].DateHappened = dateOfEvent;
                        eventArray[controlClass.NextEvent].Description = "Start year 9 at Secondary school.";
                        controlClass.NextEvent++;
                        calculateEducationScore();
                        controlClass.CurrentSchoolYear = 9;
                        break;
                    case 14:
                        //move into yr 10
                        eventArray[controlClass.NextEvent].Category = "Education";
                        eventArray[controlClass.NextEvent].DateHappened = dateOfEvent;
                        eventArray[controlClass.NextEvent].Description = "Start year 10 at Secondary school.";
                        controlClass.NextEvent++;
                        calculateEducationScore();
                        controlClass.CurrentSchoolYear = 10;
                        break;
                    case 15:
                        //move into yr 11
                        eventArray[controlClass.NextEvent].Category = "Education";
                        eventArray[controlClass.NextEvent].DateHappened = dateOfEvent;
                        eventArray[controlClass.NextEvent].Description = "Start year 11 at Secondary school. You have GCSE exams at the end of the year.";
                        controlClass.NextEvent++;
                        calculateEducationScore();
                        controlClass.CurrentSchoolYear = 11;
                        break;
                    case 16:
                        //move into yr 12
                        string nameOfSchool12 = genNameOfSchool(4);
                        eventArray[controlClass.NextEvent].Category = "Education";
                        eventArray[controlClass.NextEvent].DateHappened = dateOfEvent;
                        eventArray[controlClass.NextEvent].Description = "Start year 12 at College. Your parents wanted you to take English, Maths and Chemistry but you are taking Drama, music and Physics. You're now attending " + nameOfSchool12 + ".";
                        controlClass.NextEvent++;
                        calculateEducationScore();
                        controlClass.CurrentSchoolYear = 12;
                        break;
                    case 17:
                        //move into yr 13
                        eventArray[controlClass.NextEvent].Category = "Education";
                        eventArray[controlClass.NextEvent].DateHappened = dateOfEvent;
                        eventArray[controlClass.NextEvent].Description = "Start year 13 at College. You have your A-levels at the end of this year.";
                        controlClass.NextEvent++;
                        calculateEducationScore();
                        controlClass.CurrentSchoolYear = 13;
                        break;
                    default:
                        //no change to school situation
                        break;
                }
            }
            
        }
        public void updateEducationPage()
        {
            //first update the events box
            txtEduEducationEvents.ResetText();
            for (int i = 0; i < controlClass.NextEvent; i++)
            {
                if(eventArray[i].Category == "Education")
                {
                    txtEduEducationEvents.Text = txtEduEducationEvents.Text + Environment.NewLine + eventArray[i].DateHappened.ToShortDateString() + " - " + eventArray[i].Description;
                }
                
            }

            //now update the lables
            lblEduCurrentEducationPoints.Text = "Current Education Points: " + mainCharacterScores.EducationScore.ToString();
            lblEduEducationSituation.Text = "Current Education Situation: At school - " + controlClass.NameOfCurrentSchool;
            lblEduCurrentYear.Text = "Current Year: " + controlClass.CurrentSchoolYear.ToString();
        }

        public int randomNumber(int lower, int upper)
        {
            Random rnd = new Random();
            int random = rnd.Next(lower, upper);
            return random;
        }

        public int choiceBox(int typeOfChoice, string opt1, string opt2, string opt3)
        {
            /*
             * Will need to pass the following into this function and into the choice box form.
             * -type of choice (this will determine the header and subheader text)
             * --> using this rather than passing all information in to simplifiy calls to this function.
             * -opt1
             * -opt2
             * -opt3
             */

            //declare vars used for passing data to choice box
            string headerText = "HeaderText";
            string subheaderText = "SubheaderText";
            string opt1header = "Choice 1";
            string opt1body = opt1;
            string opt2header = "Choice 2";
            string opt2body = opt2;
            string opt3header = "Choice 3";
            string opt3body = opt3;


            //first setup the typeOfChoice
            switch (typeOfChoice)
            {
                case 1:
                    //choose infant school
                    headerText = "Infant school";
                    subheaderText = "Select the infant school you want to spend the next 3 years at";
                    break;
                case 2:
                    //choose junior school
                    headerText = "Junior school";
                    subheaderText = "Select the junior school you want to spend the next 4 years at";
                    break;
                case 3:
                    //choose secondary school
                    headerText = "Secondary school";
                    subheaderText = "Select the secondary school you want to spend the next 5 years at";
                    break;
                case 4:
                    //choose college
                    headerText = "College";
                    subheaderText = "Select the college you want to spend the next 2 years at";
                    break;
                default:
                    //no option found
                    break;
            }

            using (var form = new frmChoiceBox(headerText, subheaderText, opt1header, opt1body, opt2header, opt2body, opt3header, opt3body))
            {

                this.Hide();
                var result = form.ShowDialog();

                this.Show();
                if (result == DialogResult.OK)
                {
                    int val = form.ReturnValue1;            //values preserved after close
                    //Do something here with these values

                    //for example
                    
                    return (val);
                }
                
            }
            return 0;
        }

        public string genNameOfSchool(int typeOfSchool)
        {
            Random rnd = new Random();
            //first declare array contianing different names of schools, then randomly generate three to be used.
            string[] namesOfSchools = {"Westwood comprehensive", "Oak Ridge academy", "Eastview Institute", "Westview Institute", "Lone Oak all-through", "Sunset valley school", "Oceanside", "Cape Coral technical school", "Big pine institute", "Mapel Hills academy" };

            //first set the options for return
            string option1 = namesOfSchools[rnd.Next(1, 9)];
            string option2 = namesOfSchools[rnd.Next(1, 9)];
            string option3 = namesOfSchools[rnd.Next(1, 9)];
            int returnedValue = choiceBox(typeOfSchool, option1, option2, option3);
            switch (returnedValue)
            {
                case 1:
                    controlClass.NameOfCurrentSchool = option1;
                    break;
                case 2:
                    controlClass.NameOfCurrentSchool = option2;
                    break;
                case 3:
                    controlClass.NameOfCurrentSchool = option3;
                    break;
            }

            return controlClass.NameOfCurrentSchool;
            
        }

        private void btnEduWithdraw_Click(object sender, EventArgs e)
        {
            mainCharacter.InEducation = false;
            // eventArray[controlClass.NextEvent].Category = "Education";
            //eventArray[controlClass.NextEvent].DateHappened = controlClass.InGameDate;
            //eventArray[controlClass.NextEvent].Description = "You permanently withdrew from education. You said it was too boring.";
            //controlClass.NextEvent++;
            generateEvent("Education", "You permanently withdrew from education. You said it was too boring.", controlClass.InGameDate);
            mainCharacterScores.EducationScore = 0;

        }

        public void lifeEvents()
        {
            switch (mainCharacter.Age)
            {
                case 2:
                    controlClass.EduScorePlus = 12;
                    break;
                case 4:
                    mainCharacter.InEducation = true;
                    break;
            }
        }

        public void calculateEducationScore()
        {
            //Calculate the education score based off of the current score and the modifier
            //1 in 8 chance of the score being decreased rather than being increased. When decreased, needs to go down a fair chunk.

            int randomReturn = randomNumber(1, 30);
            if (randomReturn == 6)
            {
                //Decrease
                mainCharacterScores.EducationScore = mainCharacterScores.EducationScore - controlClass.EduScorePlus;
            }
            else
            {
                //increase 
                //1 in 2 chance of modifier being upped by 0.3 of the current modifier.
                int randomReturn1 = randomNumber(1, 3);
                if(randomReturn1 == 1)
                {
                    //normal modifier
                    mainCharacterScores.EducationScore = mainCharacterScores.EducationScore + controlClass.EduScorePlus;
                }
                else if (randomReturn1 == 2)
                {
                    //increased modifier
                    int plus = (int)Math.Round(controlClass.EduScorePlus * 0.3);
                    mainCharacterScores.EducationScore = mainCharacterScores.EducationScore + plus;
                }
            }

        }

        public void generateEvent(string category, string content, DateTime date)
        {
            eventArray[controlClass.NextEvent].Category = category;
            eventArray[controlClass.NextEvent].DateHappened = date;
            eventArray[controlClass.NextEvent].Description = content;
            controlClass.NextEvent++;
        }

        public void updateHappinessScore(int externalModifier)
        {
            //add to the current happiness score the generic value to increse by plus a modifier
            mainCharacterScores.HappinessScore = mainCharacterScores.HappinessScore + controlClass.HappinessScorePlus + externalModifier;
            
        }
        
        public void disableClickables()
        {
            //procedure to disable all clickable elements on this form, for use when the main char dies.
            //There is no way to reverse this.

            btnAgeUp.Enabled = false;
            btnEduWithdraw.Enabled = false;
            btnEndLife.Enabled = false;
            btnJobApplyForJob.Enabled = false;
            btnJobQuitJob.Enabled = false;
        }

        private void btnCriCommitCrime_Click(object sender, EventArgs e)
        {
            //first generate new crime then time of community service for it. 

            string[] crimes = { "Burgle a shop", "Steal a car", "Pirate a DVD" };
            int crimesLength = crimes.Length;
            int rand = randomNumber(0, crimesLength);
            int years = randomNumber(1, 12);
            string crimeText = "You committed a crime " + crimes[rand] + " and you recieved " + years.ToString() + " years community service.";
            generateEvent("Crime", crimes[rand], controlClass.InGameDate);
            updateCrimes();
        }

        public void updateCrimes()
        {
            txtCriCrimeEvents.ResetText();
            for (int i = 0; i < controlClass.NextEvent; i++)
            {
                if (eventArray[i].Category == "Crime")
                {
                    txtCriCrimeEvents.Text = txtCriCrimeEvents.Text + Environment.NewLine + eventArray[i].DateHappened.ToShortDateString() + " - " + eventArray[i].Description;
                }

            }
        }

        public void yearlyCheck()
        {
            //this is a place which will run every time the mc ages up. If they are older than the specified age then will run code in relevant section.

            if(mainCharacter.Age >= 13)
            {
                //'unlock' character customisation screen.
                txtEditFirstName.Enabled = true;
                txtEditLastName.Enabled = true;
                cboEditGender.Enabled = true;
                cboEditSexuality.Enabled = true;
                cboEditEyeColour.Enabled = true;
                cboEditHairColour.Enabled = true;
            }
            if (mainCharacter.Age >= 18)
            {
                //'unlock' character job options
                btnJobApplyForJob.Enabled = true;
                btnJobQuitJob.Enabled = true;
                btnJobReshuffleJobBoard.Enabled = true;
            }
        }

        public void updateCharCustomisationOptions()
        {
            //fill all the Character Customisation option controls with current information from main char object
            txtEditFirstName.Text = mainCharacter.FirstName;
            txtEditLastName.Text = mainCharacter.LastName;
            cboEditGender.Text = mainCharacter.Gender;
            cboEditSexuality.Text = mainCharacter.Sexuality;
            cboEditHairColour.Text = mainCharacter.HairColour;
            cboEditEyeColour.Text = mainCharacter.EyeColour;
        }

        public void saveCharCustomisationOptions()
        {
            //if the contents of the text boxes etc is NOT null and is different to the current contents of the main character object, then it needs to be written into the object

            //start with the first name
            if(txtEditFirstName.Text != "" && txtEditFirstName.Text != mainCharacter.FirstName)
            {
                mainCharacter.FirstName = txtEditFirstName.Text;
            }
            //next - do the last name
            if(txtEditLastName.Text != "" && txtEditLastName.Text != mainCharacter.LastName)
            {
                mainCharacter.LastName = txtEditLastName.Text;
            }
            if(cboEditGender.Text != "" && cboEditGender.Text != mainCharacter.Gender)
            {
                mainCharacter.Gender = cboEditGender.Text;
            }
            if(cboEditSexuality.Text != "" && cboEditSexuality.Text != mainCharacter.Sexuality)
            {
                mainCharacter.Sexuality = cboEditSexuality.Text;
            }
            if(cboEditHairColour.Text != "" && cboEditHairColour.Text != mainCharacter.HairColour)
            {
                mainCharacter.HairColour = cboEditHairColour.Text;
            }
            if(cboEditEyeColour.Text != "" && cboEditEyeColour.Text != mainCharacter.EyeColour)
            {
                mainCharacter.EyeColour = cboEditEyeColour.Text;
            }
        }

        private void btnEditSaveChanges_Click(object sender, EventArgs e)
        {
            saveCharCustomisationOptions();
            populateForms();
        }

        private void btnEndLife_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to end this life?", "End life", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                killMainCharacter();
            }
            else if (dialogResult == DialogResult.No)
            {
                //do something else
            }

        }

        private void btnJobReshuffleJobBoard_Click(object sender, EventArgs e)
        {
            if(mainCharacter.JobCurrentDuration <= 5)
            {
                //can only get band 1 job
                reshuffleJobs(1);
            }
            else if (mainCharacter.JobCurrentDuration >=6 && mainCharacter.JobCurrentDuration <= 14)
            {
                //get band 2 job
                reshuffleJobs(2);
            }
            else if (mainCharacter.JobCurrentDuration >= 15 && mainCharacter.JobCurrentDuration <= 26)
            {
                //get band 3 job
                reshuffleJobs(3);
            }
            else if (mainCharacter.JobCurrentDuration >= 27 && mainCharacter.JobCurrentDuration <= 44)
            {
                //get band 4 job
                reshuffleJobs(4);
            }
            else if (mainCharacter.JobCurrentDuration >= 45)
            {
                //get band 5 job
                reshuffleJobs(5);
            }
        }

        public void reshuffleJobs(int band)
        {
            //first generate the jobs
            int maxNumb = 10;
            for (int i = 0; i < maxNumb; i++)
            {
                Job tempJob = new Job(); //make new instance of the object
                tempJob.generateJob(band); 
                availableJobs[i] = tempJob; //transfer the temp object into the array.
            }

            //now populate data grid view
            //first have to clear it
            dgvEmpAvailableJobs.Rows.Clear();
            dgvEmpAvailableJobs.Refresh();

            //use code from openldbws app to populate dgvEmpAvailableJobs

            //first, set var for which column contains which data
            int jtCol = 0;
            int salCol = 1;
            int bandCol = 2;

            //now loop through familyArray and populate dgvEmpAvailableJobs
            //as jobs are only ever generated and populated here - can use max number from job generation loop for loop here

            for (int x = 0; x < maxNumb; x++)
            {
                if (availableJobs[x].JobTitle != null)
                {
                    int i = dgvEmpAvailableJobs.Rows.Add();
                    dgvEmpAvailableJobs.Rows[i].Cells[jtCol].Value = availableJobs[x].JobTitle;
                    dgvEmpAvailableJobs.Rows[i].Cells[salCol].Value = availableJobs[x].Salary;
                    dgvEmpAvailableJobs.Rows[i].Cells[bandCol].Value = availableJobs[x].Band;
                }
            }
        }
        public void updateCurrentJobInformation()
        {
            lblJobCurrentJobBand.Text = "Current Job Band: " + mainCharacter.JobBand.ToString(); ;
            lblJobCurrentJobTitle.Text = "Current Job Title: " + mainCharacter.JobTitle;
            lblJobCurrentJobSalary.Text = "Current Job Salary: " + mainCharacter.JobSalary.ToString();
            lblJobCurrentJobDuration.Text = "Current Job Duration: " + mainCharacter.JobCurrentDuration.ToString();
            mainCharacterScores.JobScore = mainCharacter.JobCurrentDuration*4;
            prbJobScore.Value = mainCharacterScores.JobScore;
        }

        private void btnJobApplyForJob_Click(object sender, EventArgs e)
        {
            try
            {
                mainCharacter.JobTitle = dgvEmpAvailableJobs.SelectedCells[0].Value.ToString();
                mainCharacter.JobSalary = Int32.Parse(dgvEmpAvailableJobs.SelectedCells[1].Value.ToString());
                mainCharacter.JobBand = Int32.Parse(dgvEmpAvailableJobs.SelectedCells[2].Value.ToString());
                mainCharacter.InJob = true;
                generateEvent("Job", "You applied for and got the job: " + mainCharacter.JobTitle, controlClass.InGameDate);
                updateCurrentJobInformation();
            } catch(Exception)
            {
                MessageBox.Show("Please select a job before applying for it.");
            }

            
        }

        private void button1_Click(object sender, EventArgs e) //quit job
        {
            try
            {
                mainCharacter.JobTitle = "";
                mainCharacter.JobSalary = 0;
                mainCharacter.JobBand = 0;
                mainCharacter.InJob = false;
                mainCharacter.JobCurrentDuration = 0;
                generateEvent("Job", "You quit your job", controlClass.InGameDate);
            }
            catch (Exception)
            {
                MessageBox.Show("Please make sure you have a job before you try to quit it.");
            }
            updateCurrentJobInformation();
        }

        public void calculateSalary()
        {
            Random rnd = new Random();
            //algorithm to generate the salary of the main character
            //needs to take into consideration the current band of job, base band of that salary and how long the mc has been in that job
            int salary = mainCharacter.JobSalary;
            int band = mainCharacter.JobBand;
            int duration = mainCharacter.JobCurrentDuration;

            if(mainCharacter.InJob == true)
            {
                //mc is in job so can now calc salary.
                int increaseAmount = rnd.Next(0, (band*duration)*50);
                mainCharacter.JobSalary = salary + increaseAmount;
            }
            
        }

        public void generateMedicalScore()
        {
            Random rnd = new Random();
            int currentMedScore = mainCharacterScores.MedicalScore;
            if(currentMedScore == 100)
            {
                //has perfect health, 1/10 chance of it being lowered so can then be diseased.
                int chance = rnd.Next(0, 11);
                if(chance == 5)
                {
                    currentMedScore = 99;
                }

            }
            if(currentMedScore < 100)
            {
                //can be diseased. 1 in 20 chance of getting disease
                int chance1 = rnd.Next(0, 21);
                if(chance1 == 12)
                {
                    //is gonna be diseased
                    string condition = getMedicalCondition();

                    generateEvent("Medical","You " + condition, controlClass.InGameDate);

                    mainCharacterScores.MedicalScore = mainCharacterScores.MedicalScore - rnd.Next(0, 50);

                }
            }
        }

        public string getMedicalCondition()
        {
            string[] medicalConditions = {"broke your arm", "broke your leg", "stubbed your toe" };
            Random rnd = new Random();

            string returnString = medicalConditions[rnd.Next(0, medicalConditions.Length)];
            return returnString;
        }

        private void btnParGenMore_Click(object sender, EventArgs e)
        {
            generateAndPopulateMorePartners();
        }

        public void generateAndPopulateMorePartners()
        {
            //Now create family members
            for (int i = 0; i < 10; i++)
            {
                Partner tempFamily = new Partner(); 
                tempFamily.generateDetailedChar(mainCharacter.Gender, mainCharacter.Sexuality, controlClass.InGameDate); 

                //add to the array
                potentialPartners[i] = tempFamily;
            }
            //now fill dgv. Code taken from dgvFamily

            //first have to clear it
            dgvPartPotential.Rows.Clear();
            dgvPartPotential.Refresh();


            //use code from openldbws app to populate dgvFamily

            //first, set var for which column contains which data
            int firstNameCol = 0;
            int lastNameCol = 1;
            int genderCol = 2;
            int dobCol = 3;
            int indexCol = 4;

            //now loop through familyArray and populate dgvFamily
            // can use a for loop as number of family members are in the controlClass

            for (int x = 0; x < 10; x++)
            {
                
                int i = dgvPartPotential.Rows.Add();
                dgvPartPotential.Rows[i].Cells[firstNameCol].Value = potentialPartners[x].FirstName;
                dgvPartPotential.Rows[i].Cells[lastNameCol].Value = potentialPartners[x].LastName;
                dgvPartPotential.Rows[i].Cells[genderCol].Value = potentialPartners[x].Gender;
                dgvPartPotential.Rows[i].Cells[dobCol].Value = potentialPartners[x].DateOfBirth.ToShortDateString();
                dgvPartPotential.Rows[i].Cells[indexCol].Value = x;

            }
        }

        private void btnAskOnDate_Click(object sender, EventArgs e)
        {
            //basically the same as applying for a job
            try 
            {
                int index = Int32.Parse(dgvPartPotential.SelectedCells[4].Value.ToString());
                partner = potentialPartners[index];
                MessageBox.Show(partner.FirstName);
                generateEvent("Relationships", "You asked " + partner.FirstName + " on a date and they said yes!", controlClass.InGameDate);
                partner.WhenMet = controlClass.InGameDate;
                mainCharacter.InRelationship = true;
            }

            catch(Exception)
            {
                MessageBox.Show("Please select a partner before trying to go on a date with them.");
            }
            updatePartnerInformation();
        }

        public void updatePartnerInformation()
        {
            if (mainCharacter.InRelationship == true)
            {
                lblPartFirstName.Text = "First Name: " + partner.FirstName;
                lblPartLastName.Text = "Last Name: " + partner.LastName;
                lblPartDateOfBirth.Text = "Date of Birth: " + partner.DateOfBirth.ToShortDateString();
                lblPartAge.Text = "Age: " + partner.Age.ToString();
            }
            else
            {
                lblPartFirstName.Text = "First Name: ";
                lblPartLastName.Text = "Last Name: ";
                lblPartDateOfBirth.Text = "Date of Birth: ";
                lblPartAge.Text = "Age: ";
            }
        }

        private void btnPartDump_Click(object sender, EventArgs e)
        {
            mainCharacter.InRelationship = false;
            partner.FirstName = null;
            partner.LastName = null;
            partner.Sexuality = null;
            partner.Gender = null;
            partner.LivingStatus = false;
            partner.Age = 0;
            updatePartnerInformation();
        }
    }
}

