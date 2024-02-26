using NUnit.Framework;
using Streakathon.MAUI.Entities.Streaks;
using Streakathon.MAUI.Pages;
using Streakathon.MAUI.Test.Mocks;

namespace Streakathon.MAUI.Test
{
    [TestFixture]
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
            mockEnvironment.AddStreakEntry("user_1", new StreakEntry("entry_1", "streak_1", DateTime.Now.Subtract(TimeSpan.FromDays(2))));
            mockEnvironment.AddStreakEntry("user_1", new StreakEntry("entry_1", "streak_1", DateTime.Now.Subtract(TimeSpan.FromDays(1))));
            mockEnvironment.AddStreakEntry("user_1", new StreakEntry("entry_1", "streak_1", DateTime.Now));

            HomeViewModel viewModel = mockEnvironment.ServiceProvider.GetRequiredService<HomeViewModel>();

            Assert.That(viewModel.StreakOverviewViewModels.Count(), Is.EqualTo(1));
            Assert.That(viewModel.StreakOverviewViewModels.First(s => s.Id == "streak_1").Length, Is.EqualTo(3));
        }
    }
}