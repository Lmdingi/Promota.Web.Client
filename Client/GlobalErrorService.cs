namespace Client
{
    public class GlobalErrorService
    {
        public event Action<string>? OnError;

        public void RaiseError(string message)
        {
            OnError?.Invoke(message);
        }
    }
}
