using System.Threading;
using System.Collections.Concurrent;
using Riritia.Interfaces;
using Riritia.Core;
using Microsoft.Extensions.Configuration;
using TwitchLib;
using TwitchLib.Models.Client;
using TwitchLib.Events.Client;
using Serilog;

namespace Riritia.Communicators.Irc
{
    public class IrcCommunicator : ICommunicator
    {
        private const int MessageDelay = 5000;

        private const string Irc = "Irc";
        public bool Active { get; set; }
        public string Destination { get; set; }

        private IHumanInterface HumanInterface { get; }
        private IConfiguration Configuration { get; }
        private ILogger Logger { get; }

        private ITwitchClient TwitchClient { get; set; }

        private BlockingCollection<IKalos> OutgoingMessages { get; }

        private Thread SendingThread { get; set; }

        public string Name { get; set; } = Irc;
        private System.Timers.Timer MessageTimer { get; }
        private ManualResetEvent MessageWaitHandle { get; }
        private CancellationTokenSource TokenSource { get; set; }

        public IrcCommunicator(IHumanInterface humanInterface, IConfiguration configuration, ILogger logger)
        {
            HumanInterface = humanInterface;
            Configuration = configuration;
            Logger = logger;

            OutgoingMessages = new BlockingCollection<IKalos>();

            MessageWaitHandle = new ManualResetEvent(true);
            MessageTimer = new System.Timers.Timer(MessageDelay);
            MessageTimer.Elapsed += MessageTimer_Elapsed;
        }

        private void MessageTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            MessageWaitHandle.Set();
        }

        public void Listen()
        {
            var connectionCredentials = new ConnectionCredentials(Configuration[SettingNames.Username], Configuration[SettingNames.OAuthCode]);
            TwitchClient = new TwitchClient(connectionCredentials, Configuration[SettingNames.ChannelName]);
            TwitchClient.OnConnected += TwitchClient_OnConnected;
            TwitchClient.OnConnectionError += TwitchClient_OnConnectionError;
            TwitchClient.OnJoinedChannel += TwitchClient_OnJoinedChannel;
            TwitchClient.OnMessageReceived += TwitchClient_OnMessageReceived;

            TwitchClient.Connect();
            SendingThread = new Thread(Run);
            SendingThread.Start();
        }

        private void TwitchClient_OnConnectionError(object sender, OnConnectionErrorArgs e)
        {
            Logger.Error($"Failed to connect to twitch: {e.Error}");
        }

        private void TwitchClient_OnConnected(object sender, OnConnectedArgs e)
        {
            Logger.Information("Connected to twitch.");
        }

        private void TwitchClient_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            ReceiveMessage(e.ChatMessage.Username, e.ChatMessage.Message);
        }

        private void ReceiveMessage(string username, string message)
        {
            var newMessage = MessageCreator.CreateIncomingMessage(Name, username, message);
            HumanInterface.AddMessageToIncomingAsync(newMessage);
        }

        private void TwitchClient_OnJoinedChannel(object sender, OnJoinedChannelArgs e)
        {
            TwitchClient.SendMessage("/me Joined Channel.");
        }

        public void Run()
        {
            TokenSource = new CancellationTokenSource();
            while (Active)
                Send();
        }

        public void Send()
        {
            var message = OutgoingMessages.Take(TokenSource.Token);
            MessageWaitHandle.WaitOne();
            TwitchClient.SendMessage(message.Msg);
            MessageWaitHandle.Reset();
            MessageTimer.Start();
        }

        public void AddMessage(IKalos message)
        {
            OutgoingMessages.Add(message);
        }

        public void Stop()
        {
            TwitchClient.Disconnect();
            Active = false;
            TokenSource.Cancel();
            SendingThread.Join();
        }
    }
}
