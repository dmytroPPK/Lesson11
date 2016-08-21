using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson11
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.InputEncoding = Encoding.Unicode;
            Console.OutputEncoding = Encoding.Unicode;
            Program prog = new Program();
            prog.Prompt();
            MyEventArgs arg = new MyEventArgs {
                FileName = "file.txt",
            };
            prog.WriteData(arg);
            //Delay
            Console.ReadKey();
        }


        // Prop  and fields
        public event EventHandler<MyEventArgs> write;
        protected string @string;
        protected double number;
        protected bool isNumber;

        // method
        private void WriteData(MyEventArgs args)
        {
            this.OnWrite(this,args);
        }
        protected virtual void OnWrite(object o, MyEventArgs args)
        {
            if (write != null)
            {
                write.Invoke(o,args);
            }
            else
            {
                throw new NullReferenceException("does not have subscribers");
            }
        }

        public void Prompt()
        {
            Console.Write("Hi dude. Enter either number or string please: ");
            this.@string = Console.ReadLine();
            this.ValidateString(this.@string);
            Console.WriteLine((this.isNumber)?"You entered a number":"You entered a string");
            this.Menu();
        }

        private void Menu()
        {
            string typeDataMsg = (this.isNumber) ?"number":"string";
            string msg = "1. Write your "+typeDataMsg+" to file\n2. Write your "+typeDataMsg+" to Console";
            Console.WriteLine(msg);
            string flagMenu = null;
            while (true)
            {
                Console.Write("\t =>> ");
                flagMenu = Console.ReadLine();
                switch (flagMenu)
                {
                    case "1":
                        this.SubscribeOnEvent("file");
                        return;
                        
                    case "2":
                        this.SubscribeOnEvent("console");
                        return;
                    default:
                        Console.WriteLine("Wrong point of menu");
                        continue;
                }
            }

        }

        protected void SubscribeOnEvent(string flag)
        {
            if(flag == "console")
            {
                string dataToConsole = (this.isNumber) ? this.number.ToString() :this.@string;
                // Event
                write += (o, args) => Console.WriteLine("OUTPUT: "+ dataToConsole);
            }
            if (flag == "file")
            {
                // Event
                write += (o, args) => WriteFile(fileName : args.FileName);
            }
        }

        private void WriteFile(string fileName = null)
        {
            string dataToFile = (this.isNumber) ? this.number.ToString() : this.@string;
            using (StreamWriter sw = new StreamWriter(fileName,true,Encoding.Unicode))
            {
                sw.Write(dataToFile+"\n");
            }
            Console.WriteLine("\n\tYour data <<<"+dataToFile+">>> was writen to file: "+ fileName);
        }

        protected void ValidateString(string strForvalidate)
        {

            if (Double.TryParse(strForvalidate,out this.number))
            {
                isNumber = true;
                return;
            }

            isNumber = false;
             
        }




















    }
}
