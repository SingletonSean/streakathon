using Microsoft.Extensions.Logging;
using Streakathon.MAUI.Entities.Streaks;
using Streakathon.MAUI.Pages;

namespace Streakathon.MAUI;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

        IServiceCollection services = builder.Services;

		services.AddSingleton<StreakStore>();

		services.AddTransient<HomeViewModel>();
		services.AddTransient<HomeView>();

        services.AddTransient<AddStreakViewModel>();
        services.AddTransient<AddStreakView>();

        return builder.Build();
	}
}
