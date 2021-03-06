﻿using ServiceBus.Entities.Enums;
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
using ServiceBus;
using ServiceBus.Resources;

namespace WPF_ServiceBus
{
    /// <summary>
    /// Interaction logic for PlayingField.xaml
    /// </summary>
    public partial class PlayingField : Window
    {
        ServiceBusHandler _handler { get; set; }
        Coordinates coordinates { get; set; }

        void addLogItem(string text)
        {
            int itemNr = lbLog.Items.Count + 1;
            lbLog.Items.Add(itemNr.ToString() + ": " + text);
            lbLog.SelectedIndex = lbLog.Items.Count - 1;
            lbLog.ScrollIntoView(lbLog.SelectedItem);
        }

        public PlayingField(ServiceBusHandler handler)
        {
            InitializeComponent();
            _handler = handler;
        }

        public PlayingField()
        {
            InitializeComponent();
            CreateDummyConnection();
        }

        public void CreateDummyConnection()
        {
            // check if handler is empty, if so create an instance of it
            if (_handler == null)
            {
                Player player = new Player()
                {
                    name = "",
                    orderNumber = 1,
                    ready = true,
                    type = PlayerType.Host
                };

                // create an instance of the servicebus handler
                _handler = new ServiceBusHandler(player, true);

                // initialise SessionCodeGenerator
                SessionCodeGenerator generator = new SessionCodeGenerator();

                // Generade sessionCode
                string sessionCode = generator.GenerateSessionCode();
                StaticResources.sessionCode = sessionCode;

                //_handler.program.CreateQueueConnection(PlayerType.Host);

                //_handler.program.QueueListner.MessageReceived += OnMessageReceived;
            }


            _handler.program.topic.MessageReceived += OnMessageReceived;
        }

        private void shoot_Click(object sender, RoutedEventArgs e)
        {
            GameAction actionModel = new GameAction();
            if (coordinates != null)
            {
                actionModel.action = PlayerAction.shoot;
                actionModel.coordinates = coordinates;
                actionModel.sessionCode = "ab6ER8";

                string message = JsonConvert.SerializeObject(actionModel);

                _handler.program.topic.SendTopicMessage(message, MessageType.Action);

                addLogItem("shooting data send");
            }
            else
            {
                addLogItem("coordinates are required");
            }
        }

        private void surrender_Click(object sender, RoutedEventArgs e)
        {
            GameAction actionModel = new GameAction();

            actionModel.action = PlayerAction.surender;
            actionModel.sessionCode = "ab6ER8";

            string message = JsonConvert.SerializeObject(actionModel);

            _handler.program.topic.SendTopicMessage(message, MessageType.Action);
            addLogItem("you chose to surender");
        }

        private void OnPreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            coordinates = new Coordinates();
            var point = Mouse.GetPosition(myGrid);

            double accumulatedHeight = 0.0;
            double accumulatedWidth = 0.0;

            // calc row mouse was over
            foreach (var rowDefinition in myGrid.RowDefinitions)
            {
                accumulatedHeight += rowDefinition.ActualHeight;
                if (accumulatedHeight >= point.Y)
                    break;
                coordinates.row++;
            }

            // calc col mouse was over
            foreach (var columnDefinition in myGrid.ColumnDefinitions)
            {
                accumulatedWidth += columnDefinition.ActualWidth;
                if (accumulatedWidth >= point.X)
                    break;
                coordinates.col++;
            }


            addLogItem("coordiantes chosen Row: " + coordinates.row + " Col: " + coordinates.col);

            // row and col now correspond Grid's RowDefinition and ColumnDefinition mouse was 
            // over when double clicked!
        }

        public void OnMessageReceived(string message)
        {
            Transfer transfer = JsonConvert.DeserializeObject<Transfer>(message);

            if (transfer.type == MessageType.Action)
            {
                GameAction source = JsonConvert.DeserializeObject<GameAction>(transfer.message);
                responseGrid.DataContext = source;
            }

            if (transfer.type == MessageType.Log)
            {

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
