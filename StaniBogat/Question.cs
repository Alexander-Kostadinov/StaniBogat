using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace StaniBogat
{
    public class Question
    {
        public string question { get; set; }

        public string correct_answer { get; set; }

        public List<string> wrong_answers { get; set; }

        public Question() 
        {
            wrong_answers = new List<string>();
        }
    }
}
