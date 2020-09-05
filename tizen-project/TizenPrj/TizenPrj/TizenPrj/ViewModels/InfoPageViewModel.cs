using System;
using TizenPrj.models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json.Linq;

namespace TizenPrj.ViewModels
{
    class InfoPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        private List<InfoModel> _dataList;
        public List<InfoModel> DataList
        {
            get { return _dataList; }
            set
            {
                _dataList = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DataList"));
            }
        }

        public InfoPageViewModel()
        {
            _ = GetInfoData();
        }

        private async Task GetInfoData()
        {
            var json = await new HttpClient().GetStringAsync("http://openapi.airkorea.or.kr/openapi/services/rest/ArpltnInforInqireSvc/getCtprvnRltmMesureDnsty?serviceKey=ng%2FLijSMgnWyRTRqAAQMqwcyEEt5yqOqnkeEGn1oMGfqIEaP492zeRvX4%2BBnK9Zn4woeMLrSj21o6WToZ%2BOpxQ%3D%3D&numOfRows=10&pageNo=1&sidoName=%EC%9D%B8%EC%B2%9C&ver=1.3&_returnType=json");
            var data = InfoModel.FromJson(json);
            //var jdata = (JArray)data["list"];
            DataList = new List<InfoModel>(data);
        }

    }



}