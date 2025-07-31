using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Domain.Entities;
using Infrastructure.Data;
using Application.Services;

namespace Presentation.WebApp.Controllers;

public class LibrosController : Controller
{
    private readonly LibrosDbContext _librosDbContext;

    public LibrosController(IConfiguration configuration)
    {
        _librosDbContext = new LibrosDbContext(configuration.GetConnectionString("DefaultConnection")!);
    }

    public IActionResult Index()
    {
        var data = _librosDbContext.List();
        return View(data);
    }

    public IActionResult Details(Guid id)
    {
        var data = _librosDbContext.Details(id);
        if (data == null) return NotFound();

        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            return PartialView("DetailsPartial", data);

        return View(data);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(IM253E07Libro libro, IFormFile? file)
    {
        if (file != null && file.Length > 0)
            libro.Foto = FileConverterService.ConvertToBase64(file.OpenReadStream());

        libro.Id = Guid.NewGuid();
        _librosDbContext.Create(libro);
        return RedirectToAction("Index");
    }

    public IActionResult Edit(Guid id)
    {
        var data = _librosDbContext.Details(id);
        if (data == null) return NotFound();
        return View(data);
    }

    [HttpPost]
    public IActionResult Edit(IM253E07Libro libro, IFormFile? file)
    {
        if (file != null && file.Length > 0)
            libro.Foto = FileConverterService.ConvertToBase64(file.OpenReadStream());

        _librosDbContext.Edit(libro);
        return RedirectToAction("Index");
    }

    public IActionResult Delete(Guid id)
    {
        _librosDbContext.Delete(id);
        return RedirectToAction("Index");
    }
}