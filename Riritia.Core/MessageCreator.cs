using Riritia.Interfaces;

namespace Riritia.Core
{
    public static class MessageCreator
    {
        public static IKalos CreateIncomingMessage(string communicator, string sender, string message)
        {
            var kalos = new Kalos(CommandStrings.IncomingMessage, message)
            {
                Sender = sender,
                Origin = communicator
            };
            return kalos;
        }

        public static IKalos CreateOutgoingMessage(string communicator, string sender, string message)
        {
            var kalos = new Kalos(CommandStrings.OutgoingMessage, message)
            {
                Sender = sender,
                Target = communicator
            };
            return kalos;
        }
    }
}
