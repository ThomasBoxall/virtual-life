using System;
using System.Collections.Generic;
using System.Text;


namespace A_level_Computer_Science_Project
{
    public class GenericFamilyMember : Person
    {
        

        private string relationshipToMain;

        //get and set
        public string RelationshipToMain { get; set; }
        

        //constructor
        public GenericFamilyMember populateGenericFamilyMember(int charNumber, MainCharacter mainCharacter)
        {
            //construct here
            //go through a swtich statement to set the character based on what their relationship to the main is.
            //setup random number generation
            Random rnd = new Random();
            GenericFamilyMember familyMember = new GenericFamilyMember();
            switch (charNumber)
            {
                case 0:
                    //Dad to main character
                    RelationshipToMain = "Father";
                    FirstName = genMFN();
                    LastName = mainCharacter.LastName;
                    Gender = "Male";
                    Sexuality = "Straight";
                    DateOfBirth = genParentAge(mainCharacter.DateOfBirth);
                    LivingStatus = true;
                    InRelationship = true;
                    Age = calcAge(DateOfBirth);
                    break;
                case 1:
                    //mum to main character
                    RelationshipToMain = "Mother";
                    FirstName = genFFN();
                    switch (rnd.Next(1, 3))
                    {
                        case 1:
                            //same last name as father and main character
                            LastName = mainCharacter.LastName;
                            break;
                        case 2:
                        default:
                            //different last name to father and main character - need to randomGen this
                            LastName = genLN();
                            break;
                    }
                    Gender = "Female";
                    Sexuality = "Straight";
                    DateOfBirth = genParentAge(mainCharacter.DateOfBirth);
                    LivingStatus = true;
                    InRelationship = true;
                    Age = calcAge(DateOfBirth);
                    break;
                default:
                    FirstName = null;
                    break;
            }
            return familyMember;
        }

        static DateTime genParentAge(DateTime mcDOB)
        {
            //need to generate a date between 16 and 30 years ago from main character birthday.
            DateTime dateOfBirth;
            Random rand = new Random();
            int randomNumber = rand.Next(5845, 10957);
            dateOfBirth = mcDOB.AddDays(-randomNumber);


            return dateOfBirth;
        }

        

    }
}
