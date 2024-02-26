using NSubstitute;
using NSubstitute.ClearExtensions;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using Streakathon.MAUI.Entities.Streaks.Data;
using Streakathon.MAUI.Pages;
using Streakathon.MAUI.Test.Mocks;

namespace Streakathon.MAUI.Test
{
    [TestFixture]
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

        [Test]
        public async Task SavesStreakToFirestore()
        {
            MockEnvironment mockEnvironment = new MockEnvironment();

            AddStreakViewModel addStreakViewModel = mockEnvironment.ServiceProvider.GetRequiredService<AddStreakViewModel>();

            addStreakViewModel.Title = "Read";
            addStreakViewModel.Description = "Read for 30 minutes every day";
            await addStreakViewModel.SubmitStreakCommand.ExecuteAsync(null);

            await mockEnvironment.MockCreateStreakCommand.Received().Execute(Arg.Any<CreateStreakCommandRequest>());
        }

        [Test]
        public async Task NavigatesToHomePage_WhenSuccessfullyAddedStreak()
        {
            MockEnvironment mockEnvironment = new MockEnvironment();

            AddStreakViewModel addStreakViewModel = mockEnvironment.ServiceProvider.GetRequiredService<AddStreakViewModel>();

            addStreakViewModel.Title = "Read";
            addStreakViewModel.Description = "Read for 30 minutes every day";
            await addStreakViewModel.SubmitStreakCommand.ExecuteAsync(null);

            await mockEnvironment.MockShell.Received().GoToAsync("//Streaks");
        }

        [Test]
        public async Task DisplaysErrorMessage_WhenFailedToAddStreak()
        {
            MockEnvironment mockEnvironment = new MockEnvironment();

            mockEnvironment.MockCreateStreakCommand.ClearSubstitute();
            mockEnvironment.MockCreateStreakCommand.Execute(default).ThrowsForAnyArgs(new Exception());

            AddStreakViewModel addStreakViewModel = mockEnvironment.ServiceProvider.GetRequiredService<AddStreakViewModel>();

            addStreakViewModel.Title = "Read";
            addStreakViewModel.Description = "Read for 30 minutes every day";
            await addStreakViewModel.SubmitStreakCommand.ExecuteAsync(null);

            await mockEnvironment.MockShell.Received().DisplayAlert("Error", "Failed to create streak. Please try again later.", "Ok");
        }
    }
}
