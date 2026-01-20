using AutoMapper;
using BessinessLogicLayer.Interface;
using DataLogicLayer.Interface;
using DataLogicLayer.Repository;
using ModelLayer.Dto;
using ModelLayer.Entities;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BessinessLogicLayer.Repository
{
    public class NoteBL:INoteBL
    {
        private readonly INoteDL _notedl;
        private readonly IMapper _mapper;
        public NoteBL(INoteDL notedl, IMapper mapper)
        {
            _notedl = notedl;
            _mapper = mapper;
        }
        public async Task<NoteCreateResponseDto> CreateNoteAsync(int userId, NoteCreateDto noteCreateDto)
        {
            var note=_mapper.Map<Note>(noteCreateDto);
            note.UserId= userId;
            note.CreatedAt= DateTime.Now;
            var createdNote = await _notedl.CreateNoteAsync(note);
            return _mapper.Map<NoteCreateResponseDto>(createdNote);

        }
        public async Task<IEnumerable<NoteResponseDto>> GetAllNotesAsync(int userId, bool? isArchived = null, bool? isTrashed = null)
        {
            IEnumerable<Note> notes;

            if (isArchived == true)
                notes = await _notedl.GetArchivedAsync(userId);
            else if (isTrashed == true)
                notes = await _notedl.GetTrashedAsync(userId);
            else
                notes = await _notedl.GetAllNoteAsync(userId);

            return _mapper.Map<IEnumerable<NoteResponseDto>>(notes);
        }
        public async Task<NoteDetailsDto> GetNoteByIdAsync(int userId, int noteId)
        {
            var note=await _notedl.GetNoteByIdAsync(userId, noteId);
            if(note==null) throw new KeyNotFoundException("Note not found");
            return _mapper.Map<NoteDetailsDto>(note);
        }
        public async Task<NoteUpdateResponseDto> UpdateNoteAsync(int userId, int noteId, NoteUpdateDto noteDto)
        {
            var existingNote=await _notedl.GetNoteByIdAsync(noteId, userId);
            if (existingNote == null) throw new KeyNotFoundException("Note not found");
            _mapper.Map(noteDto, existingNote);
            existingNote.UpdatedAt= DateTime.Now;
            var updatedNote = await _notedl.UpdateNoteAsync(existingNote);

            return _mapper.Map<NoteUpdateResponseDto>(updatedNote);
        }
        public async Task<bool> DeleteNoteAsync(int userId, int noteId)
        {
            var existingNote=await _notedl.GetNoteByIdAsync(userId,noteId);
            if(existingNote == null) throw new KeyNotFoundException("Note not found");
            await _notedl.MoveToTrashAsync(userId,noteId);
            return true;
        }
        public async Task<bool> DeleteNotePermanentlyAsync(int userId, int noteId)
        {
            var existingNote=await _notedl.GetNoteByIdAsync(userId, noteId);
            if( existingNote == null) throw new KeyNotFoundException("Note not found");
            await _notedl.PermanentDeleteAsync(userId, noteId);
            return true;
        }
        public async Task<NoteArchiveResponseDto> ToggleArchiveNoteAsync(int userId, int noteId)
        {
            var existingNote = await _notedl.GetNoteByIdAsync(noteId, userId);
            if (existingNote == null)
                throw new KeyNotFoundException("Note not found");

            bool success;

            if (existingNote.IsArchive)
            {
                success = await _notedl.UnarchiveAsync(noteId, userId);
            }
            else
            {
                success = await _notedl.ArchiveAsync(noteId, userId);
            }

            if (!success)
                throw new KeyNotFoundException("Note not found");
            var result = await _notedl.UpdateNoteAsync(existingNote);

            return _mapper.Map<NoteArchiveResponseDto>(result);
        }

        public async Task<NoteIsPinResponseDto> TogglePinAsync(int userId, int noteId, NotePinDto pinDto)
        {
            bool success;
            if (pinDto.IsPin)
            {
                success=await _notedl.PinAsync(userId, noteId);
            }
            else
            {
                success=await _notedl.UnpinAsync(userId, noteId);
            }
            if (!success)
            {
                throw new KeyNotFoundException("Note not found");
            }
            return new NoteIsPinResponseDto
            {
                NotesId = noteId,
                IsPin = pinDto.IsPin,
                UpdatedAt = DateTime.UtcNow
            };
        }
        public async Task<NoteResponseDto> UpdateColorAsync(int userId, int noteId, string color)
        {
            var existingNote = await _notedl.GetNoteByIdAsync(userId, noteId);

            if (existingNote is null)
                throw new KeyNotFoundException("Note not found");

            existingNote.Colour = color;
            existingNote.UpdatedAt = DateTime.UtcNow;

            var result = await _notedl.UpdateNoteAsync(existingNote);
            return _mapper.Map<NoteResponseDto>(result);
        }
        public async Task<NoteResponseDto> RestoreNoteAsync(int userId, int noteId)
        {
            var existingNote = await _notedl.GetNoteByIdAsync(noteId, userId); // Fixed param order: noteId, userId

            if (existingNote is null)
                throw new KeyNotFoundException("Note not found");

            if (!existingNote.IsTrash)
                throw new InvalidOperationException("Note is not trashed");

            existingNote.IsTrash = false;
            existingNote.UpdatedAt = DateTime.UtcNow;
            var result = await _notedl.UpdateNoteAsync(existingNote);

            return _mapper.Map<NoteResponseDto>(result);
        }

    }
}
