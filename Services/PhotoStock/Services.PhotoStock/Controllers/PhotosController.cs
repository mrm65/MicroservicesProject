﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Services.PhotoStock.Dtos;

namespace Services.PhotoStock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> PhotoSave(IFormFile photo,CancellationToken cancellationToken)
        {
            if (photo!=null && photo.Length>0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/photos", photo.FileName);

                using (var stream = new FileStream(path,FileMode.Create))
                {
                    await photo.CopyToAsync(stream, cancellationToken);
                }

                var returnPath = "photos/" + photo.FileName;

                PhotoDto photoDto = new() {Url = returnPath};
                return Ok(photoDto);

            }

            return BadRequest("Photo is empty");
        }

        public IActionResult PhotoDelete(string photoUrl)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/photos",photoUrl);
            if (!System.IO.File.Exists(path))
            {
                return BadRequest("Photo not found");
            }

            System.IO.File.Delete(path);

            return Ok();
        }
    }
}