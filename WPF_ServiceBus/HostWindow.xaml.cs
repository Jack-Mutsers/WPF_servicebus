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
        ServiceBusHandler initialiser = new ServiceBusHandler(true); 

        public HostWindow()
        {
            InitializeComponent();

            initialiser.program.MessageReceived += OnMessageReceived;
        }

        public void OnMessageReceived(ActionModel source)
        {
            
        }
    }
}
