using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HowDoYouDoThis.Models;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.Extensions.Configuration;
using MemeBank.Helpers;

namespace HowDoYouDoThis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly HowDoYouDoThisContext _context;
        private IConfiguration _configuration;

        public QuestionController(HowDoYouDoThisContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/Question
        [HttpGet]
        public IEnumerable<QuestionItem> GetQuestionItem()
        {
            return _context.QuestionItem;
        }

        // GET: api/Question/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetQuestionItem([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var questionItem = await _context.QuestionItem.FindAsync(id);

            if (questionItem == null)
            {
                return NotFound();
            }

            return Ok(questionItem);
        }

        // PUT: api/Question/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuestionItem([FromRoute] int id, [FromBody] QuestionItem questionItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != questionItem.ID)
            {
                return BadRequest();
            }

            _context.Entry(questionItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestionItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        

        // DELETE: api/Question/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuestionItem([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var questionItem = await _context.QuestionItem.FindAsync(id);
            if (questionItem == null)
            {
                return NotFound();
            }

            _context.QuestionItem.Remove(questionItem);
            await _context.SaveChangesAsync();

            return Ok(questionItem);
        }

        // GET: api/question/tag
        [Route("tag")]
        [HttpGet]
        public async Task<List<string>> GetTags()
        {
            var memes = (from m in _context.QuestionItem
                         select m.tag).Distinct();

            var returned = await memes.ToListAsync();

            return returned;
        }

        [HttpPost]
        public async Task<IActionResult> PostQuestionItem([FromForm]NewQuestion newQuestion)
        {
            if (!MultipartRequestHelper.IsMultipartContentType(Request.ContentType))
            {
                return BadRequest($"Expected a multipart request, but got {Request.ContentType}");
            }
            QuestionItem questionItem = new QuestionItem();
            try
            {
                using (var stream = newQuestion.image.OpenReadStream())
                {
                    var cloudBlock = await UploadToBlob(newQuestion.image.FileName, null, stream);
                    //// Retrieve the filename of the file you have uploaded
                    //var filename = provider.FileData.FirstOrDefault()?.LocalFileName;
                    if (string.IsNullOrEmpty(cloudBlock.StorageUri.ToString()))
                    {
                        return BadRequest("An error has occured while uploading your file. Please try again.");
                    }
                    System.Drawing.Image image = System.Drawing.Image.FromStream(stream);
                    
                    
                    
                }
            }
            catch (Exception)
            {
                // No images was received
                questionItem.diagramURL = "";
            }

            try
            {
                questionItem.title = newQuestion.title;
                questionItem.description = newQuestion.description;
                questionItem.tag = newQuestion.tag;
                questionItem.authorID = newQuestion.authorID;

                _context.QuestionItem.Add(questionItem);
                await _context.SaveChangesAsync();
                return Ok($"File: {questionItem.title} has successfully uploaded");
            }
            catch (Exception ex)
            {
                return BadRequest($"An error has occured. Details: {ex.Message}");
            }


        }

        private async Task<CloudBlockBlob> UploadToBlob(string filename, byte[] imageBuffer = null, System.IO.Stream stream = null)
        {

            var accountName = _configuration["AzureBlob:name"];
            var accountKey = _configuration["AzureBlob:key"];
            var storageAccount = new CloudStorageAccount(new StorageCredentials(accountName, accountKey), true);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            CloudBlobContainer imagesContainer = blobClient.GetContainerReference("question-diagrams");

            string storageConnectionString = _configuration["AzureBlob:connectionString"];

            // Check whether the connection string can be parsed.
            if (CloudStorageAccount.TryParse(storageConnectionString, out storageAccount))
            {
                try
                {
                    // Generate a new filename for every new blob
                    var fileName = Guid.NewGuid().ToString();
                    fileName += GetFileExtention(filename);

                    // Get a reference to the blob address, then upload the file to the blob.
                    CloudBlockBlob cloudBlockBlob = imagesContainer.GetBlockBlobReference(fileName);

                    if (stream != null)
                    {
                        await cloudBlockBlob.UploadFromStreamAsync(stream);
                    }
                    else
                    {
                        return new CloudBlockBlob(new Uri(""));
                    }

                    return cloudBlockBlob;
                }
                catch (StorageException ex)
                {
                    return new CloudBlockBlob(new Uri(""));
                }
            }
            else
            {
                return new CloudBlockBlob(new Uri(""));
            }

        }

        private string GetFileExtention(string fileName)
        {
            if (!fileName.Contains("."))
                return ""; //no extension
            else
            {
                var extentionList = fileName.Split('.');
                return "." + extentionList.Last(); //assumes last item is the extension 
            }
        }

        private bool QuestionItemExists(int id)
        {
            return _context.QuestionItem.Any(e => e.ID == id);
        }
    }
}