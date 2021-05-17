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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace Server
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TcpListener _listener;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            //apro una porta del server
            _listener = new TcpListener(System.Net.IPAddress.Any, int.Parse(PortaTextBox.Text));
            _listener.Start();
            //mi preparo per il gioco
            Random rn = new Random();

            //aspetto i buffer inviati dal client
            while (true)
            {
                TcpClient client = _listener.AcceptTcpClient();
                NetworkStream stream = client.GetStream();
                StreamReader sr = new StreamReader(client.GetStream());
                StreamWriter sw = new StreamWriter(client.GetStream());
                try
                {
                    byte[] buffer = new byte[1024];
                    stream.Read(buffer, 0, buffer.Length);
                    int recv = 0;
                    foreach(byte b in buffer)
                    {
                        if (b != 0)
                        {
                            recv++;
                        }
                    }
                    string request = Encoding.UTF8.GetString(buffer, 0, recv);
                    int dadi = rn.Next(1, 7);
                    if (dadi >  int.Parse(request)) sw.WriteLine(dadi + "-" + "Hai perso!");
                    if (dadi <  int.Parse(request)) sw.WriteLine(dadi + "-" + "Hai vinto!");
                    if (dadi == int.Parse(request)) sw.WriteLine(dadi + "-" + "Hai pareggiato!");
                    sw.Flush();
                    stream.Close();
                }
                catch
                {

                }
            }

        }
    }
}
