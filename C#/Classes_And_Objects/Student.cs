namespace Classes_And_Objects
{
    class Student
    {
        public int rollno;
        public string name;
        public int age;
        public string fname ;
        public static string schName;
        public static int fees;
        // Static Cconstructor
        static Student() 
        {
            schName = "abc school";
            fees = 5000;
        }
        // Copy Constructor
        public Student(Student std)
        {
            this.rollno=std.rollno;
            this.name=std.name;
            this.age=std.age;
            this.fname=std.fname;
        }
        public void Insert_std() 
        {
            Console.WriteLine("Enter Roll Number : ");
            rollno = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter Name :");
            name = Console.ReadLine();
            Console.WriteLine("Enter Age : ");
            age = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter FName :");
            fname = Console.ReadLine();


        }
        public static int get_fees() 
        {
            return fees;
        }
        public static int get_inr_fees(int fee)
        {
            return fee +( fee / 10);
        }
        public void Get_Std()
        {
            Console.WriteLine(" Roll Number : {0} ",rollno);
            Console.WriteLine(" Name : {0}",name);
            Console.WriteLine(" Age : {0}",age);
            Console.WriteLine(" FName : {0}",fname);
            Console.WriteLine(" School Name : {0}", Student.schName);
            Console.WriteLine(" Fees : {0}", Student.fees );
            Console.WriteLine(" Incremented Fees : {0}", Student.get_inr_fees(Student.fees));
        }

      
    }

}