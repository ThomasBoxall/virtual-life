using System;
using System.Collections.Generic;
using System.Text;

namespace A_level_Computer_Science_Project
{
    public class Job
    {
        private string jobTitle;
        private int salary;
        private int band;

        public string JobTitle { get; set; }
        public int Salary { get; set; }
        public int Band { get; set; }

        public Job generateJob (int band)
        {
            string[] band1Job = { "Cleaner", "Wearhouse opperative", "Grocery picker", "Sahara delivery driver", "Burger flipper", "Hospital porter", "Community care worker", "Care assistant", "Handyperson", "Nursery assistant" };
            string[] band2Job = { "College security guard", "Pet groomer", "Engineer", "Secretary", "Line cook", "Plumber", "Cleaning company owner", "HR advisor", "Vehicle Technician", "Tanker driver" };
            string[] band3Jobs = { "Supervisor", "Senior Vehicle Technician", "Head of Resources", "Product manager", "Web developer", "Comms manager", "Software engineer", "Project manager", "Site manager", "Sales Director" };
            string[] band4Jobs = { "Store manager", "Finance Manager", "Site supervisor", "Nursing home manager", "IT Solutions Architect", "Senior API Developer", "Electrical Installation Lecturer", "Matron", "Defects manager", "Family Lawyer" };
            string[] band5Jobs = { "CEO", "Retail park manager", "Hotel manager", "Specialty doctor", "User interface designer", "SQL Developer", "DevOps engineer", "Senior Software consultant", "Executive head teacher", "Hospital director" };
            Job newJob = new Job();
            Random rnd = new Random();
            switch (band)
            {
                case 1:
                    Salary = 17000;
                    Band = 1;
                    JobTitle = band1Job[rnd.Next(1, 9)];
                    break;
                case 2:
                    Salary = 34000;
                    Band = 2;
                    JobTitle = band2Job[rnd.Next(1, 9)];
                    break;
                case 3:
                    Salary = 51000;
                    Band = 3;
                    jobTitle = band3Jobs[rnd.Next(1, 9)];
                    break;

                case 4:
                    Salary = 68000;
                    Band = 4;
                    jobTitle = band4Jobs[rnd.Next(1, 9)];
                    break;
                case 5:
                    Salary = 150000;
                    Band = 5;
                    jobTitle = band5Jobs[rnd.Next(1, 9)];
                    break;
            }
            return newJob;
        }
    }
}
