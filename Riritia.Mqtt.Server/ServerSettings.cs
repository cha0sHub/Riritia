using System;
using System.Collections.Generic;
using System.Text;

namespace Riritia.Mqtt.Server
{
    internal class MqttSettings : IMqttSettings
    {
        public int DefaultPort { get; set; } = 1884;
    }
}
