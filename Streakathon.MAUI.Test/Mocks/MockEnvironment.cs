using Microsoft.Extensions.DependencyInjection.Extensions;
using NSubstitute;
using Serilog;
using Streakathon.MAUI.Entities.Streaks;
using Streakathon.MAUI.Entities.Streaks.Data;
using Streakathon.MAUI.Shared.Firestore;
using Streakathon.MAUI.Shared.Shells;

namespace Streakathon.MAUI.Test.Mocks
{
    public class MockEnvironment
    {
        private readonly List<StreakCollectionGroupItem> _streakCollectionGroupItems;

        public IServiceProvider ServiceProvider { get; }

        public MockEnvironment()
        {
            IServiceCollection serviceCollection = new ServiceCollection();

            serviceCollection.AddStreakathon(Substitute.For<ILogger>());

            IGetAllStreaksQuery mockGetAllStreaksQuery = Substitute.For<IGetAllStreaksQuery>();
            _streakCollectionGroupItems = new List<StreakCollectionGroupItem>();
            mockGetAllStreaksQuery
                .Execute(default)
                .ReturnsForAnyArgs(c => Task.FromResult(_streakCollectionGroupItems.AsEnumerable()));
            serviceCollection.Replace(ServiceDescriptor.Singleton(mockGetAllStreaksQuery));

            IGetAllStreakEntriesQuery mockGetAllStreakEntriesQuery = Substitute.For<IGetAllStreakEntriesQuery>();
            serviceCollection.Replace(ServiceDescriptor.Singleton(mockGetAllStreakEntriesQuery));

            ICreateStreakCommand mockCreateStreakCommand = Substitute.For<ICreateStreakCommand>();
            mockCreateStreakCommand
                .Execute(default)
                .ReturnsForAnyArgs(c =>
                {
                    CreateStreakCommandRequest request = c.Arg<CreateStreakCommandRequest>();

                    return new CreateStreakCommandResponse()
                    {
                        Name = $"streaks/{Guid.NewGuid()}",
                        Fields = request.Fields
                    };
                });
            serviceCollection.Replace(ServiceDescriptor.Singleton(mockCreateStreakCommand));

            IShell mockShell = Substitute.For<IShell>();
            serviceCollection.Replace(ServiceDescriptor.Singleton(mockShell));

            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        public void AddStreak(string userId, Streak streak)
        {
            _streakCollectionGroupItems.Add(new StreakCollectionGroupItem()
            {
                Document = new FirestoreDocument<FirestoreStreakFields>
                {
                    Name = $"streaks/{streak.Id}",
                    Fields = new FirestoreStreakFields()
                    {
                        Title = new FirestoreStringField()
                        {
                            StringValue = streak.Title
                        },
                        Description = new FirestoreStringField()
                        {
                            StringValue = streak.Description
                        },
                        UserId = new FirestoreStringField()
                        {
                            StringValue = userId
                        }
                    }
                }
            });
        }
    }
}
