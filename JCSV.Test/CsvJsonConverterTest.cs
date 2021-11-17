using JCSV.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace JCSV.Test
{
    public class CsvJsonConverterTest
    {
        const string CSV_FILE_NAME = "sample_csv.csv";
        const string JSON_FILE_NAME = "sample_json.json";

        [Fact]
        public void Should_Have_Same_Count_As_Original_Json()
        {
            string csvToJsonString = CsvJsonConverter.LoadFromPath(CSV_FILE_NAME).ConvertCsvToJson();
            var json = JsonConvert.DeserializeObject<IEnumerable<JObject>>(csvToJsonString);

            string originalJsonString = File.ReadAllText(JSON_FILE_NAME);
            var jsonOrg = JsonConvert.DeserializeObject<IEnumerable<JObject>>(originalJsonString);

            Assert.Equal(jsonOrg.Count(), json.Count());
        }

        [Fact]
        public void Should_Have_Inferred_Types()
        {
            string csvToJsonString = CsvJsonConverter.LoadFromPath(CSV_FILE_NAME).ConvertCsvToJson(options: (opt) =>
            {
                opt.PreserveUnderlyingTypes = true;
            });
            var json = JsonConvert.DeserializeObject<IEnumerable<JObject>>(csvToJsonString);

            string originalJsonString = File.ReadAllText(JSON_FILE_NAME);
            var jsonOrg = JsonConvert.DeserializeObject<IEnumerable<JObject>>(originalJsonString);

            Assert.Equal(json.First().GetValue("anaemia").Type, jsonOrg.First().GetValue("anaemia").Type);
        }

        [Fact]
        public void Should_Treat_Empty_String_As_Null()
        {
            string csvToJsonString = CsvJsonConverter.LoadFromPath(CSV_FILE_NAME).ConvertCsvToJson(options: (opt) =>
            {
                opt.TreatEmptyStringAsNull = true;
            });
            var json = JsonConvert.DeserializeObject<IEnumerable<JObject>>(csvToJsonString);

            Assert.Equal(JTokenType.Null, json.First().GetValue("age").Type);
        }

        [Fact]
        public void Should_All_Fields_As_String()
        {
            string csvToJsonString = CsvJsonConverter.LoadFromPath(CSV_FILE_NAME).ConvertCsvToJson();
            var json = JsonConvert.DeserializeObject<IEnumerable<JObject>>(csvToJsonString);

            Dictionary<string, JTokenType> jsonTypeDict = new();

            foreach (var kvp in json.First())
            {
                jsonTypeDict.Add(kvp.Key, kvp.Value.Type);
            }

            Assert.All(jsonTypeDict.Values, value => Assert.Equal(JTokenType.String, value));
        }
    }
}
