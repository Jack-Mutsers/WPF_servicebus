using Newtonsoft.Json;
using ServiceBus.model;
using ServiceBus.session;
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
using WPF_ServiceBus.Logics;

namespace WPF_ServiceBus
{
    /// <summary>
    /// Interaction logic for HostWindow.xaml
    /// </summary>
    public partial class HostWindow : Window
    {
        ServiceBusHandler _handler = new ServiceBusHandler(); 

        public HostWindow()
        {
            InitializeComponent();

            _handler.program.MessageReceived += OnMessageReceived;
        }

        private void Start_Host(object sender, RoutedEventArgs e)
        {
            string sessionCode = "AB12RB";

            PlayerModel player = new PlayerModel();
            player.name = tbName.Text;
            player.type = PlayerType.Host;

            _handler.self = player;

            string message = JsonConvert.SerializeObject(player);

            _handler.SendMessage(message, MessageType.JoinRequest, sessionCode);
        }



        public void OnMessageReceived(string message)
        {
            _handler.HandleMessage(message);

            TransferModel transfer = JsonConvert.DeserializeObject<TransferModel>(message);

            lblSession.Content = transfer.sessionCode;

            if (transfer.type == MessageType.Response)
            {
                SessionResponseModel response = JsonConvert.DeserializeObject<SessionResponseModel>(transfer.message);
                lv.ItemsSource = response.playerList;
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }
    }
}
