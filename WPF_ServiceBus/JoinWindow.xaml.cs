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
        ServiceBusHandler initialiser = new ServiceBusHandler();

        public JoinWindow()
        {
            InitializeComponent();

            initialiser.program.MessageReceived += OnMessageReceived;
        }

        public void OnMessageReceived(string message)
        {
            TransferModel transfer = JsonConvert.DeserializeObject<TransferModel>(message);

            if (transfer.type == MessageType.Response)
            {
                ActionModel source = JsonConvert.DeserializeObject<ActionModel>(transfer.message);
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
