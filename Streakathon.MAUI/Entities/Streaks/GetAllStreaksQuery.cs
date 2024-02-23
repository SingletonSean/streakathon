using Streakathon.MAUI.Entities.Streaks.Data;
using Streakathon.MAUI.Shared.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streakathon.MAUI.Entities.Streaks
{
    public class GetAllStreaksQuery
    {
        private readonly IGetAllStreaksQuery _getAllStreaksQuery;
        private readonly IGetAllStreakEntriesQuery _getAllStreakEntriesQuery;

        public GetAllStreaksQuery(IGetAllStreaksQuery getAllStreaksQuery, IGetAllStreakEntriesQuery getAllStreakEntriesQuery)
        {
            _getAllStreaksQuery = getAllStreaksQuery;
            _getAllStreakEntriesQuery = getAllStreakEntriesQuery;
        }

        public async Task<IEnumerable<Streak>> Execute(string userId)
        {
            IEnumerable<StreakCollectionGroupItem> streaksResponse = await _getAllStreaksQuery.Execute(
                new FirestoreRunQueryRequest()
                {
                    StructuredQuery = new FirestoreStructuredQuery()
                    {
                        From = new List<FirestoreFromItem>()
                        {
                            new FirestoreFromItem()
                            {
                                CollectionId = "streaks"
                            }
                        },
                        Where = new FirestoreWhereItem()
                        {
                            FieldFilter = new FirestoreFieldFilter()
                            {
                                Field = new FirestoreField()
                                {
                                    FieldPath = "userId"
                                },
                                Op = "EQUAL",
                                Value = new FirestoreStringField()
                                {
                                    StringValue = userId
                                }
                            }
                        }
                    }
                }
            );

            IEnumerable<Streak> streaks = streaksResponse
                .Where(d => d.Document != null)
                .Select(d => new Streak()
                {
                    Id = d.Document.Name.Split("/").LastOrDefault(),
                    Title = d.Document.Fields.Title.StringValue,
                    Description = d.Document.Fields.Description.StringValue,
                })
                .ToList();

            if (streaks == null || !streaks.Any())
            {
                return new List<Streak>();
            }

            IEnumerable<StreakEntryCollectionGroupItem> streakEntriesResponse = await _getAllStreakEntriesQuery.Execute(
                new FirestoreRunQueryRequest()
                {
                    StructuredQuery = new FirestoreStructuredQuery()
                    {
                        From = new List<FirestoreFromItem>()
                        {
                            new FirestoreFromItem()
                            {
                                CollectionId = "entries",
                                AllDescendants = true
                            }
                        },
                        Where = new FirestoreWhereItem()
                        {
                            FieldFilter = new FirestoreFieldFilter()
                            {
                                Field = new FirestoreField()
                                {
                                    FieldPath = "userId"
                                },
                                Op = "EQUAL",
                                Value = new FirestoreStringField()
                                {
                                    StringValue = userId
                                }
                            }
                        }
                    }
                });

            ILookup<string, StreakEntry> streakEntriesLookup = streakEntriesResponse
                .Where(s => s?.Document?.Fields?.Created?.TimestampValue != null)
                .Select(s => new StreakEntry(s.StreakEntryId, s.StreakId, s.Document.Fields.Created.TimestampValue))
                .ToLookup(s => s.StreakId);

            foreach (Streak streak in streaks)
            {
                IEnumerable<StreakEntry> entries = streakEntriesLookup[streak.Id];

                streak.ResetEntries(entries);
            }

            return streaks;
        }
    }
}
