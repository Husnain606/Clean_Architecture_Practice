using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Classes_And_Objects
{
    interface IAccount
    {
        public void InsertDetails();
        public void Displaydetails();

    }

    class SavingAccount : IAccount
    {
        int savingammount;
        string sname;
        public void Displaydetails()
        {
            Console.WriteLine(" savingammount : {0} ", savingammount);
            Console.WriteLine(" sname : {0} ", sname);
        }

        public void InsertDetails()
        {
            Console.WriteLine(  "Enter Saving Ammount : ");
            savingammount =int.Parse( Console.ReadLine());

            Console.WriteLine("Enter Account Name : ");
            sname = Console.ReadLine();
        }
    }
    class CurrentAccount : IAccount
    {
        int salary;
        string comapny;
            
        public void Displaydetails()
        {
            Console.WriteLine(" Salary : {0} ", salary);
            Console.WriteLine(" Comapny : {0} ", comapny);
        }
        public void InsertDetails()
        {
            Console.WriteLine("Enter Salary : ");
            salary = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter Company Name : ");
            comapny = Console.ReadLine();
        }
    }
    class Account
    {
        //public IAccount acount { get; set; }
        //public Account(IAccount account)
        //{
        //    this.acount = account;
        //}
        public void InsertDetails(IAccount acount)
        {
            acount.InsertDetails();
        }
        public void Displaydetails(IAccount acount) 
        {
            acount.Displaydetails();
        }

    }
    internal class DependencyInjection
    {
        static void Main(string[] args)
        {

            //  CONSTRUCTER  Injection
            //IAccount CA = new CurrentAccount();
            //Account account1 = new Account(CA);
            //account1.InsertDetails();
            //account1.Displaydetails();

            //IAccount SA = new SavingAccount();
            //Account account2 = new Account(SA);
            //account2.InsertDetails();
            //account2.Displaydetails();

            //  PROPERTY  Injection
            //Account ca = new Account();
            //ca.acount=new CurrentAccount();
            //ca.InsertDetails();
            //ca.Displaydetails();

            // Method Injection
            Account a = new Account();
            IAccount CA = new CurrentAccount();
            a.InsertDetails(CA);
            a.Displaydetails(CA);
        }  
    }
}
