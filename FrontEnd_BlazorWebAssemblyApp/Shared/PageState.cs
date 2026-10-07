namespace FrontEnd_BlazorWebAssemblyApp.Shared
{
    public class PageState
    {
        private string? _title;
        public string? Title
        {
            get => _title;
            set
            {
                if (_title == value) return;
                _title = value;
                OnChange?.Invoke();
            }
        }

        public event System.Action? OnChange;
    }
}

