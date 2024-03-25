using Streakathon.MAUI.Pages;
using Streakathon.MAUI.Test.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streakathon.MAUI.Test
{
    public class AddStreakTests
    {
        [Test]
        public async Task AddsStreakToHomePage()
        {
            MockEnvironment mockEnvironment = new MockEnvironment();

            HomeViewModel homeViewModel = mockEnvironment.ServiceProvider.GetRequiredService<HomeViewModel>();
            Assert.That(homeViewModel.StreakOverviewViewModels.Count(), Is.EqualTo(0));

            AddStreakViewModel addStreakViewModel = mockEnvironment.ServiceProvider.GetRequiredService<AddStreakViewModel>();

            addStreakViewModel.Title = "Read";
            addStreakViewModel.Description = "Read for 30 minutes every day";
            await addStreakViewModel.SubmitStreakCommand.ExecuteAsync(null);

            Assert.That(homeViewModel.StreakOverviewViewModels.Count(), Is.EqualTo(1));
        }
    }
}
