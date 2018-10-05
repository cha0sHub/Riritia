using System;
using System.Collections.Generic;
using System.Text;

namespace Riritia.Mqtt.Server
{
    internal interface IMqttSettings
    {
        int DefaultPort { get; set; }
    }
}
