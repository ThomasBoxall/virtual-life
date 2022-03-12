using System;
using System.Collections.Generic;
using System.Text;

namespace A_level_Computer_Science_Project
{
    public class Event
    {
        private DateTime dateHappened;
        private string description;
        private string category;

        //get and set
        public DateTime DateHappened { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
    }
}
