using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracing.DatabaseHandling
{
    public interface IDatabaseWriter
    {
        void WriteQuery(string data);
        bool TableExists(string SQLTableName);
        bool ColumnExists(string dbName, string SQLTableName, string SQLColumn);
    }
    
}
