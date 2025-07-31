namespace Domain.Entities;
public class IM253E07Prestamo
{
    public Guid Id { get; set; }
    public Guid UsuarioId { get; set; }
    public Guid LibroId { get; set; }
    public DateTime FechaPrestamo { get; set; }
    public DateTime? FechaDevolucion { get; set; }

    public IM253E07Usuario? Usuario { get; set; }
    public IM253E07Libro? Libro { get; set; }
}
