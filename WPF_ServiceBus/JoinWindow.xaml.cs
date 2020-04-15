﻿using Newtonsoft.Json;
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
                string sessionCode = tbCode.Text;

                // create an instance of the servicebus handler
                _handler = new ServiceBusHandler(sessionCode, true);

                _handler.program.MessageReceived += OnMessageReceived;
            }

            if (_handler.self == null) 
            {
                PlayerModel player = new PlayerModel();
                player.name = tbName.Text;
                player.type = PlayerType.Guest;

                _handler.self = player;

                string message = JsonConvert.SerializeObject(player);

                _handler.SendMessage(message, MessageType.JoinRequest);
            }
        }

        public void OnMessageReceived(string message)
        {
            _handler.HandleMessage(message);

            TransferModel transfer = JsonConvert.DeserializeObject<TransferModel>(message);
            if (transfer.type == MessageType.Response)
            {
                lblSession.Content = _handler.SessionCode;
                lv.ItemsSource = _handler.PlayerCollection;
            }
        }
    }
}
