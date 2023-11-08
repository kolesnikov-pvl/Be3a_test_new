using Be3a.Model;
using Newtonsoft.Json;

namespace Be3a.Operation;

public class CurrencyCalculator
{
    private string Url { get; set; }
    public CurrencyCalculator( string url = null )
    {
        Url = url;
    }

    public async Task<decimal> CalculateSum(CalculationRequest cr)
    {
        var curRates = await GetExchangeRateAsync();
        var bynRate = getRateByCode(curRates, cr.ResultCurrency);
        var result = cr.Summa.Select(currencyVal => getRateByCode(curRates, currencyVal.Currency) * currencyVal.Value / bynRate).Sum();
        return result;
    }

    private async Task<Dictionary<string, CurrencyInfo>> GetExchangeRateAsync(string url = null)
    {
        var defUrl = "https://www.cbr-xml-daily.ru/daily_json.js";
        url = !string.IsNullOrWhiteSpace(Url)
            ? Url
            : !string.IsNullOrWhiteSpace(url)
                ? url
                : defUrl;
        Dictionary<string, CurrencyInfo> resp = null;
        using (var client = new HttpClient())
            try
            {
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var currencyRates = JsonConvert.DeserializeObject<CurrencyRatesRemote>(await response.Content.ReadAsStringAsync());
                    resp = currencyRates?.Valute ?? new Dictionary<string, CurrencyInfo>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error requesting data: {ex.Message}");
            }

        return resp;
    }

    private static decimal getRateByCode(Dictionary<string, CurrencyInfo> rates, string @code)
    {
        if (rates == null)
            throw new Exception("We do not have rates for calculations!");
        
        return rates[@code.ToUpper().Substring(0, 3)].Value;
    }
}