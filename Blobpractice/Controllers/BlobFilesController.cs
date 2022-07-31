using Blobpractice.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Blobpractice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlobFilesController : ControllerBase
    {
        private readonly IBlobService _blobService;

        public BlobFilesController(IBlobService blobService)
        {
            _blobService = blobService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFiles()
        {
            var files = await _blobService.AllBlobs("images");
            return Ok(files);
        }

        [HttpGet("checky")]
        //[Route("cheky")]
        public async Task<IActionResult> cheky(string name)
        {
            var files = await _blobService.GetBlob(name, "images");
            return Ok(files);
        }


        [HttpPost]
        public async Task<IActionResult> AddFile(IFormFile file)
        {
            if(file == null || file.Length < 1)
            {
                return BadRequest("null");

            }

            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);

            var res = await _blobService.UploadBlob(fileName, file, "images");

            return Ok(res);

        }

        [HttpDelete]
        public async Task<IActionResult> DeleteFile(string name)
        {
            await _blobService.DeleteBlob(name, "images");
            return Ok();
        }

    }
}
