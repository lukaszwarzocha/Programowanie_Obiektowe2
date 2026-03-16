using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Labolatorium2.Zadania
{
    internal class Student
    {
        private string firstname;
        private string lastname;
        private List<int> grades;

        public Student(string firstname, string lastname)
        {
            this.firstname = firstname;
            this.lastname = lastname;
            grades = new List<int>();
        }

        public void DodajOcene(int grades)
        {
            if (grades > 0 && grades <= 6)
            {

                this.grades.Add(grades);
            }
            else
            {
                Console.WriteLine("Ocena musi byc w przedziale 1-6");
            }
        }

        public void SredniaOcen()
        {
            if (this.grades.Count == 0)
            {
                Console.WriteLine("Brak ocen dla "+firstname+" "+lastname);
                return;
            }

            double suma = 0;
            double srednia = 0;

            foreach (int grade in grades)
            {
                suma += grade;
                srednia = suma / this.grades.Count;
            }
            Console.WriteLine("Srednia ocen " + firstname + ", " + lastname + " wynosi " + srednia);
        }
    }
}
