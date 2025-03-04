using Avalonia.Controls;
using conference.Views;
using ReactiveUI;

namespace conference.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    UserControl uc;

    public UserControl UC
    {
        get => uc;
        set => this.RaiseAndSetIfChanged(ref uc, value);
    }

    public MainWindowViewModel()
    {
        UC = new PublicView();
    }

}