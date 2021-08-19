using System;

namespace Modul3HW6
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var start = new Startup();
            start.Run().GetAwaiter().GetResult();
        }
    }
}
