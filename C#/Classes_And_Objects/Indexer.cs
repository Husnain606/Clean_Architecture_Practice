using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes_And_Objects
{
    class Employes 
    {
        int[] contact = new int[3];
        public int this[int index]
        {
            set
            {
                if (index >= 0 && index < contact.Length)
                {
                    if (value > 0)
                    {
                        contact[index] = value;
                    }
                    else
                    {
                        Console.WriteLine(" Invalid value");
                    }
                }
                else 
                {
                    Console.WriteLine(  "Invalid index");
                }
            }
            get 
            {
                return contact[index];
            }
            
        }
    }
     class Indexer
    {
        //static void Main(string[] args)
        //{
        //    Employes []emp = new Employes[5];
        //    emp[0] = new Employes();
        //    emp[0][1] = 25;
        //    Console.WriteLine(emp[0][1]);
        //}

    }
}
