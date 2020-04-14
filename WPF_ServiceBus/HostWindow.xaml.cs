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
        ServiceBusHandler _handler;

        public HostWindow()
        {
            InitializeComponent();

        }

        private void Start_Host(object sender, RoutedEventArgs e)
        {
            // check if handler is empty, if so create an instance of it
            if (_handler == null)
            {
                // initialise SessionCodeGenerator
                SessionCodeGenerator generator = new SessionCodeGenerator();

                // Generade sessionCode
                string sessionCode = generator.GenerateSessionCode();

                // create an instance of the servicebus handler
                _handler = new ServiceBusHandler(sessionCode);

                _handler.program.MessageReceived += OnMessageReceived;
                lv.ItemsSource = _handler.PlayerCollection;
            }

            // check if user data is unset
            if (_handler.self == null)
            {

                // Set player data
                PlayerModel player = new PlayerModel();
                player.name = tbName.Text;
                player.type = PlayerType.Host;

                // store player data in handler
                _handler.self = player;

                // Serialize player data
                string message = JsonConvert.SerializeObject(player);

                // sent player data in a join request
                _handler.SendMessage(message, MessageType.JoinRequest);
            }
        }

        public void OnMessageReceived(string message)
        {
            _handler.HandleMessage(message);
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }
    }
}
