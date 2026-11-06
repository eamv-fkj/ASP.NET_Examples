using Microsoft.AspNetCore.Mvc;
using PersonApp.Models;
using PersonApp.Repositories;

namespace PersonApp.Controllers;

public class HomeController : Controller {
  private readonly IPersonRepository _personRepository;
  private readonly IFileRepository _fileRepository;

  public HomeController(IPersonRepository personRepository, IFileRepository fileRepository) {
    _personRepository = personRepository;
    _fileRepository = fileRepository;
  }

  public IActionResult Index() {
    if (TempData["Message"] != null)
      ViewData["Message"] = TempData["Message"];

    IEnumerable<Person> persons = _personRepository.GetAll();
    
    return View(persons);
  }

  [HttpPost]
  public IActionResult Index(string name, IFormFile file) {
    IEnumerable<Person> persons = _personRepository.GetAll();

    if (string.IsNullOrWhiteSpace(name)) {
      ViewData["Message"] = "Indtast navn, før du registrerer.";

      return View(persons);
    }

    if (file is null || file.Length == 0) {
      ViewData["Message"] = "Vælg en billedfil, før du registrerer.";

      return View(persons);
    }

    if (!IsImage(file)) {
      ViewData["Message"] = "Kun billedfiler (jpg, png, gif) er tilladt.";

      return View(persons);
    }

    using (Stream stream = file.OpenReadStream())
      _fileRepository.Save(file.FileName, stream);

    Person person = new Person {
      Name = name,
      ImageFileName = file.FileName
    };

    _personRepository.Add(person);
    _personRepository.Save();

    TempData["Message"] = $"Personen '{person.Name}' blev registreret.";

    return RedirectToAction(nameof(Index));
  }

  public IActionResult Download(string fileName) {
    Stream? stream = _fileRepository.Open(fileName);

    if (stream is null)
      return NotFound();

    return File(stream, GetContentType(fileName), fileName);
  }

  private static string GetContentType(string fileName)
    => Path.GetExtension(fileName).ToLowerInvariant() switch {
      ".jpg" or ".jpeg" => "image/jpeg",
      ".png" => "image/png",
      ".gif" => "image/gif",
      _ => "application/octet-stream",
    };

  private static readonly byte[][] _imageSignatures = [
    [0xFF, 0xD8, 0xFF],       // jpg
    [0x89, 0x50, 0x4E, 0x47], // png
    [0x47, 0x49, 0x46, 0x38], // gif
  ];

  private static bool IsImage(IFormFile file) {
    using Stream stream = file.OpenReadStream();

    byte[] header = new byte[4];
    int read = stream.Read(header, 0, header.Length);

    foreach (byte[] signature in _imageSignatures)
      if (signature.Length <= read && header.Take(signature.Length).SequenceEqual(signature))
        return true;

    return false;
  }
}
