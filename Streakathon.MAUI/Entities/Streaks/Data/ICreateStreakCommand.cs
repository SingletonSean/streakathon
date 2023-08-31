using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streakathon.MAUI.Entities.Streaks.Data
{
    public interface ICreateStreakCommand
    {
        [Post("/streaks")]
        Task<FirestoreQueryDocumentResponse<GetAllStreaksQueryFieldsResponse>> Execute([Body] FirestoreQueryDocumentResponse<GetAllStreaksQueryFieldsResponse> request);
    }
}
