using EasyCaching.Core;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.StaticFiles;

using QianShiChat.Common.Extensions;
using QianShiChat.WebApi.Hubs;
using QianShiChat.WebApi.Models.Requests;

using static System.Net.Mime.MediaTypeNames;

namespace QianShiChat.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : BaseController
    {
        private readonly IRedisCachingProvider _redisCachingProvider;
        private readonly IHubContext<ChatHub, IChatClient> _hubContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ChatController(
            IRedisCachingProvider redisCachingProvider,
            IHubContext<ChatHub, IChatClient> hubContext,
            IWebHostEnvironment webHostEnvironment)
        {
            _redisCachingProvider = redisCachingProvider;
            _hubContext = hubContext;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet("{filename}")]
        public async Task<IActionResult> GetFile(string filename)
        {
            var wwwroot = _webHostEnvironment.WebRootPath;

            var filePath = Path.Combine(wwwroot, filename);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            new FileExtensionContentTypeProvider()
                .Mappings.TryGetValue(Path.GetExtension(filePath), out var contenttype);

            return PhysicalFile(filePath, contenttype ?? "application/octet-stream");
        }

        [HttpPost("file")]
        public async Task<IActionResult> SendFile(
            [FromForm] ShedFileRequest request,
            CancellationToken cancellationToken = default)
        {
            // save to wwwroot
            var wwwroot = _webHostEnvironment.WebRootPath;

            var fileExt = Path.GetExtension(request.File.FileName);
            using var stream = request.File.OpenReadStream();
            var md5 = stream.ToMd5();

            var saveFilePath = Path.Combine(wwwroot, md5 + fileExt);

            using var fileStream = new FileStream(saveFilePath, FileMode.OpenOrCreate, FileAccess.Write);
            stream.CopyTo(fileStream);
            await fileStream.FlushAsync();

            // send message
            //await _hubContext.Clients.User(request.ToId.ToString()).PrivateChat(new QianShiChat.Models.ChatMessageDto
            //{
            //    Id = Yitter.IdGenerator.YitIdHelper.NextId(),
            //    Content = 
            //});

            return CreatedAtAction(nameof(GetFile), new { filename = md5 + fileExt }, "");
        }
    }
}
