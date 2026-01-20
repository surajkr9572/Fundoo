using ModelLayer.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BessinessLogicLayer.Interface
{
    public interface INoteBL
    {
        Task<NoteCreateResponseDto> CreateNoteAsync(int userId, NoteCreateDto noteCreateDto);
        Task<IEnumerable<NoteResponseDto>> GetAllNotesAsync(int userId, bool? isArchived = null, bool? isTrashed = null);
        Task<NoteDetailsDto> GetNoteByIdAsync(int userId, int noteId);
        Task<NoteUpdateResponseDto> UpdateNoteAsync(int userId, int noteId, NoteUpdateDto noteDto);
        Task<bool> DeleteNoteAsync(int userId, int noteId);
        Task<bool> DeleteNotePermanentlyAsync(int userId, int noteId);
        Task<NoteResponseDto> RestoreNoteAsync(int userId, int noteId);
        Task<NoteArchiveResponseDto> ToggleArchiveNoteAsync(int userId, int noteId);
        Task<NoteIsPinResponseDto> TogglePinAsync(int userId, int noteId, NotePinDto pinDto);
        Task<NoteResponseDto> UpdateColorAsync(int userId, int noteId, string color);

    }
}
