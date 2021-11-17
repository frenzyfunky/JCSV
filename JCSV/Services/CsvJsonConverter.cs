using JCSV.Config;
using JCSV.Interfaces;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JCSV.Services
{
    public class CsvJsonConverter : IConverter, IDisposable
    {
        private MemoryStream _memoryStream = new MemoryStream();

        public static IConverter LoadFromStream(MemoryStream source)
        {
            return new CsvJsonConverter(source);
        }

        public static IConverter LoadFromPath(string path)
        {
            return new CsvJsonConverter(new DataLoader(), path: path);
        }

        private CsvJsonConverter(MemoryStream source)
        {
            _memoryStream = source;
        }

        private CsvJsonConverter(IDataLoader dataLoader, string path)
        {
            dataLoader.Load(_memoryStream, path: path);
        }

        //TODO implement this!
        public DataTable ConvertJsonToCsv()
        {
            throw new NotImplementedException();
        }

        public string ConvertCsvToJson(string delimiter = ",", bool hasHeader = true, Action<ConverterOptions> options = null)
        {
            DataTable dt = new DataTable();
            using (TextFieldParser parser = new(_memoryStream))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.TrimWhiteSpace = true;
                parser.SetDelimiters(delimiter);

                int i = 0;

                while (!parser.EndOfData)
                {
                    var line = parser.ReadFields();

                    if (line.All(l => string.IsNullOrEmpty(l)))
                    {
                        i++;
                        continue;
                    }

                    if (i == 0)
                    {
                        if (hasHeader)
                        {
                            foreach (var header in line)
                            {
                                dt.Columns.Add(header);
                            }
                            i++;
                            continue;
                        }
                    }

                    var row = dt.NewRow();
                    for (int j = 0; j < line.Length; j++)
                    {
                        row[j] = line[j];
                    }
                    dt.Rows.Add(row);

                    i++;
                }
            }

            ConverterOptions config = new ConverterOptions();

            if (options is not null)
            {
                options(config);
            }

            TypeInferringDataTableConverter converter = new TypeInferringDataTableConverter(config);

            var settings = new JsonSerializerSettings { Converters = new[] { converter } };
            var json = JsonConvert.SerializeObject(dt, settings);

            return json;
        }

        public void Dispose()
        {
            _memoryStream.Dispose();
        }
    }
}
