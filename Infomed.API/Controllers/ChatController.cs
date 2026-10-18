using Infomed.Domain.Dto;
using Infomed.Services.Chat;
using Microsoft.AspNetCore.Mvc;

namespace Infomed.API.Controllers;

public class ChatController(DoctorChatService chatService) : BaseController
{
    [HttpPost("messages")]
    public async Task<ActionResult<ChatReplyDto>> SendAsync([FromBody] ChatRequestDto request, CancellationToken cancellationToken)
    {
        var response = await chatService.AskAsync(request.Message.Trim(), cancellationToken);

        return Ok(response);
    }
}
