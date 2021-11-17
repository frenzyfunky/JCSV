## JSCV

Simple CSV - Json converter.

- [x] Csv to Json
- [ ] Json to Csv

```csharp
var converter = CsvJsonConverter.LoadFromPath(Path);
string json = converter.ConvertCsvToJson(delimeter: ",", hasHeader: true, options: opt => 
			{
				opt.PreserveUnderlyingTypes = true,
				opt.TreatEmptyStringAsNull = true
			})
```