using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Threading;
using System.IO;
using System.Diagnostics;
namespace Http
{
    /// <summary>
    /// Interaction logic for PortalWindow.xaml
    /// </summary>
    public partial class PortalWindow : Window
    {
        private readonly string projectDir;
        public PortalWindow()
        {
            InitializeComponent();

            String currentDir = Directory.GetCurrentDirectory();

            int httpPosition = currentDir.IndexOf("Http");
            String projectDir = currentDir[..httpPosition];
        }

        private void ChatServer_Click(object sender, RoutedEventArgs e)
        {
           
            String serverPath = @"Server\bin\Debug\net6.0-windows\Server.exe";
            Process serverProcess = Process.Start(projectDir + serverPath);
            
        }

        private void ChatClient_Click(object sender, RoutedEventArgs e)
        {
            String serverPath = @"Server\bin\Debug\net6.0-windows\Client.exe";
            Process clientProcess = Process.Start(projectDir + serverPath);
        }

        private void HttpClient_Click(object sender, RoutedEventArgs e)
        {
          new MainWindow().Show();
        }

        private void Apirequest_Click(object sender, RoutedEventArgs e)
        {
            new ApiWindow().Show();
        }
    }
}
