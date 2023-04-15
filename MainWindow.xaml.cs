using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Http
{
    public partial class MainWindow : Window
    {
        private HttpClient httpClient;
        public MainWindow()
        {
            InitializeComponent();
            httpClient = new HttpClient();
        }

        private async void get1Button_Click(object sender, RoutedEventArgs e)
        {
            String result = await httpClient.GetStringAsync(url1TextBox.Text);
            resultTextBlock.Text = result;
        }

        private async void get2Button_Click(object sender, RoutedEventArgs e)
        {
            HttpRequestMessage request = new(
                HttpMethod.Get, url2TextBox.Text);

            HttpResponseMessage response = await 
                httpClient.SendAsync(request);
            printResponse(response);
        }

        private async void head3Button_Click(object sender, RoutedEventArgs e)
        {
            var response = await httpClient.SendAsync(new HttpRequestMessage(
                HttpMethod.Head,url3TextBox.Text));
            printResponse(response);
        }
        private async void printResponse(HttpResponseMessage response)
        {
            resultTextBlock.Text = $"HTTP/{response.Version} {(int)response.StatusCode} {response.ReasonPhrase}\n";

            foreach (var header in response.Headers)
            {
                String headerString = header.Key + ": ";
                foreach (string value in header.Value)
                {
                    headerString += value + " ";
                }
                resultTextBlock.Text += $"{headerString}\n";
            }
            resultTextBlock.Text += "------------------------------\n";
            resultTextBlock.Text += await response.Content.ReadAsStringAsync();
            resultTextBlock.Text += "------------------------------\n";
        }

        private async void options4Button_Click(object sender, RoutedEventArgs e)
        {
            var response = await httpClient.SendAsync(new HttpRequestMessage(
               HttpMethod.Options, url4TextBox.Text));
            printResponse(response);
        }

        private async void options5Button_Click(object sender, RoutedEventArgs e)
        {
            var response = await httpClient.SendAsync(new HttpRequestMessage(
             HttpMethod.Get, url5TextBox.Text));
            printResponse(response);
        }
    }
}
