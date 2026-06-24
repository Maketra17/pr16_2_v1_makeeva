using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _16_1_maakeeva
{
    internal class Student
    {
        public string famil { get; set; }//фамилия
        public string Group { get; set; }//группа
        public string Specialty { get; set; }//специальность
        public string Subject1 { get; set; }//предмет 1
        public int Grade1 { get; set; }//оценка 1
        public string Subject2 { get; set; }//предмет 2
        public int Grade2 { get; set; }//оценка 2
        public string Subject3 { get; set; }//предмет 3
        public int Grade3 { get; set; }//оценка 3

        public Student(string surname, string group, string specialty)
        {
            famil = surname;
            Group = group;
            Specialty = specialty;
            Subject1 = "птпм";
            Grade1 = 0;
            Subject2 = "рпм";
            Grade2 = 0;
            Subject3 = "уп";
            Grade3 = 0;
        }

        public Student(string surname, string group, string specialty,
                       string subj1, int gr1, string subj2, int gr2, string subj3, int gr3)
        {
            famil = surname;
            Group = group;
            Specialty = specialty;
            Subject1 = subj1;
            Grade1 = gr1;
            Subject2 = subj2;
            Grade2 = gr2;
            Subject3 = subj3;
            Grade3 = gr3;
        }
    }
}
