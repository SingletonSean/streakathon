using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Refit;
using Streakathon.MAUI.Entities.Streaks;
using Streakathon.MAUI.Entities.Streaks.Data;
using Streakathon.MAUI.Pages;

namespace Streakathon.MAUI;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

        IServiceCollection services = builder.Services;

		services.AddSingleton<GetAllStreaksQuery>();
		services.AddSingleton<CreateStreakCommand>();
		services.AddSingleton<StreakStore>();

        services.AddTransient<HomeViewModel>();
		services.AddTransient<HomeView>();

        services.AddTransient<AddStreakViewModel>();
        services.AddTransient<AddStreakView>();

		services.AddRefitClient<IGetAllStreaksQuery>().ConfigureHttpClient(c =>
		{
			c.BaseAddress = new Uri("https://firestore.googleapis.com/v1/projects/streakathon/databases/(default)/documents");
        });
        services.AddRefitClient<IGetAllStreakEntriesQuery>().ConfigureHttpClient(c =>
        {
            c.BaseAddress = new Uri("https://firestore.googleapis.com/v1/projects/streakathon/databases/(default)");
        });
        services.AddRefitClient<ICreateStreakCommand>().ConfigureHttpClient(c =>
        {
            c.BaseAddress = new Uri("https://firestore.googleapis.com/v1/projects/streakathon/databases/(default)/documents");
        });

        return builder.Build();
	}
}
