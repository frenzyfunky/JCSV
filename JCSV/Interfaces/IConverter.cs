using JCSV.Config;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JCSV.Interfaces
{
    public interface IConverter
    {
        DataTable ConvertJsonToCsv();
        string ConvertCsvToJson(string delimiter = ",", bool hasHeader = true, Action<ConverterOptions> options = null);
    }
}
