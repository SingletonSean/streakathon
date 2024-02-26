using Firebase.Auth;
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
        private readonly List<StreakEntryCollectionGroupItem> _streakEntryCollectionGroupItems;

        public IServiceProvider ServiceProvider { get; }
        public ICreateStreakCommand MockCreateStreakCommand { get; }
        public IShell MockShell { get; }

        public MockEnvironment()
        {
            _streakCollectionGroupItems = new List<StreakCollectionGroupItem>();
            _streakEntryCollectionGroupItems = new List<StreakEntryCollectionGroupItem>();

            IServiceCollection serviceCollection = new ServiceCollection();

            serviceCollection.AddStreakathon(Substitute.For<ILogger>());

            IGetAllStreaksQuery mockGetAllStreaksQuery = Substitute.For<IGetAllStreaksQuery>();
            mockGetAllStreaksQuery
                .Execute(default)
                .ReturnsForAnyArgs(c => Task.FromResult(_streakCollectionGroupItems.AsEnumerable()));
            serviceCollection.Replace(ServiceDescriptor.Singleton(mockGetAllStreaksQuery));

            IGetAllStreakEntriesQuery mockGetAllStreakEntriesQuery = Substitute.For<IGetAllStreakEntriesQuery>();
            mockGetAllStreakEntriesQuery
                .Execute(default)
                .ReturnsForAnyArgs(c => Task.FromResult(_streakEntryCollectionGroupItems.AsEnumerable()));
            serviceCollection.Replace(ServiceDescriptor.Singleton(mockGetAllStreakEntriesQuery));

            MockCreateStreakCommand = Substitute.For<ICreateStreakCommand>();
            MockCreateStreakCommand
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
            serviceCollection.Replace(ServiceDescriptor.Singleton(MockCreateStreakCommand));

            MockShell = Substitute.For<IShell>();
            serviceCollection.Replace(ServiceDescriptor.Singleton(MockShell));

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
                            StringValue = streak.Description,
                        },
                        UserId = new FirestoreStringField()
                        {
                            StringValue = userId
                        }
                    }
                }
            });
        }

        public void AddStreakEntry(string userId, StreakEntry streakEntry)
        {
            _streakEntryCollectionGroupItems.Add(new StreakEntryCollectionGroupItem()
            {
                Document = new FirestoreDocument<FirestoreStreakEntryFields>
                {
                    Name = $"streaks/{streakEntry.StreakId}/entries/${streakEntry.Id}",
                    Fields = new FirestoreStreakEntryFields()
                    {
                        Created = new FirestoreTimestampField()
                        {
                            TimestampValue = streakEntry.Created
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
