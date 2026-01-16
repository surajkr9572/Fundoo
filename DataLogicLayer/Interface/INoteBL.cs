using ModelLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLogicLayer.Interface
{
    public interface INoteBL
    {
        Task<Note>CreateNoteAsync(Note note);

        Task<IEnumerable<Note>> GetAllNoteAsync(int userId);
        Task<Note> GetNoteByIdAsync(int noteId, int userId);

        Task<Note> UpdateNoteAsync(Note note);

        Task<bool> MoveToTrashAsync(int noteId, int userId);
        Task<IEnumerable<Note>> GetTrashedAsync(int userId);
        Task<bool> RestoreAsync(int noteId, int userId);
        Task<bool> PermanentDeleteAsync(int noteId, int userId);

        Task<IEnumerable<Note>> GetArchivedAsync(int userId);
        Task<bool> ArchiveAsync(int noteId, int userId);
        Task<bool> UnarchiveAsync(int noteId, int userId);

        Task<bool> PinAsync(int noteId, int userId);
        Task<bool> UnpinAsync(int noteId, int userId);

        Task<bool> ChangeColorAsync(int noteId, int userId, string color);
    }
}
