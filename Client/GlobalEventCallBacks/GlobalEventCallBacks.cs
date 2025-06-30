using Microsoft.AspNetCore.Components;
using Services.Models;

namespace Client.GlobalEventCallBacks
{
    public  class GlobalEventCallBacks
    {
        public event Action<string>? OnString;
        public event Action<bool>? OnBool;

        public void RaiseStringEvent(string value)
        {
            OnString?.Invoke(value);
        }

        public void RaiseBoolEvent(bool value)
        {
            OnBool?.Invoke(value);
        }
    }
}
