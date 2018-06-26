
using System.Threading;
using Riritia.HumanInterface.Riritia1;

namespace Riritia.Console
{
    internal static class Program
    {
        static void Main()
        {
            var riritia = InterfaceLoader.GetHumanInterfaceInstance();

            riritia.StartVoice("Irc");
            riritia.StartMind("Keyword");
            riritia.StartMind("HereToObserve");
            riritia.StartMind("WhatIs");

            while (true)
            {
                Thread.Sleep(100);
            }
        }
    }
}
