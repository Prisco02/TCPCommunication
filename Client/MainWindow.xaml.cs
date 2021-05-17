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

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TcpClient _client;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _client = new TcpClient(IPTextBox.Text, int.Parse(PortaTextBox.Text));

                //generiamo il messaggio contenente il risultato del lancio dei dadi
                Random rn = new Random();
                string messaggio = rn.Next(1, 7).ToString();
                MyResultLbl.Content = "Mio risultato: " + messaggio;

                //creiamo il buffer da mandare
                int byteCount = Encoding.ASCII.GetByteCount(messaggio + 1);
                byte[] sendData = new byte[byteCount];
                sendData = Encoding.ASCII.GetBytes(messaggio);

                //mandiamo il buffer al server
                NetworkStream stream = _client.GetStream();
                stream.Write(sendData, 0, sendData.Length);

                //aspettiamo una risposta dal server
                StreamReader sr = new StreamReader(stream);
                string response = sr.ReadLine();
                string[] risposte = response.Split('-');
                ServerResultLbl.Content ="Risultato server: "+risposte[0];
                MatchResultLbl.Content = risposte[1];

                stream.Close();
                _client.Close();
            }
            catch
            {
                MessageBox.Show("IMPOSSIBILE CONNETTERSI AL SERVER", "Impossibile connettersi al server specificato, verificare la stabilitá della connessione");
            }
            
        }
    }
}
