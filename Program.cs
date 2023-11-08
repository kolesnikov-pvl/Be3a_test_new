using Be3a.Model;
using Be3a.Operation;
using Newtonsoft.Json;

var requestJson = @"
        {
            ""Summa"": [
                { ""Currency"": ""euro"", ""Value"": 10 },
                { ""Currency"": ""usd"", ""Value"": 12 }
            ],
            ""ResultCurrency"": ""byn""
        }";

var request = JsonConvert.DeserializeObject<CalculationRequest>(requestJson) ?? new CalculationRequest();

var cc = new CurrencyCalculator();
var result = await cc.CalculateSum(request);

Console.WriteLine($"Hello, Be3a!\r\nResult: {result} {request.ResultCurrency}");
Console.Read();