using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CommunityItaly.Services;
using CommunityItaly.Services.FolderStructures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace CommunityItaly.Server.Functions
{
    public class Upload
    {
        private readonly IFileService fileService;
        private readonly ICommunityService communityService;
        private readonly IPersonService personService;
        private readonly IEventService eventService;

        public Upload(IFileService fileService, IEventService eventService, ICommunityService communityService, IPersonService personService)
        {
            this.fileService = fileService;
            this.eventService = eventService;
            this.communityService = communityService;
            this.personService = personService;
        }

        [FunctionName("UploadImage")]
        public async Task<IActionResult> PostImage(
           [HttpTrigger(AuthorizationLevel.Anonymous, HttpVerbs.POST, Route = "UploadImage")] HttpRequestMessage req,
           ILogger log)
        {
            var provider = new MultipartMemoryStreamProvider();
            await req.Content.ReadAsMultipartAsync(provider);
            var file = provider.Contents.First();
            var fileExtension = new FileInfo(file.Headers.ContentDisposition.FileName.Replace("\"", "")).Extension.ToLower();
            var fileData = await file.ReadAsByteArrayAsync();

            var querystring = req.RequestUri.ParseQueryString();
            string typeUpload = querystring.Get("type");
            string id = querystring.Get("id");
            if (string.IsNullOrEmpty(typeUpload) || string.IsNullOrEmpty(id))
                throw new ArgumentNullException("Id and UploadType must not be empty");
            Uri imageStorageUri = null;
            BlobInformation blobInformation;
            switch (typeUpload.ToUpper())
            {
                case "COMMUNITY":
                    if (await communityService.ExistsAsync(id))
                    {
                        blobInformation = ImageStructure.CommunityPictureOriginal(id, fileExtension);
                        imageStorageUri = await fileService.UploadImageAsync(blobInformation.BlobContainerName, blobInformation.FileName, fileData).ConfigureAwait(false);
                        await communityService.UpdateImageAsync(id, imageStorageUri).ConfigureAwait(false);
                    }
                    else
                        throw new ArgumentException($"Community id: {id}, not exist");
                    break;
                case "EVENT":
                    if (await eventService.ExistsAsync(id))
                    {
                        blobInformation = ImageStructure.EventPictureOriginal(id, fileExtension);
                        imageStorageUri = await fileService.UploadImageAsync(blobInformation.BlobContainerName, blobInformation.FileName, fileData).ConfigureAwait(false);
                        await eventService.UpdateLogoAsync(id, imageStorageUri).ConfigureAwait(false);
                    }
                    else
                        throw new ArgumentException($"Event id: {id}, not exist");
                    break;
                case "PERSON":
                    if (await personService.ExistsAsync(id))
                    {
                        blobInformation = ImageStructure.PersonPictureOriginal(id, fileExtension);
                        imageStorageUri = await fileService.UploadImageAsync(blobInformation.BlobContainerName, blobInformation.FileName, fileData);
                        await personService.UpdateImageAsync(id, imageStorageUri);
                    }
                    else
                        throw new ArgumentException($"Person id: {id}, not exist");
                    break;
            }
            return new OkObjectResult(new { ImageUrl = imageStorageUri });
        }
    }
}