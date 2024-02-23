using Streakathon.MAUI.Pages;

namespace Streakathon.MAUI;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

        Routing.RegisterRoute("Streaks/New", typeof(AddStreakView));
        Routing.RegisterRoute("Streaks/Details", typeof(StreakDetailsView));
    }
}
