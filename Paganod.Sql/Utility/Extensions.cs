//using Paganod.Types.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paganod.Sql.Utility
{
    public static class Extensions
    {
        public static bool IsNullOrEmpty(this string strString)
        {
            return String.IsNullOrEmpty(strString);
        }
    }
}
