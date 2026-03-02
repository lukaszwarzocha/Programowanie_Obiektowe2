using System;
using System.Collections.Generic;
using System.Text;

namespace Labolatorium1.Zadania
{
    internal class Zad3
    {
        public void Run()
        {

            for (int i = 20; i > 0; i--)
            {
                if (i == 19 || i == 15 || i == 9 || i == 6 || i == 2) { continue; }
                Console.WriteLine(i);
            }
        }
    }
}
