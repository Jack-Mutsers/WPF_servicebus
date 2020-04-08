using ServiceBus.model;
using System.Windows;
using System.Windows.Input;
using WPF_ServiceBus.Logics;

namespace WPF_ServiceBus
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ServiceBusHandler initialiser = new ServiceBusHandler(true);
        CoordinatesModel coordinates { get; set; }

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

            initialiser.program.MessageReceived += OnMessageReceived;
        }

        private void shoot_Click(object sender, RoutedEventArgs e)
        {
            ActionModel actionModel = new ActionModel();
            if (coordinates != null)
            {
                actionModel.action = ActionModel.Action.shoot;
                actionModel.coordinates = coordinates;
                actionModel.sessionCode = "ab6ER8";

                initialiser.SendMessage(actionModel);

                addLogItem("shooting data send");
            }
            else
            {
                addLogItem("coordinates are required");
            }
        }

        private void surrender_Click(object sender, RoutedEventArgs e)
        {
            ActionModel actionModel = new ActionModel();

            actionModel.action = ActionModel.Action.surender;
            actionModel.sessionCode = "ab6ER8";

            initialiser.SendMessage(actionModel);
            addLogItem("you chose to surender");
        }

        private void OnPreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            coordinates = new CoordinatesModel();
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

        public void OnMessageReceived(ActionModel source)
        {
            responseGrid.DataContext = source;
        }
    }
}
