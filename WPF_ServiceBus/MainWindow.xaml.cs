using System.Windows;

namespace WPF_ServiceBus
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnHost_Click(object sender, RoutedEventArgs e)
        {
            HostWindow host = new HostWindow();
            host.Show();
            this.Close();
        }

        private void btnJoin_Click(object sender, RoutedEventArgs e)
        {
            JoinWindow join = new JoinWindow();
            join.Show();
            this.Close();
        }

        private void tbnPlay_Click(object sender, RoutedEventArgs e)
        {
            PlayingField play = new PlayingField();
            play.Show();
            this.Close();
        }
    }
}
