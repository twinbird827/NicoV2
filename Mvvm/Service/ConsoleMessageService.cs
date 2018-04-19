using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NicoV2.Mvvm.Service
{
    public class ConsoleMessageService : IMessageService
    {
        public void Error(string message)
        {
            Console.WriteLine("Error: {0}", message);
        }

        public void Info(string message)
        {
            Console.WriteLine("Info: {0}", message);
        }

        public void Debug(string message)
        {
            Console.WriteLine("**************************************************");
            Console.WriteLine(message);
            Console.WriteLine("**************************************************");
        }
    }
}
