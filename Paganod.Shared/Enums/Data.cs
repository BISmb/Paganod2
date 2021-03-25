using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paganod.Shared
{
    public static partial class Enums
    {
        public static class Data
        {
            public enum RelationshipType
            {
                OneToOne,
                OneToMany,
                ManyToOne,
                ManyToMany
            }
        }
    }
}
