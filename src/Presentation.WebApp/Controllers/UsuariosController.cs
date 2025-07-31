using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Domain.Entities;
using Infrastructure.Data;

namespace Presentation.WebApp.Controllers;

public class UsuariosController : Controller
{
    private readonly UsuariosDbContext _usuariosDbContext;

    public UsuariosController(IConfiguration configuration)
    {
        _usuariosDbContext = new UsuariosDbContext(configuration.GetConnectionString("DefaultConnection")!);
    }

    // Muestra la lista de usuarios
    public IActionResult Index()
    {
        var data = _usuariosDbContext.List();
        return View(data);
    }

    // Muestra detalles del usuario, compatible con AJAX
    public IActionResult Details(Guid id)
    {
        var data = _usuariosDbContext.Details(id);
        if (data == null) return NotFound();

        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            return PartialView("DetailsPartial", data);

        return View(data);
    }

    // Crear nuevo usuario (GET)
    public IActionResult Create()
    {
        return View();
    }

    // Crear nuevo usuario (POST)
    [HttpPost]
    public IActionResult Create(IM253E07Usuario usuario)
    {
        usuario.Id = Guid.NewGuid();
        _usuariosDbContext.Create(usuario);
        return RedirectToAction("Index");
    }

    // Editar usuario (GET)
    public IActionResult Edit(Guid id)
    {
        var data = _usuariosDbContext.Details(id);
        if (data == null) return NotFound();
        return View(data);
    }

    // Editar usuario (POST)
    [HttpPost]
    public IActionResult Edit(IM253E07Usuario usuario)
    {
        _usuariosDbContext.Edit(usuario);
        return RedirectToAction("Index");
    }

    // Borrar usuario
    [HttpPost]
    public IActionResult Delete(Guid id)
    {
        _usuariosDbContext.Delete(id);
        return RedirectToAction("Index");
    }
}
