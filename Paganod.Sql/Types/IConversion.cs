using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paganod.Sql.Types
{
    public interface IConversion
    {
        dynamic DbToAppConvert(object dbType);
        object AppToDbConvert(dynamic appType);
    }
}
