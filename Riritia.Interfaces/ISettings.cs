using System;
using System.Collections.Generic;
using System.Text;

namespace Riritia.Interfaces
{
    public interface ISettings
    {
        string GetSettingValue(string key);
        T GetSettingValue<T>(string key);
    }
}
