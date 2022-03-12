using System;
using System.Collections.Generic;
using System.Text;

namespace A_level_Computer_Science_Project
{
    public class Partner : DetailedCharacter
    {
        Random rnd = new Random();
        private string whereMet;
        private DateTime whenMet;

        //get and set
        public string WhereMet { get; set; }
        public DateTime WhenMet { get; set; }

        public Partner generateDetailedChar(string gender, string sexuality, DateTime today)
        {
            Partner potential = new Partner();

            if (gender == "Female")
            {
                if (sexuality == "Straight")
                {
                    //generate a man
                    FirstName = genMFN();
                    LastName = genLN();
                    Sexuality = "Straight";
                    Gender = "Male";
                    LivingStatus = true;
                    DateOfBirth = today.AddDays(-rnd.Next(5840, 14600));
                    Age = calcAge(DateOfBirth);
                }else if (sexuality == "Homosexual")
                {
                    //generate woman
                    FirstName = genFFN();
                    LastName = genLN();
                    Sexuality = "Homosexual";
                    Gender = "Female";
                    LivingStatus = true;
                    DateOfBirth = today.AddDays(-rnd.Next(5840, 14600));
                    Age = calcAge(DateOfBirth);
                }
            }
            if (gender == "Male")
            {
                if (sexuality == "Homosexual")
                {
                    //generate a man
                    FirstName = genMFN();
                    LastName = genLN();
                    Sexuality = "Homosexual";
                    Gender = "Male";
                    LivingStatus = true;
                    DateOfBirth = today.AddDays(-rnd.Next(5840, 14600));
                    Age = calcAge(DateOfBirth);
                }
                else if (sexuality == "Straight")
                {
                    //generate woman
                    FirstName = genFFN();
                    LastName = genLN();
                    Sexuality = "Straight";
                    Gender = "Female";
                    LivingStatus = true;
                    DateOfBirth = today.AddDays(-rnd.Next(5840, 14600));
                    Age = calcAge(DateOfBirth);
                }
            }


            return potential;
        }

    }
}
