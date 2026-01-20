using AutoMapper;
using ModelLayer.Dto;
using ModelLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BessinessLogicLayer.Mapping
{
    public class NoteProfile:Profile
    {
        public NoteProfile()
        {
            CreateMap<NoteCreateResponseDto, Note>();
            CreateMap<Note, NoteResponseDto>();
            CreateMap<Note, NoteResponseDto>();
            CreateMap<Note, NoteDetailsDto>();
            CreateMap<NoteUpdateDto, Note>()//want to map properties from NoteUpdateDto to Note
              .ForAllMembers(opt =>// Applies a rule to ALL properties during mapping. Instead of configuring each property one by one, this sets a global condition.
                  opt.Condition((src, dest, srcMember) => srcMember != null)); //src → Source object (NoteUpdateDto) , dest → Destination object (Note),srcMember → Current property value from source
            CreateMap<Note, NoteUpdateResponseDto>();
            
            CreateMap<Note, NoteArchiveResponseDto>();
            CreateMap<Note, NoteIsPinResponseDto>();
        }
    }
}
