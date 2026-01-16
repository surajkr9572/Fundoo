using DataLogicLayer.Data;
using DataLogicLayer.Interface;
using Microsoft.EntityFrameworkCore;
using ModelLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLogicLayer.Repository
{
    public class NoteBL : INoteBL
    {
        private readonly ApplicationDbContext context;
        public NoteBL(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<Note> CreateNoteAsync(Note note)
        {
            context.Notes.Add(note);
            await context.SaveChangesAsync();
            return note;
        }
        public async Task<IEnumerable<Note>> GetAllNoteAsync(int userId)
        {
            return await context.Notes.Where(e => e.UserId == userId && !e.IsTrash).ToListAsync();
        }
        public async Task<Note> GetNoteByIdAsync(int noteId, int userId)
        {
            return await context.Notes
                .Include(n => n.Labels)
                .Include(n => n.Collaborators)
                .FirstOrDefaultAsync(e => e.UserId == userId && e.NotesId == noteId);
        }
        public async Task<Note> UpdateNoteAsync(Note note)
        {
            var existingNote = await context.Notes.FindAsync(note.NotesId);
            if (existingNote == null)
            {
                return null;
            }
            existingNote.Title = note.Title;
            existingNote.Description = note.Description;
            existingNote.UpdatedAt = DateTime.Now;
            existingNote.Colour = note.Colour;
            existingNote.IsPin = note.IsPin;
            context.Notes.Update(existingNote);
            await context.SaveChangesAsync();
            return existingNote;
        }
        public async Task<bool> MoveToTrashAsync(int noteId, int userId)
        {
            var movetotrash=await context.Notes.FirstOrDefaultAsync(e=>e.NotesId == noteId && e.UserId == userId);
            if (movetotrash == null)
            {
                return false;
            }
            movetotrash.IsTrash = true;
            movetotrash.UpdatedAt = DateTime.Now;
            await context.SaveChangesAsync();
            return true;
        }
        public async Task<IEnumerable<Note>> GetTrashedAsync(int userId)
        {
            return await context.Notes
               .Where(n => n.UserId == userId && n.IsTrash)
               .ToListAsync();
        }
        public async Task<bool> RestoreAsync(int noteId, int userId)
        {
            var note = await context.Notes.FirstOrDefaultAsync(n => n.NotesId == noteId && n.UserId == userId);
            if (note == null) return false;

            note.IsTrash = false;
            note.UpdatedAt = DateTime.UtcNow;
            await context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> PermanentDeleteAsync(int noteId, int userId)
        {
            var note = await context.Notes.FirstOrDefaultAsync(n => n.NotesId == noteId && n.UserId == userId);
            if (note == null) return false;

            context.Notes.Remove(note);
            await context.SaveChangesAsync();
            return true;
        }

        // Archive
        public async Task<IEnumerable<Note>> GetArchivedAsync(int userId)
        {
            return await context.Notes
                .Where(n => n.UserId == userId && n.IsArchive && !n.IsTrash)
                .ToListAsync();
        }

        public async Task<bool> ArchiveAsync(int noteId, int userId)
        {
            var note = await context.Notes.FirstOrDefaultAsync(n => n.NotesId == noteId && n.UserId == userId);
            if (note == null) return false;

            note.IsArchive = true;
            note.UpdatedAt = DateTime.UtcNow;
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UnarchiveAsync(int noteId, int userId)
        {
            var note = await context.Notes.FirstOrDefaultAsync(n => n.NotesId == noteId && n.UserId == userId);
            if (note == null) return false;

            note.IsArchive = false;
            note.UpdatedAt = DateTime.UtcNow;
            await context.SaveChangesAsync();
            return true;
        }

        // Pin
        public async Task<bool> PinAsync(int noteId, int userId)
        {
            var note = await context.Notes.FirstOrDefaultAsync(n => n.NotesId == noteId && n.UserId == userId);
            if (note == null) return false;

            note.IsPin = true;
            note.UpdatedAt = DateTime.UtcNow;
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UnpinAsync(int noteId, int userId)
        {
            var note = await context.Notes.FirstOrDefaultAsync(n => n.NotesId == noteId && n.UserId == userId);
            if (note == null) return false;

            note.IsPin = false;
            note.UpdatedAt = DateTime.UtcNow;
            await context.SaveChangesAsync();
            return true;
        }

        // Color
        public async Task<bool> ChangeColorAsync(int noteId, int userId, string color)
        {
            var note = await context.Notes.FirstOrDefaultAsync(n => n.NotesId == noteId && n.UserId == userId);
            if (note == null) return false;

            note.Colour = color;
            note.UpdatedAt = DateTime.UtcNow;
            await context.SaveChangesAsync();
            return true;
        }
    }
}
