using FileApp.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FileApp.Controllers;

public class HomeController : Controller {
  private readonly IFileRepository _repository;

  public HomeController(IFileRepository repository) {
    _repository = repository;
  }

  public IActionResult Index() {
    return View();
  }

  [HttpPost]
  public IActionResult Index(IFormFile file) {
    if (file is null || file.Length == 0) {
      ViewData["Message"] = "Vælg en billedfil, før du trykker upload.";
      return View();
    }

    if (!IsImage(file)) {
      ViewData["Message"] = "Kun billedfiler (jpg, png, gif) er tilladt.";
      return View();
    }

    using (Stream stream = file.OpenReadStream())
      _repository.Save(file.FileName, stream);

    ViewData["Message"] = $"Filen '{file.FileName}' blev uploadet.";
    ViewData["ImageUrl"] = _repository.GetUrl(file.FileName);
    ViewData["FileName"] = file.FileName;

    return View();
  }

  public IActionResult Download(string fileName) {
    Stream? stream = _repository.Open(fileName);

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
