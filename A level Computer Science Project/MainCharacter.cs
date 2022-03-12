using System;
using System.Collections.Generic;
using System.Text;

namespace A_level_Computer_Science_Project
{
    public class MainCharacter : DetailedCharacter
    {
        //attributes

        private int expenses;
        private int bankBalance;
        private int jobBand;
        private string jobTitle;
        private int jobSalary;
        private int jobCurrentDuration;
        private int salary;

        //get and set
        public int Expenses { get; set; }
        public int BankBalance { get; set; }
        public int JobBand { get; set; }
        public string JobTitle { get; set; }
        public int JobSalary { get; set; }
        public int JobCurrentDuration { get; set; }
    }
}
