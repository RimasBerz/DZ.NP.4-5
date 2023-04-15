using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;
using System.Web;
using HtmlAgilityPack;

namespace Http
{
   
    public partial class ApiWindow : Window
    {
        public ObservableCollection<NbuRate> NbuRates { get; set; }
        private HttpClient httpClient;
        private List<NbuRate> _nbuRates;
        public ObservableCollection<PasswordClient> PasswordClients { get; set; }
        public ApiWindow()
        {
            InitializeComponent();
            httpClient = new();
            NbuRates = new();
            PasswordClients = new();
            this.DataContext = this;

        }

        private async void NBYTodayButton_Click(object sender, RoutedEventArgs e)
        {
            String url = "https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange?json";
            
           // String json = await httpClient.GetStringAsync(url);
           // var rates = JsonSerializer.Deserialize<List<NbuRate>>(json);

            _nbuRates = await httpClient.GetFromJsonAsync<List<NbuRate>>(url);

            if (_nbuRates is null)
            {
                MessageBox.Show("Json parse error");
            }
            else
            {
                NbuRates.Clear();
                foreach (NbuRate rate in _nbuRates)
                {
                    NbuRates.Add(rate);
                }
             
            }
        }
        public class NbuRate
        {
            public int r030 { get; set; }
            public String txt { get; set; } = null!;
            public Double rate { get; set; }
            public String cc { get; set; } = null!;
            public String exchangedate { get; set; } = null!;
        }

        private void ListView_Click(object sender, RoutedEventArgs e)
        {
            if(e.OriginalSource is GridViewColumnHeader header && header.Content is not null
                && _nbuRates is not null)
            {
                _nbuRates = header.Content.ToString() switch
                {
                    "cc" => _nbuRates.OrderBy(r => r.cc).ToList(),
                    "txt" => _nbuRates.OrderBy(r => r.txt).ToList(),
                    "rate" => _nbuRates.OrderBy(r => r.rate).ToList(),
                    "r030" => _nbuRates.OrderBy(r => r.r030).ToList(),
                    _ => _nbuRates
                };

                NbuRates.Clear();
                foreach (NbuRate rate in _nbuRates)
                {
                    NbuRates.Add(rate);
                }
               // MessageBox.Show(header.Content.ToString());
            }
        }
        public class PasswordClient
        {
            public String p1 { get; set; } = null!;
            public String p2 { get; set; } = null!;
            public String p3 { get; set; } = null!;
            public String p4 { get; set; } = null!;
            public String p5 { get; set; } = null!;
        }
        private async void PasswordButton_Click(object sender, RoutedEventArgs e)
        {
            String url = "https://www.random.org/passwords/?num=5&len=8&format=html&rnd=new";
            var response = await httpClient.GetAsync(url);
            var responseString = await response.Content.ReadAsStringAsync();

            var pattern = @"<ul\s+class=""data"">.*?<li>(.*?)<\/li>.*?<li>(.*?)<\/li>.*?<li>(.*?)<\/li>.*?<li>(.*?)<\/li>.*?<li>(.*?)<\/li>.*?<\/ul>";
            var match = Regex.Match(responseString, pattern, RegexOptions.Singleline);

            if (!match.Success)
            {
                MessageBox.Show("No matches found.");
                return;
            }

            var client = new PasswordClient
            {
                p1 = match.Groups[1].Value,
                p2 = match.Groups[2].Value,
                p3 = match.Groups[3].Value,
                p4 = match.Groups[4].Value,
                p5 = match.Groups[5].Value
            };

            PasswordClients.Clear();
            PasswordClients.Add(client);

            PasswordText.Text = $"{client.p1}\n{client.p2}\n{client.p3}\n{client.p4}\n{client.p5}";
        }

        private async void NBUDateButton_Click(object sender, RoutedEventArgs e)
        {
            if (NBUDatePicker.SelectedDate.HasValue)
            {
                var selectedDate = NBUDatePicker.SelectedDate.Value;
                var dateParam = selectedDate.ToString("yyyyMMdd");
                String url = $"https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange?date={dateParam}&json";

                _nbuRates = await httpClient.GetFromJsonAsync<List<NbuRate>>(url);

                if (_nbuRates is null)
                {
                    MessageBox.Show("Json parse error");
                }
                else
                {
                    NbuRates.Clear();
                    foreach (NbuRate rate in _nbuRates)
                    {
                        NbuRates.Add(rate);
                    }

                }
            }
            else
            {
                MessageBox.Show("Please select a date.");
            }
        }
    }
    }

