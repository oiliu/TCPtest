using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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

namespace TCPPortCheck
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            labelResult.Foreground = Brushes.Black;
            labelResult.Content = "测试中";
            CheckTCPPort(txtIp.Text, Convert.ToInt32(txtPort.Text));
        }

        void CheckTCPPort(string ipAddress, int port, int timeout = 3000)
        {
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            ipAddress = ipAddress.Replace("http://", "").Replace("https://", "");
            IPHostEntry lipa = Dns.GetHostEntry(ipAddress);
            IPEndPoint iep = new IPEndPoint(lipa.AddressList[0], port);
            var result = socket.BeginConnect(iep, new AsyncCallback(Connect), socket);
            //var success = result.AsyncWaitHandle.WaitOne(timeout, true);
            //if (!result.IsCompleted)
            //{
            //    socket.Close();
            //}
        }

        void Connect(IAsyncResult ar)
        {
            Socket client = (Socket)ar.AsyncState;
            try
            {
                DisplayLabel(client.Connected);
                client.EndConnect(ar);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                //System.Windows.MessageBox.Show(e.Message);
            }
        }

        void DisplayLabel(bool t)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (t)
                {
                    labelResult.Content = "通";
                    labelResult.Foreground = Brushes.Green;
                }
                else
                {
                    labelResult.Content = "不通";
                    labelResult.FontWeight = FontWeights.Bold;
                    labelResult.Foreground = Brushes.Red;
                }
            }));
        }
    }
}
