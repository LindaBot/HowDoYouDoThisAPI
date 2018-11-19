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
using MemeBank.Helpers;
using Microsoft.Extensions.Configuration;

namespace HowDoYouDoThis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SolutionController : ControllerBase
    {
        private readonly HowDoYouDoThisContext _context;
        private IConfiguration _configuration;

        public SolutionController(HowDoYouDoThisContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/Solution
        [HttpGet]
        public IEnumerable<SolutionItem> GetSolutionItem()
        {
            return _context.SolutionItem;
        }

        // GET: api/Solution/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSolutionItem([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var solutionItem = await _context.SolutionItem.FindAsync(id);

            if (solutionItem == null)
            {
                return NotFound();
            }

            return Ok(solutionItem);
        }

        // PUT: api/Solution/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSolutionItem([FromRoute] int id, [FromBody] SolutionItem solutionItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != solutionItem.ID)
            {
                return BadRequest();
            }

            _context.Entry(solutionItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SolutionItemExists(id))
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

        // POST: api/Solution
        [HttpPost]
        public async Task<IActionResult> PostQuestionItem([FromForm]NewSolution newSolution)
        {
            if (!MultipartRequestHelper.IsMultipartContentType(Request.ContentType))
            {
                return BadRequest($"Expected a multipart request, but got {Request.ContentType}");
            }
            SolutionItem solutionItem = new SolutionItem();
            try
            {
                using (var stream = newSolution.workingImage.OpenReadStream())
                {
                    var cloudBlock = await UploadToBlob(newSolution.workingImage.FileName, null, stream);
                    //// Retrieve the filename of the file you have uploaded
                    //var filename = provider.FileData.FirstOrDefault()?.LocalFileName;
                    if (string.IsNullOrEmpty(cloudBlock.StorageUri.ToString()))
                    {
                        return BadRequest("An error has occured while uploading your file. Please try again.");
                    }
                    System.Drawing.Image image = System.Drawing.Image.FromStream(stream);
                    solutionItem.workingImage = cloudBlock.SnapshotQualifiedUri.AbsoluteUri;
                }
            }
            catch (Exception)
            {
                solutionItem.workingImage = "";
            }

            try
            {
                solutionItem.questionID = newSolution.questionID;
                solutionItem.answer = newSolution.answer;
                solutionItem.description = newSolution.description;
                // Solution is initialised with 0 upvotes
                solutionItem.upvotes = 0;
                solutionItem.authorID = newSolution.authorID;
                _context.SolutionItem.Add(solutionItem);
                await _context.SaveChangesAsync();

                return Ok($"File: {solutionItem.answer} has successfully uploaded");
            }
            catch (Exception ex) {
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

        // DELETE: api/Solution/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSolutionItem([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var solutionItem = await _context.SolutionItem.FindAsync(id);
            if (solutionItem == null)
            {
                return NotFound();
            }

            _context.SolutionItem.Remove(solutionItem);
            await _context.SaveChangesAsync();

            return Ok(solutionItem);
        }

        private bool SolutionItemExists(int id)
        {
            return _context.SolutionItem.Any(e => e.ID == id);
        }
    }
}