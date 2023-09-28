namespace Streakathon.MAUI.Pages;

public partial class StreakDetailsView : ContentPage
{
	public StreakDetailsView(StreakDetailsViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}
}