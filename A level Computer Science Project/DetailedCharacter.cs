using System;
using System.Collections.Generic;
using System.Text;

namespace A_level_Computer_Science_Project
{
    public class DetailedCharacter : Person
    {
        Random rand = new Random();

        private bool inEducation;
        private string educationSituation;
        private bool inJob;
        private string hairColour;
        private string eyeColour;
        
        //get and set
        public bool InEducation { get; set; }
        public string EducationSituation { get; set; }
        public bool InJob { get; set; }
        public string HairColour { get; set; }
        public string EyeColour { get; set; }

        

    }
}


