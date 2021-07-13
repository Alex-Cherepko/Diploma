using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHR
{
    class DataBaseInitializer: DropCreateDatabaseIfModelChanges<EntityContext>
    {
    }
}
