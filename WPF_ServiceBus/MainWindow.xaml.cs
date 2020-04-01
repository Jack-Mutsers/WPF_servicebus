using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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
using WPF_ServiceBus.ServiceBus;
using WPF_ServiceBus.ServiceBus.model;
using WPF_ServiceBus.ServiceBus.session;

namespace WPF_ServiceBus
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ConnectionModel connectionModel = new ConnectionModel();

        // start add
        Program program = new Program();
        // end add

        void addLogItem(string text)
        {
            int itemNr = lbLog.Items.Count + 1;
            lbLog.Items.Add(itemNr.ToString() + ": " + text);
            lbLog.SelectedIndex = lbLog.Items.Count - 1;
            lbLog.ScrollIntoView(lbLog.SelectedItem);
        }

        public MainWindow()
        {
            InitializeComponent();
            
            // start add
            InitializeProgram();
            // end add
        }

        // start add
        private void InitializeProgram()
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            //data.Add("ConnectionString", "");
            //data.Add("Topic", "");
            data.Add("Subscription", "S2DB04");
            //data.Add("Queue", "");

            program.SetData(data);
            program.MessageReceived += OnMessageReceived;
        }
        // end add

        private void shoot_Click(object sender, RoutedEventArgs e)
        {
            if (connectionModel.coordinates != null)
            {
                connectionModel.action = ConnectionModel.Action.shoot;
                connectionModel.naam = "Jack";
                connectionModel.sessionCode = "ab6ER8";

                program.SendMessage(connectionModel);

                addLogItem("shooting data send");
            }
            else
            {
                addLogItem("coordinates are required");
            }

            connectionModel.coordinates = null;
        }

        private void surrender_Click(object sender, RoutedEventArgs e)
        {
            connectionModel.action = ConnectionModel.Action.surender;
            connectionModel.naam = "Jack";
            connectionModel.sessionCode = "ab6ER8";

            program.SendMessage(connectionModel);
            addLogItem("you chose to surender");
        }

        private void OnPreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Coordinates coordinates = new Coordinates();
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
            connectionModel.coordinates = coordinates;

            // row and col now correspond Grid's RowDefinition and ColumnDefinition mouse was 
            // over when double clicked!
        }



        // start add
        public void OnMessageReceived(ConnectionModel source)
        {
            responseGrid.DataContext = source;
        }
        // end add

    }
}
