using Microsoft.AspNetCore.Components;
using Services.Models;

namespace Client.GlobalEventCallBacks
{
    public  class GlobalEventCallBacks
    {
        public event Action<string>? OnRegisterOrSignInTabSelection;
        public event Action<bool>? OnLoadingSpinner;
        public event Action? OnGetCurrentUserStateForNav;

        public void RaiseRegisterOrSignInTabSelectionEvent(string value)
        {
            OnRegisterOrSignInTabSelection?.Invoke(value);
        }

        public void RaiseLoadingSpinnerEvent(bool value)
        {
            OnLoadingSpinner?.Invoke(value);
        }

        public void RaiseGetCurrentUserStateForNavEvent()
        {
            OnGetCurrentUserStateForNav?.Invoke();
        }
    }
}
