﻿using CommunityToolkit.Maui;
using Firebase.Auth;
using Firebase.Auth.Providers;
using Microsoft.Extensions.Logging;
using Refit;
using Streakathon.MAUI.Entities.Streaks;
using Streakathon.MAUI.Entities.Streaks.Data;
using Streakathon.MAUI.Entities.Users;
using Streakathon.MAUI.Pages;
using Serilog;
using Firebase.Auth.Repository;
using Streakathon.MAUI.Shared.Shells;

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

        services.AddStreakathon(
            new LoggerConfiguration()
                .WriteTo.Debug()
                .WriteTo.File(Path.Combine(FileSystem.Current.AppDataDirectory, "logs", "log.txt"), rollingInterval: RollingInterval.Day)
                .CreateLogger());

        return builder.Build();
	}

    public static IServiceCollection AddStreakathon(this IServiceCollection services, Serilog.ILogger logger)
    {
        services.AddSerilog(logger);

        services.AddSingleton<IFirebaseAuthClient>(new FirebaseAuthClient(new FirebaseAuthConfig()
        {
            ApiKey = "AIzaSyC90ghgJuy9qyY7K8rTiSZg56FC0VbVaHQ",
            AuthDomain = "streakathon.firebaseapp.com",
            Providers =
            [
                new EmailProvider()
            ],
            UserRepository = new FileUserRepository("Streakathon")
        }));

        services.AddSingleton<GetAllStreaksQuery>();
        services.AddSingleton<CreateStreakCommand>();
        services.AddSingleton<CreateStreakEntryCommand>();
        services.AddSingleton<StreakStore>();

        services.AddTransient<HomeViewModel>();
        services.AddTransient<HomeView>();

        services.AddTransient<AddStreakViewModel>();
        services.AddTransient<AddStreakView>();

        services.AddTransient<StreakDetailsViewModel>();
        services.AddTransient<StreakDetailsView>();

        services.AddTransient<SignUpViewModel>();
        services.AddTransient<SignUpView>();

        services.AddTransient<SignInViewModel>();
        services.AddTransient<SignInView>();

        services.AddTransient<CurrentUserAuthHttpMessageHandler>();

        services.AddRefitClient<IGetAllStreaksQuery>().ConfigureHttpClient(c =>
        {
            c.BaseAddress = new Uri("https://firestore.googleapis.com/v1/projects/streakathon/databases/(default)");
        }).AddHttpMessageHandler<CurrentUserAuthHttpMessageHandler>();

        services.AddRefitClient<IGetAllStreakEntriesQuery>().ConfigureHttpClient(c =>
        {
            c.BaseAddress = new Uri("https://firestore.googleapis.com/v1/projects/streakathon/databases/(default)");
        }).AddHttpMessageHandler<CurrentUserAuthHttpMessageHandler>();

        services.AddRefitClient<ICreateStreakCommand>().ConfigureHttpClient(c =>
        {
            c.BaseAddress = new Uri("https://firestore.googleapis.com/v1/projects/streakathon/databases/(default)/documents");
        }).AddHttpMessageHandler<CurrentUserAuthHttpMessageHandler>();

        services.AddRefitClient<ICreateStreakEntryCommand>().ConfigureHttpClient(c =>
        {
            c.BaseAddress = new Uri("https://firestore.googleapis.com/v1/projects/streakathon/databases/(default)/documents");
        }).AddHttpMessageHandler<CurrentUserAuthHttpMessageHandler>();
        
        services.AddSingleton<IShell, MauiShell>();

        return services;
    }
}