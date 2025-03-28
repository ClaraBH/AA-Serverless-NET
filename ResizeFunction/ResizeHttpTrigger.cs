using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Company.Function
{
    public static class ResizeHttpTrigger
    {
        // TODO N'accepter que le POST
        [FunctionName("ResizeHttpTrigger")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            if (!int.TryParse(req.Query["w"], out int w) || !int.TryParse(req.Query["h"], out int h))
            {
                return new BadRequestObjectResult("Les paramètres 'w' et 'h' sont requis dans la query string et doivent être des entiers.");
            }

            byte[]  targetImageBytes;
            using(var  msInput = new MemoryStream())
            {
                // Récupère le corps du message en mémoire
                await req.Body.CopyToAsync(msInput);
                msInput.Position = 0;

                // Charge l'image       
                using (var image = Image.Load(msInput)) 
                {
                    // Effectue la transformation
                    image.Mutate(x => x.Rotate(SixLabors.ImageSharp.Processing.RotateMode.Rotate180));                // Sauvegarder l'image

                    // Sauvegarde en mémoire               
                    using (var msOutput = new MemoryStream())
                    {
                        image.SaveAsJpeg(msOutput);
                        targetImageBytes = msOutput.ToArray();
                    }
                }
            }
            // Renvoie le contenu avec le content-type correspondant à une image jpeg
            // TODO renvoyer les octets de l'image
            // TODO ... ainsi que le content-type correspondant à une image Jpeg
            return new FileContentResult(targetImageBytes, "image/jpeg");        }
    }
}