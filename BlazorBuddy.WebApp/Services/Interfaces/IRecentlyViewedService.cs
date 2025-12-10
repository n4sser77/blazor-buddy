using BlazorBuddy.Models;
using System.Collections.Generic;

namespace BlazorBuddy.WebApp.Services.Interfaces
{
    public interface IRecentlyViewedService
    {
        void Add(NoteDocument note);
        IEnumerable<NoteDocument> GetRecent();
        void Clear();


    }


}
