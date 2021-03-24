using System;

namespace Paganod.API.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            PaganodConnection client = new PaganodConnection("http://localhost:5000");
        }
    }
}
