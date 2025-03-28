using System.Diagnostics;
using Newtonsoft.Json;
namespace sample1;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

class Personne
{
    public string Nom { get; set; }
    public int Age { get; set; }
    public Personne(string nom, int age) {
        Nom = nom;
        Age = age;
        }

    public string Hello(bool isLowercase)
    {
        string message = $"Hello {Nom}, you are {Age}";
        return isLowercase ? message.ToLower() : message.ToUpper();
    }
}


class Program
{
    static void Main(string[] args)
    {
        string inputFolder = "images"; // Chemin de l'image d'entrée
        string outputFolder = "resized_images"; // Chemin de sortie
        string[] imageFiles = Directory.GetFiles(inputFolder, "*.jpg"); // Liste des fichiers JPG

        Stopwatch sw = Stopwatch.StartNew(); // Mesure du temps d'exécution

        Parallel.ForEach(imageFiles, (inputPath) =>
        {
            string fileName = Path.GetFileName(inputPath);
            string outputPath = Path.Combine(outputFolder, "resized_" + fileName);

            try
            {
                using (Image image = Image.Load(inputPath))
                {
                    image.Mutate(x => x.Rotate(SixLabors.ImageSharp.Processing.RotateMode.Rotate180));                // Sauvegarder l'image
                    image.Save(outputPath);
                }

                Console.WriteLine($"Image traitée : {fileName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur avec {fileName} : {ex.Message}");
            }
        });

        sw.Stop();
        Console.WriteLine($"Traitement terminé en {sw.ElapsedMilliseconds} ms");
    
        /*try
        {
            using (Image image = Image.Load(inputPath))
            {
                // Transformation avec rotation à 180°
                image.Mutate(x => x.Rotate(SixLabors.ImageSharp.Processing.RotateMode.Rotate180));                // Sauvegarder l'image
                image.Save(outputPath);
            }

            Console.WriteLine($"Image redimensionnée et enregistrée : {outputPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur : {ex.Message}");
        }*/
        var personne = new Personne("Clara", 22);
        string output = JsonConvert.SerializeObject(personne, Formatting.Indented);
        Console.WriteLine(personne.Hello(true));
        Console.WriteLine(output);
        //Console.WriteLine("Hello, World!");
    }
}
