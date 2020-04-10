using Newtonsoft.Json;
using ServiceBus.model;
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
    /// Interaction logic for JoinWindow.xaml
    /// </summary>
    public partial class JoinWindow : Window
    {
        ServiceBusHandler _handler = new ServiceBusHandler();
        ServiceBusHandler initialiser = new ServiceBusHandler();

        public JoinWindow()
        {
            InitializeComponent();

            initialiser.program.MessageReceived += OnMessageReceived;
            lv.ItemsSource = _handler.PlayerCollection;
        }

        public void OnMessageReceived(string message)
        {
            _handler.HandleMessage(message);
            int test = 1;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (_handler.self == null) 
            {
                string sessionCode = tbCode.Text;

                PlayerModel player = new PlayerModel();
                player.name = tbName.Text;
                player.type = PlayerType.Guest;

                _handler.self = player;

                string message = JsonConvert.SerializeObject(player);

                _handler.SendMessage(message, MessageType.JoinRequest, sessionCode);
            }
        }
    }
}
