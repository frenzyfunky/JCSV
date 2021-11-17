using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JCSV.Config
{
    public class ConverterOptions
    {
        public bool PreserveUnderlyingTypes { get; set; }
        public bool TreatEmptyStringAsNull { get; set; }
    }
}
