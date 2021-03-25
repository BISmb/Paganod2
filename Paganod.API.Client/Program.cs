using Paganod.Shared.Type;
using System;
using System.Collections.Generic;

namespace Paganod.API.Client
{
    public class Account : Record
    {
        public Guid AccountId
        {
            get => this.Id;
            set => this.Id = value;
        }

        public string AccountNumber
        {
            get => Get<string>(nameof(AccountNumber));
            set => Set(nameof(AccountNumber), value);
        }

        public DateTime Iniatlized
        {
            get => Get<DateTime>(nameof(Iniatlized));
            set => Set(nameof(Iniatlized), value);
        }

        public Account(Dictionary<string, object> data = null)
            : base(typeof(Account).Name, data) { }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            PaganodConnection client = new PaganodConnection("http://localhost:5000");

            Account acc = new Account();
            acc.AccountNumber = "xxxxx";
        }
    }
}
