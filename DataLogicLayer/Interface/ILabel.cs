using ModelLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLogicLayer.Interface
{
    public interface ILabel
    {
        Task<Label>CreateLabelAsync(Label label);
        Task<IEnumerable<Label>> GetAllLabelAsync(int userId);
        Task<IEnumerable<Note>> GetNotesByLabelAsync(int labelId, int userId);
        Task<Label>UpdateByIdAsync(Label label);
        Task<bool> DeleteByIdAsync(int labelId,int UserId);
    }
}
