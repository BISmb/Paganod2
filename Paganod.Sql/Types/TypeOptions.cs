using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paganod.Sql.Types
{
    public class TypeOptions : Dictionary<string, string>
    {
        private int MaxValue { get; set; }
        private int MaxLength { get; set; }

        public TypeOptions() { }

        public TypeOptions(IDictionary<string, string> dictionary)
            : base(dictionary) { }

        public TypeOptions Max(int maxValue)
        {
            MaxValue = maxValue;
            return this;
        }

        public TypeOptions Length(int length)
        {
            MaxLength = length;
            return this;
        }

        public TypeOptions Constraints(string constraintsString)
        {



            return this;
        }

        public TypeOptions IsVariable()
        {
            return this;
        }

        public int MaxStringLength
        {
            get
            {
                if (this.ContainsKey(nameof(MaxStringLength)))
                    return Convert.ToInt32(this[nameof(MaxStringLength)]);

                return 255; // TODO: Move this to constants
            }
            set
            {
                if (this.ContainsKey(nameof(MaxStringLength)))
                    this[nameof(MaxStringLength)] = $"{value}";
                else
                    this.Add(nameof(MaxStringLength), $"{value}");
            }
        }
    }
}
