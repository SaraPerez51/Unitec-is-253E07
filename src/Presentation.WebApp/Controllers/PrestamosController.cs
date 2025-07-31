using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Domain.Entities;
using Infrastructure.Data;

namespace Presentation.WebApp.Controllers;

public class PrestamosController : Controller
{
    private readonly PrestamosDbContext _prestamosDbContext;
    private readonly UsuariosDbContext _usuariosDbContext;
    private readonly LibrosDbContext _librosDbContext;

    public PrestamosController(IConfiguration configuration)
    {
        var conn = configuration.GetConnectionString("DefaultConnection")!;
        _prestamosDbContext = new PrestamosDbContext(conn);
        _usuariosDbContext = new UsuariosDbContext(conn);
        _librosDbContext = new LibrosDbContext(conn);
    }

    public IActionResult Index()
    {
        var data = _prestamosDbContext.List();
        return View(data);
    }

    [HttpGet]
    public IActionResult Details(Guid id)
    {
        var data = _prestamosDbContext.Details(id);
        if (data == null)
            return NotFound();

        // Si es una solicitud AJAX, retornamos vista parcial
        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            return PartialView("DetailsPartial", data);

        // Si no, devolvemos la vista normal completa
        return View(data);
    }

    public IActionResult Create()
    {
        ViewBag.UsuarioId = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_usuariosDbContext.List(), "Id", "Nombre");
        ViewBag.LibroId = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_librosDbContext.List(), "Id", "ISBN");
        return View();
    }

    [HttpPost]
    public IActionResult Create(IM253E07Prestamo prestamo)
    {
        prestamo.Id = Guid.NewGuid();
        _prestamosDbContext.Create(prestamo);
        return RedirectToAction("Index");
    }

    public IActionResult Edit(Guid id)
    {
        var data = _prestamosDbContext.Details(id);
        if (data == null) return NotFound();
        ViewBag.UsuarioId = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_usuariosDbContext.List(), "Id", "Nombre", data.UsuarioId);
        ViewBag.LibroId = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_librosDbContext.List(), "Id", "ISBN", data.LibroId);
        return View(data);
    }

    [HttpPost]
    public IActionResult Edit(IM253E07Prestamo prestamo)
    {
        _prestamosDbContext.Edit(prestamo);
        return RedirectToAction("Index");
    }

    public IActionResult Delete(Guid id)
    {
        _prestamosDbContext.Delete(id);
        return RedirectToAction("Index");
    }
}
