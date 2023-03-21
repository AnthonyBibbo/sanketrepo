using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WSDLsanket
{
    /// <summary>
    /// Summary description for WebService1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WebService1 : System.Web.Services.WebService
    {

        [WebMethod]
        
        public double CurrencyConverter(string Base, string target)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://currencystack.p.rapidapi.com/currency?target=" + target + "&base=" + Base),
                Headers =
        {
            { "X-RapidAPI-Key", "d0f831c9c2msh85532df1c7d8e45p168a96jsnd7aa295bff05" },
            { "X-RapidAPI-Host", "currencystack.p.rapidapi.com" },
        },
            };

            var response = client.SendAsync(request).Result;
            response.EnsureSuccessStatusCode();
            var body = response.Content.ReadAsStringAsync().Result;
            var json = JsonConvert.DeserializeObject<dynamic>(body);
            var newJson = JObject.Parse(json.rates);
            var key = newJson.Keys.FirstOrDefault();
            var d = new CurrData();
            d.rate = newJson.rates[key];
            d.Base = Base;
            d.Target = target;
            var j = JsonConvert.SerializeObject(d);

            return d.rate;
        }

    }


    public class CurrData
    {
        public string Base { get; set; }
        public string Target { get; set; }
        public double rate { get; set; }
    }
}
