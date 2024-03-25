using Microsoft.Extensions.DependencyInjection.Extensions;
using NSubstitute;
using Serilog;
using Streakathon.MAUI.Entities.Streaks;
using Streakathon.MAUI.Entities.Streaks.Data;
using Streakathon.MAUI.Pages;
using Streakathon.MAUI.Shared.Firestore;
using Streakathon.MAUI.Test.Mocks;

namespace Streakathon.MAUI.Test
{
    public class LoadStreaksTests
    {
        [Test]
        public void LoadsStreaks()
        {
            MockEnvironment mockEnvironment = new MockEnvironment();

            mockEnvironment.AddStreak("user_1", new Streak()
            {
                Id = "streak_1",
                Title = "Read",
                Description = "Read for 30 minutes every day",
            });

            HomeViewModel viewModel = mockEnvironment.ServiceProvider.GetRequiredService<HomeViewModel>();

            Assert.That(viewModel.StreakOverviewViewModels.Count(), Is.EqualTo(1));
        }
    }
}