using System;
using System.Windows;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace Iot.SignalR.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private HubConnection hubConnection;
        public MainWindow()
        {
            InitializeComponent();

            hubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:5001/IotServerHub")
                .Build();

            hubConnection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await hubConnection.StartAsync();
            };
        }

        private void BtnConnect_Click(object sender, RoutedEventArgs e)
        {
            hubConnection.On<string>("SetState", (message) =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    lstMessages.Items.Add(message);
                });
            });

            hubConnection.StartAsync();
        }

        private void BtnSendMessage_Click(object sender, RoutedEventArgs e)
        {
            hubConnection.SendAsync("SendState", txtMessage.Text);
        }
    }
}
