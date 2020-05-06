using ServiceBus.Entities.Enums;
using Newtonsoft.Json;
using ServiceBus.Entities.models;
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
using ServiceBus.Resources;

namespace WPF_ServiceBus
{
    /// <summary>
    /// Interaction logic for JoinWindow.xaml
    /// </summary>
    public partial class JoinWindow : Window
    {
        ServiceBusHandler _handler;

        public JoinWindow()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }

        private void btnJoin_Click(object sender, RoutedEventArgs e)
        {
            // check if handler is empty, if so create an instance of it
            if (_handler == null)
            {

                Player player = new Player();
                player.name = tbName.Text;
                player.type = PlayerType.Guest;

                // get sessin code
                string sessionCode = tbCode.Text;

                StaticResources.sessionCode = sessionCode;

                // create an instance of the servicebus handler
                _handler = new ServiceBusHandler(player);

                // create Queue connection
                _handler.program.CreateQueueListner(PlayerType.Guest);
                _handler.program.CreateQueueWriter(PlayerType.Guest);

                _handler.program.QueueListner.MessageReceived += OnQueueMessageReceived;
            }

            string message = JsonConvert.SerializeObject(StaticResources.user);

            _handler.program.QueueWriter.SendQueueMessage(message, MessageType.JoinRequest, _handler.program.QueueListner.QueueData);
        }

        public void OnQueueMessageReceived(string message)
        {
            Transfer transfer = JsonConvert.DeserializeObject<Transfer>(message);

            if (transfer.type == MessageType.Response)
            {
                _handler.HandleQueueMessage(message);
                lblSession.Content = StaticResources.sessionCode;
                _handler.program.topic.MessageReceived += OnTopicMessageReceived;
            }
        }

        public void OnTopicMessageReceived(string message)
        {
            Transfer transfer = JsonConvert.DeserializeObject<Transfer>(message);

            if (transfer.type == MessageType.NewPlayer)
            {
                _handler.HandleNewPlayerTopicMessage(message);
                lv.ItemsSource = StaticResources.PlayerList;
            }
        }
    }
}
