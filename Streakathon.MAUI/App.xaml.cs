using Firebase.Auth;

namespace Streakathon.MAUI;

public partial class App : Application
{
    private readonly IFirebaseAuthClient _authClient;

    public App(IFirebaseAuthClient authClient)
	{
        _authClient = authClient;

        InitializeComponent();

		MainPage = new AppShell();
    }

    protected override void OnStart()
    {
        MainPage = new AppShell();

        if (_authClient.User == null)
        {
            Shell.Current.GoToAsync("//SignIn");
        }
        else
        {
            Shell.Current.GoToAsync("//Streaks");
        }

        base.OnStart();
    }
}
