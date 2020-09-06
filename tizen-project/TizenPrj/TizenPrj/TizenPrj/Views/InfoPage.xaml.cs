using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TizenPrj.models;
using Newtonsoft.Json.Linq;

namespace TizenPrj.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InfoPage : ContentPage
    {
        public InfoPage()
        {
            InitializeComponent();
        }


        private async void Button_Load(object sender, EventArgs e)
        {
            var json = await new HttpClient().GetStringAsync("http://openapi.airkorea.or.kr/openapi/services/rest/ArpltnInforInqireSvc/getCtprvnMesureSidoLIst?serviceKey=ng%2FLijSMgnWyRTRqAAQMqwcyEEt5yqOqnkeEGn1oMGfqIEaP492zeRvX4%2BBnK9Zn4woeMLrSj21o6WToZ%2BOpxQ%3D%3D&numOfRows=10&pageNo=2&sidoName=%EA%B2%BD%EA%B8%B0&searchCondition=DAILY&_returnType=json");
            JObject jObject = JObject.Parse(json);
            var list = jObject.SelectToken("list");
            var jsonList = list[0];
            string dataTime = jsonList["dataTime"].ToString();
            string sidoName = jsonList["sidoName"].ToString();
            string cityName = jsonList["cityName"].ToString();
            string no2Value = jsonList["no2Value"].ToString();
            string o3Value = jsonList["o3Value"].ToString();
            string pm10Value = jsonList["pm10Value"].ToString();
            string pm25Value = jsonList["pm25Value"].ToString();
            string total = "";
            total += "위치 : "+ sidoName + " " + cityName + "\n";
            total += "측정 시간 : " + dataTime + "\n";
            total += "이산화질소 농도 : " + no2Value + "ppm\n";
            total += "오존 농도 : " + o3Value + "ppm\n";
            total += "미세먼지(PM10) 농도 : " + pm10Value + "㎍/㎥\n";
            total += "미세먼지(PM2.5) 농도 : " + pm25Value + "㎍/㎥\n";
            apiText.Text = total;
        }


    }
}
