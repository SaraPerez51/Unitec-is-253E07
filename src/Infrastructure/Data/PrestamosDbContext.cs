using Microsoft.Data.SqlClient;
using System.Data;
using Domain.Entities;

namespace Infrastructure.Data;

public class PrestamosDbContext
{
    private readonly string _connectionString;

    public PrestamosDbContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    public List<IM253E07Prestamo> List()
    {
        var data = new List<IM253E07Prestamo>();
        using var con = new SqlConnection(_connectionString);
        var cmd = new SqlCommand(@"
            SELECT p.[Id], p.[UsuarioId], p.[LibroId], p.[FechaPrestamo], p.[FechaDevolucion],
                   u.[Id] as UsuarioId, u.[Nombre], u.[Direccion], u.[Telefono], u.[Correo],
                   l.[Id] as LibroId, l.[Autor], l.[Editorial], l.[ISBN], l.[Foto]
            FROM [IM253E07Prestamos] p
            JOIN [IM253E07Usuario] u ON p.[UsuarioId] = u.[Id]
            JOIN [IM253E07Libro] l ON p.[LibroId] = l.[Id]", con);
        con.Open();
        var dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            data.Add(new IM253E07Prestamo
            {
                Id = (Guid)dr["Id"],
                UsuarioId = (Guid)dr["UsuarioId"],
                LibroId = (Guid)dr["LibroId"],
                FechaPrestamo = (DateTime)dr["FechaPrestamo"],
                FechaDevolucion = dr["FechaDevolucion"] is DBNull ? null : (DateTime?)dr["FechaDevolucion"],
                Usuario = new IM253E07Usuario
                {
                    Id = (Guid)dr["UsuarioId"],
                    Nombre = (string)dr["Nombre"],
                    Direccion = dr["Direccion"] as string,
                    Telefono = (string)dr["Telefono"],
                    Correo = (string)dr["Correo"]
                },
                Libro = new IM253E07Libro
                {
                    Id = (Guid)dr["LibroId"],
                    Autor = (string)dr["Autor"],
                    Editorial = dr["Editorial"] as string,
                    ISBN = (string)dr["ISBN"],
                    Foto = dr["Foto"] as string
                }
            });
        }
        return data;
    }

    public IM253E07Prestamo? Details(Guid id)
    {
        using var con = new SqlConnection(_connectionString);
        var cmd = new SqlCommand(@"
            SELECT p.[Id], p.[UsuarioId], p.[LibroId], p.[FechaPrestamo], p.[FechaDevolucion],
                   u.[Id] as UsuarioId, u.[Nombre], u.[Direccion], u.[Telefono], u.[Correo],
                   l.[Id] as LibroId, l.[Autor], l.[Editorial], l.[ISBN], l.[Foto]
            FROM [IM253E07Prestamos] p
            JOIN [IM253E07Usuario] u ON p.[UsuarioId] = u.[Id]
            JOIN [IM253E07Libro] l ON p.[LibroId] = l.[Id]
            WHERE p.[Id]=@id", con);
        cmd.Parameters.Add("@id", SqlDbType.UniqueIdentifier).Value = id;
        con.Open();
        var dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            return new IM253E07Prestamo
            {
                Id = (Guid)dr["Id"],
                UsuarioId = (Guid)dr["UsuarioId"],
                LibroId = (Guid)dr["LibroId"],
                FechaPrestamo = (DateTime)dr["FechaPrestamo"],
                FechaDevolucion = dr["FechaDevolucion"] is DBNull ? null : (DateTime?)dr["FechaDevolucion"],
                Usuario = new IM253E07Usuario
                {
                    Id = (Guid)dr["UsuarioId"],
                    Nombre = (string)dr["Nombre"],
                    Direccion = dr["Direccion"] as string,
                    Telefono = (string)dr["Telefono"],
                    Correo = (string)dr["Correo"]
                },
                Libro = new IM253E07Libro
                {
                    Id = (Guid)dr["LibroId"],
                    Autor = (string)dr["Autor"],
                    Editorial = dr["Editorial"] as string,
                    ISBN = (string)dr["ISBN"],
                    Foto = dr["Foto"] as string
                }
            };
        }
        return null;
    }

    public void Create(IM253E07Prestamo prestamo)
    {
        using var con = new SqlConnection(_connectionString);
        var cmd = new SqlCommand("INSERT INTO [IM253E07Prestamos] ([Id],[UsuarioId],[LibroId],[FechaPrestamo],[FechaDevolucion]) VALUES (@id,@usuarioId,@libroId,@fechaPrestamo,@fechaDevolucion)", con);
        cmd.Parameters.Add("@id", SqlDbType.UniqueIdentifier).Value = prestamo.Id == Guid.Empty ? Guid.NewGuid() : prestamo.Id;
        cmd.Parameters.Add("@usuarioId", SqlDbType.UniqueIdentifier).Value = prestamo.UsuarioId;
        cmd.Parameters.Add("@libroId", SqlDbType.UniqueIdentifier).Value = prestamo.LibroId;
        cmd.Parameters.Add("@fechaPrestamo", SqlDbType.SmallDateTime).Value = prestamo.FechaPrestamo;
        cmd.Parameters.Add("@fechaDevolucion", SqlDbType.SmallDateTime).Value = (object?)prestamo.FechaDevolucion ?? DBNull.Value;
        con.Open();
        cmd.ExecuteNonQuery();
    }

    public void Edit(IM253E07Prestamo prestamo)
    {
        using var con = new SqlConnection(_connectionString);
        var cmd = new SqlCommand("UPDATE [IM253E07Prestamos] SET [UsuarioId]=@usuarioId,[LibroId]=@libroId,[FechaPrestamo]=@fechaPrestamo,[FechaDevolucion]=@fechaDevolucion WHERE [Id]=@id", con);
        cmd.Parameters.Add("@id", SqlDbType.UniqueIdentifier).Value = prestamo.Id;
        cmd.Parameters.Add("@usuarioId", SqlDbType.UniqueIdentifier).Value = prestamo.UsuarioId;
        cmd.Parameters.Add("@libroId", SqlDbType.UniqueIdentifier).Value = prestamo.LibroId;
        cmd.Parameters.Add("@fechaPrestamo", SqlDbType.SmallDateTime).Value = prestamo.FechaPrestamo;
        cmd.Parameters.Add("@fechaDevolucion", SqlDbType.SmallDateTime).Value = (object?)prestamo.FechaDevolucion ?? DBNull.Value;
        con.Open();
        cmd.ExecuteNonQuery();
    }

    public void Delete(Guid id)
    {
        using var con = new SqlConnection(_connectionString);
        var cmd = new SqlCommand("DELETE FROM [IM253E07Prestamos] WHERE [Id]=@id", con);
        cmd.Parameters.Add("@id", SqlDbType.UniqueIdentifier).Value = id;
        con.Open();
        cmd.ExecuteNonQuery();
    }
}
