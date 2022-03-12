using System;
using System.Collections.Generic;
using System.Text;

namespace A_level_Computer_Science_Project
{
    public class Person
    {
        //Set all private attributes for the class
        private string firstName;
        private string lastName;
        private string gender;
        private string sexuality;
        private int age;
        private bool livingStatus;
        private DateTime dateOfBirth;
        private DateTime dateOfDeath;
        private string reasonForDeath;
        private bool inRelationship;
        //get and set methods (nb. use uppercase first character for accessable get/set)
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Sexuality { get; set; }
        public int Age { get; set; }
        public bool LivingStatus { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime DateOfDeath { get; set; }
        public string ReasonForDeath { get; set; }
        public bool InRelationship { get; set; }


        //functions used in constructors
        public static string genLN()
        {
            //Generate female first name
            Random rnd = new Random();
            string[] name = { "Smith", "Brown", "Wilson", "Thomson", "Robertson", "Campbell", "Stewart", "Macdonald", "Murray", "Reid", "Taylor", "Clark", "Mitchell", "Ross", "Watson", "Miller", "Gray", "Simpson", "Duncan", "Bell", "Grant", "Mackenzie", "Allan", "Wood", "Muir", "Watt", "King", "Bruce", "Boyle", "Douglas" };
            string returnName = name[rnd.Next(1, 29)];
            return returnName;
        }
        public static string genMFN()
        {
            //generate male first name
            Random rnd = new Random();
            string[] name = { "Liam", "Noah", "Oliver", "Elijah", "William", "James", "Benjamin", "Ben", "Lucas", "Henry", "Alex", "Ethan", "Daniel", "Sebstian", "Jack", "Matt", "John", "Joe", "David", "Josh", "Julien", "Leo", "Isaac", "Thomas", "Max", "Andy", "Phill", "Harvey", "Ryan" };
            string returnName = name[rnd.Next(1, 29)];
            return returnName;
        }

        public static string genFFN()
        {
            //Generate female first name
            Random rnd = new Random();
            string[] name = { "Olivia", "Sophia", "Maria", "Mia", "Evelyn", "Jess", "Ella", "Zoe", "Jemma", "Gemma", "Emily", "Nuala", "Maggie", "Ciara", "Scarlett", "Layla", "Chloe", "Ellie", "Hazel", "Lucy", "Niamh", "Kat", "Victoria", "Lily", "Hannah", "Chloe", "Lara", "Bella", "Ruby" };
            string returnName = name[rnd.Next(1, 29)];
            return returnName;
        }

        public int calcAge(DateTime input)
        {
            int difference = (DateTime.Today.Date - input.Date).Days;
            float toround = (difference / 365);
            int age = ((int)toround);

            return age;
        }
    }

}
