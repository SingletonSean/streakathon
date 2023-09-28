using Streakathon.MAUI.Pages;

namespace Streakathon.MAUI;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

        Routing.RegisterRoute("Streak/New", typeof(AddStreakView));
        Routing.RegisterRoute("Streak/Details", typeof(StreakDetailsView));
    }
}
