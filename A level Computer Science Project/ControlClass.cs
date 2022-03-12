using System;
using System.Collections.Generic;
using System.Text;

namespace A_level_Computer_Science_Project
{
    public class ControlClass
    {
        //private attributes
        private DateTime inGameDate;
        private int numberOfEvents;
        private int nextEvent;
        private string filepath;
        private int numberOfFamily;
        private int nextFamily;
        private string nameOfCurrentSchool;
        private int currentSchoolYear;
        private int eduScorePlus;
        private int happinessScorePlus;


        //get and sets
        //GENERIC GAME STUFF----
        public DateTime InGameDate { get; set; }
        public string Filepath { get; set; }
        //EVENTS-------
        public int NumberOfEvents { get; set; }
        public int NextEvent { get; set; }
        //FAMILY--------
        public int NumberOfFamily { get; set; }
        public int NextFamily { get; set; }
        //EDUCATION------
        public string NameOfCurrentSchool { get; set; }
        public int CurrentSchoolYear { get; set; }
        public int EduScorePlus { get; set; }

        //HAPPINESS SCORE-----
        public int HappinessScorePlus { get; set; }

    }
}
