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
    public class LabelDL: ILabelDL
    {
        private readonly ApplicationDbContext context;
        public LabelDL(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<Label> CreateLabelAsync(Label label)
        {
            context.Add(label);
            await context.SaveChangesAsync();
            return label;
        }
        public async Task<IEnumerable<Label>> GetAllLabelAsync(int userId)
        {
            return await context.Labels
                .Where(e => e.UserId == userId).ToListAsync();
        }
        public async Task<IEnumerable<Note>> GetNotesByLabelAsync(int labelId, int userId)
        {
            return await context.Notes
                .Include(n => n.Labels)
                .Where(n => n.UserId == userId &&
                            n.Labels.Any(l => l.LabelId == labelId))
                .ToListAsync();
        }
        public async Task<Label> UpdateByIdAsync(Label label)
        {
            var existing = await context.Labels
                .FirstOrDefaultAsync(l => l.LabelId == label.LabelId && l.UserId == label.UserId);

            if (existing == null)
                return null;

            existing.LabelName = label.LabelName;
            existing.UpdatedAt = DateTime.UtcNow;

            context.Labels.Update(existing);
            await context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteByIdAsync(int labelId, int userId)
        {
            var label = await context.Labels
                .FirstOrDefaultAsync(l => l.LabelId == labelId && l.UserId == userId);

            if (label == null)
                return false;

            context.Labels.Remove(label);
            await context.SaveChangesAsync();
            return true;
        }

    }

}
