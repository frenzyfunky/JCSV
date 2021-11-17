using JCSV.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JCSV.Services
{
    internal class DataLoader : IDataLoader
    {
        public void Load(MemoryStream targetStream, string path)
        {
            var bytes = File.ReadAllBytes(path);
            targetStream.SetLength(0);
            targetStream.Write(bytes);
            targetStream.Position = 0;
        }
    }
}
