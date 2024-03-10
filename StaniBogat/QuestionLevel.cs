using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace StaniBogat
{
    internal class QuestionLevel
    {
        public int group_number {  get; set; }

        public string difficulty { get; set; }

        public List<Question> questions {  get; set; }
    }
}
