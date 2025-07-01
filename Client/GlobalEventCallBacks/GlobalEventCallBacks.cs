using Microsoft.AspNetCore.Components;
using Services.Models;

namespace Client.GlobalEventCallBacks
{
    public  class GlobalEventCallBacks
    {
        public event Action<string>? OnTabSelection;
        public event Action<bool>? OnLoadingSpinner;
        public event Action OnUserAuthSwitchNav;

        public void RaiseStringEvent(string value)
        {
            OnTabSelection?.Invoke(value);
        }

        public void RaiseBoolEvent(bool value)
        {
            OnLoadingSpinner?.Invoke(value);
        }

        public void RaiseSwitchNavEvent()
        {
            OnUserAuthSwitchNav.Invoke();
        }
    }
}
