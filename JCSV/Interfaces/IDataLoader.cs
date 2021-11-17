using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JCSV.Interfaces
{
    public interface IDataLoader
    {
        void Load(MemoryStream targetStream, string path);
    }
}
