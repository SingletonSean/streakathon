namespace StreakTracker.MAUI.Pages;

public partial class AddStreakView : ContentPage
{
	public AddStreakView(AddStreakViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}
}