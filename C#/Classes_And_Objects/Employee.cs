using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Classes_And_Objects
{
    abstract class Employee
    {
        uint _id;
        public  string name { get; protected set; }
        uint _age;
        uint _contact;

        public uint id
        {
            set
            {
                if (value <= 0)
                {
                    Console.WriteLine("Invalid Entre");
                }
                else
                {
                    this._id = value;
                }
            }
            get
            {
                return this._id;
            }
        }

        public uint age
        {
            set
            {
                if (value < 18)
                {
                    Console.WriteLine("Invalid Entre");
                }
                else
                {
                    this._age = value;
                }
            }
            get
            {
                return this._age;
            }
        }
        public uint contact
        {
            set
            {
                this._contact = value;
            }
            get
            {
                return this._contact;
            }
        }

        public virtual void Insert()
        {
            Console.WriteLine("Enter id : ");
            id = uint.Parse(Console.ReadLine());
            Console.WriteLine("Enter Name :");
            name = Console.ReadLine();
            Console.WriteLine("Enter Age : ");
            age = uint.Parse(Console.ReadLine());
            Console.WriteLine("Enter Contact :");
            contact = uint.Parse(Console.ReadLine());

        }

        public void Get_emp()
        {
            Console.WriteLine(" ID : {0} ", id);
            Console.WriteLine(" Name : {0}", name);
            Console.WriteLine(" Age : {0}", age);
            Console.WriteLine(" Contact : {0}", contact);


        }
    }

    class Teacher : Employee 
    {
        public string masterin;
        public string designaation { get;  set; }
        

        public override void Insert() 
        {
            base.Insert();
            Console.WriteLine("Enter Master In :");
            masterin = Console.ReadLine();
            Console.WriteLine("Enter designaation : ");
            designaation = Console.ReadLine();

        }
    }
   
}
