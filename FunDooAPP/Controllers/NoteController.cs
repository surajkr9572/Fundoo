using BessinessLogicLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using ModelLayer.Dto;
using System.Security.Claims;

namespace FunDooAPP.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/notes")]
    public class NoteController : ControllerBase
    {
        private readonly INoteBL _notebl;
        private readonly ILogger _logger;
        public NoteController(INoteBL notebl, ILogger logger)
        {
            _notebl = notebl;
            _logger = logger;
        }
        private int GetUserId()
        {
            int userId = int.Parse(
              User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (userId == 0)
            {
                throw new UnauthorizedAccessException("User ID not found in claims.");
            }
            return userId;
        }
        [HttpPost("Register-Note")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ApiResponse<NoteCreateResponseDto>>> Register([FromBody] NoteCreateDto noteCreateDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToArray();

                return BadRequest(new ApiResponse<NoteCreateResponseDto>(
                    false,
                    "Validation failed",
                    errors));
            }
            try
            {
                var userId = GetUserId();
                var result = await _notebl.CreateNoteAsync(userId, noteCreateDto);
                return StatusCode(201, new ApiResponse<NoteCreateResponseDto>(
                    true,
                    "Note Created Successfully",
                    result
                    ));
            } catch (InvalidOperationException ex)
            {
                _logger.LogWarning($"Conflict while creating Note: {ex.Message}");
                return Conflict(new ApiResponse<NoteCreateResponseDto>(
                    false,
                    ex.Message
                    ));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating note: {ex.Message}");
                return StatusCode(500, new ApiResponse<NoteCreateResponseDto>(
                    false,
                    ex.Message
                    ));
            }
        }
        [HttpPost("getUserNotes")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ApiResponse<IEnumerable<NoteResponseDto>>>> GetAllNotes(bool? isArchive = false, bool? isTrash = false)
        {
            try
            {
                var userId = GetUserId();
                var allnotes = await _notebl.GetAllNotesAsync(userId, isArchive, isTrash);
                return Ok(new ApiResponse<IEnumerable<NoteResponseDto>>(
                    true,
                     "Notes successfully retrieved",
                     allnotes
                    ));
            } catch (Exception ex)
            {
                _logger.LogError($"Error retrieving users: {ex.Message}");
                return StatusCode(500, new ApiResponse<IEnumerable<UserResponseDto>>(
                    false,
                    "An error occurred while retrieving users"));
            }
        }
        [HttpGet("{noteId}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ApiResponse<NoteDetailsDto>>> GetNoteById(int noteId)
        {
            try
            {
                int userId = GetUserId();
                var note = await _notebl.GetNoteByIdAsync(userId, noteId);
                return Ok(new ApiResponse<NoteDetailsDto>(
                    true,
                    "Note Successfully retrived",
                    note
                    ));
            } catch (KeyNotFoundException ex)
            {
                _logger.LogWarning($"Note not found {ex.Message}");
                return Ok(new ApiResponse<NoteDetailsDto>(
                    false,
                    ex.Message
                    ));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while retrieving note: {ex.Message}");
                return StatusCode(500, new ApiResponse<NoteDetailsDto>(
                    false,
                    "An error occurred while retrieving note"
                    ));
            }
        }
        [HttpPut("{noteId}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ApiResponse<NoteUpdateResponseDto>>> UpdateNote(int noteId, NoteUpdateDto noteDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToArray();

                return BadRequest(new ApiResponse<NoteCreateResponseDto>(
                    false,
                    "Validation failed",
                    errors));
            }
            try
            {
                int userId = GetUserId();
                var updateNote = await _notebl.UpdateNoteAsync(userId, noteId, noteDto);
                return Ok(new ApiResponse<NoteUpdateResponseDto>(
                    true,
                    "Note Update Successfully",
                    updateNote
                    ));
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning($"Key not found: {ex.Message}");
                return NotFound(new ApiResponse<NoteDetailsDto>(
                    false,
                    ex.Message)
                );
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while updating note: {ex.Message}");
                return StatusCode(500, new ApiResponse<NoteDetailsDto>(
                    false,
                    "An error occurred while updating note")
                );
            }
        }
        [HttpDelete("{noteId}/Permanent")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteNotePermanently(int noteId)
        {
            try
            {
                int UserId = GetUserId();
                await _notebl.DeleteNotePermanentlyAsync(UserId, noteId);
                return NoContent();
            }catch(KeyNotFoundException ex)
            {
                _logger.LogWarning($"Key not found: {ex.Message}");
                return NotFound(new ApiResponse<NoteDetailsDto>(
                    false,
                    ex.Message)
                );
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error While deleting note: {ex.Message}");
                return StatusCode(500, new ApiResponse<NoteDetailsDto>(
                    false,
                    "An error occurred while updating note")
                );
            }
        }
        [HttpDelete("{noteId}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<bool>> DeleteNoteAsync(int noteId)
        {
            try
            {
                var userId = GetUserId();
                await _notebl.DeleteNoteAsync(userId, noteId);

                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning($"Key not found: {ex.Message}");
                return NotFound(new ApiResponse<NoteResponseDto>(
                    false,
                    ex.Message)
                );
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while deleting note: {ex.Message}");
                return StatusCode(500, new ApiResponse<NoteResponseDto>(
                    false,
                    "An error occurred while deleting note")
                );
            }
        }
        [HttpPatch("{noteId}/restore")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ApiResponse<NoteResponseDto>>> RestoreNoteAsync(int noteId)
        {
            try
            {
                int userId = GetUserId();
                var restoreNote = await _notebl.RestoreNoteAsync(userId, noteId);
                return Ok(new ApiResponse<NoteResponseDto>(
                    true,
                    "Note restored successfully",
                    restoreNote
                    ));
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning($"Key not found: {ex.Message}");
                return NotFound(new ApiResponse<NoteResponseDto>(
                    false,
                    ex.Message)
                );
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while restoring note: {ex.Message}");
                return StatusCode(500, new ApiResponse<NoteResponseDto>(
                    false,
                    "An error occurred while restoring note")
                );
            }
        }
        [HttpPatch("{noteId}/archive")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ApiResponse<NoteArchiveResponseDto>>> ToggleArchiveNoteAsync(int noteId)
        {
            try
            {
                int userId = GetUserId();
                var archivenote = await _notebl.ToggleArchiveNoteAsync(userId, noteId);
                return Ok(new ApiResponse<NoteArchiveResponseDto>(
                    true,
                    "Note restored successfully",
                    archivenote
                    ));
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning($"Key not found: {ex.Message}");
                return NotFound(new ApiResponse<NoteResponseDto>(
                    false,
                    ex.Message)
                );
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while restoring note: {ex.Message}");
                return StatusCode(500, new ApiResponse<NoteResponseDto>(
                    false,
                    "An error occurred while restoring note")
                );
            }
        }
        [HttpPatch("{noteId}/pin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ApiResponse<NoteIsPinResponseDto>>> TogglePinAsync(int noteId, [FromBody] NotePinDto pinDto)
        {
            try
            {
                var userId = GetUserId();
                var updatedNote = await _notebl.TogglePinAsync(userId, noteId, pinDto);

                return Ok(new ApiResponse<NoteIsPinResponseDto>(
                    true,
                    "Note pin status updated successfully",
                    updatedNote
                ));
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning($"Key not found: {ex.Message}");
                return NotFound(new ApiResponse<NoteIsPinResponseDto>(
                    false,
                    ex.Message)
                );
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while pinning note: {ex.Message}");
                return StatusCode(500, new ApiResponse<NoteIsPinResponseDto>(
                    false,
                    "An error occurred while updating pin status")
                );
            }
        }
        [HttpPatch("{noteId}/color")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ApiResponse<NoteResponseDto>>> UpdateNoteColorAsync(int noteId, [FromBody] string color)
        {
            try
            {
                var userId = GetUserId();
                var updatedNote = await _notebl.UpdateColorAsync(userId, noteId, color);

                return Ok(new ApiResponse<NoteResponseDto>(
                    true,
                    "Note color updated successfully",
                    updatedNote
                ));
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning($"Key not found: {ex.Message}");
                return NotFound(new ApiResponse<NoteResponseDto>(
                    false,
                    ex.Message)
                );
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while updating note color: {ex.Message}");
                return StatusCode(500, new ApiResponse<NoteResponseDto>(
                    false,
                    "An error occurred while updating note color")
                );
            }
        }

    }
}
