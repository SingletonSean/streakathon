namespace Streakathon.MAUI.Entities.Streaks;

public partial class StreakLengthDisplay : ContentView
{
	public static readonly BindableProperty LengthProperty =
		BindableProperty.Create(nameof(Length), typeof(int), typeof(StreakLengthDisplay), 0);

	public int Length
	{
		get => (int)GetValue(LengthProperty);
		set => SetValue(LengthProperty, value);
	}

	public static readonly BindableProperty CaptionProperty =
		BindableProperty.Create(nameof(Caption), typeof(string), typeof(StreakLengthDisplay), string.Empty, 
			propertyChanged: OnCaptionPropertyChanged);

    public string Caption
	{
		get => (string)GetValue(CaptionProperty);
		set => SetValue(CaptionProperty, value);
	}

    private static void OnCaptionPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is StreakLengthDisplay streakLengthDisplay)
		{
			streakLengthDisplay.OnPropertyChanged(nameof(HasCaption));
		}
    }

	public bool HasCaption => !string.IsNullOrEmpty(Caption);

    public static readonly BindableProperty LengthScoreProperty =
		BindableProperty.Create(nameof(LengthScore), typeof(StreakLengthScore), typeof(StreakLengthDisplay), StreakLengthScore.BAD);

	public StreakLengthScore LengthScore
	{
		get => (StreakLengthScore)GetValue(LengthScoreProperty);
		set => SetValue(LengthScoreProperty, value);
	}

	public StreakLengthDisplay()
	{
		InitializeComponent();
	}
}