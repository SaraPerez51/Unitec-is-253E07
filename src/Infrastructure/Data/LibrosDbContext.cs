using Microsoft.Data.SqlClient;
using System.Data;
using Domain.Entities;
using Application.Services;

namespace Infrastructure.Data;

public class LibrosDbContext
{
    private readonly string _connectionString;

    public LibrosDbContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    public List<IM253E07Libro> List()
    {
        var data = new List<IM253E07Libro>();
        using var con = new SqlConnection(_connectionString);
        var cmd = new SqlCommand("SELECT [Id],[Autor],[Editorial],[ISBN],[Foto] FROM [IM253E07Libro]", con);
        con.Open();
        var dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            data.Add(new IM253E07Libro
            {
                Id = (Guid)dr["Id"],
                Autor = (string)dr["Autor"],
                Editorial = dr["Editorial"] as string,
                ISBN = (string)dr["ISBN"],
                Foto = dr["Foto"] as string ?? FileConverterService.PlaceHolder
            });
        }
        return data;
    }

    public IM253E07Libro? Details(Guid id)
    {
        using var con = new SqlConnection(_connectionString);
        var cmd = new SqlCommand("SELECT [Id],[Autor],[Editorial],[ISBN],[Foto] FROM [IM253E07Libro] WHERE [Id]=@id", con);
        cmd.Parameters.Add("@id", SqlDbType.UniqueIdentifier).Value = id;
        con.Open();
        var dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            return new IM253E07Libro
            {
                Id = (Guid)dr["Id"],
                Autor = (string)dr["Autor"],
                Editorial = dr["Editorial"] as string,
                ISBN = (string)dr["ISBN"],
                Foto = dr["Foto"] as string ?? FileConverterService.PlaceHolder
            };
        }
        return null;
    }

    public void Create(IM253E07Libro libro)
    {
        using var con = new SqlConnection(_connectionString);
        var cmd = new SqlCommand("INSERT INTO [IM253E07Libro] ([Id],[Autor],[Editorial],[ISBN],[Foto]) VALUES (@id,@autor,@editorial,@isbn,@foto)", con);
        cmd.Parameters.Add("@id", SqlDbType.UniqueIdentifier).Value = libro.Id == Guid.Empty ? Guid.NewGuid() : libro.Id;
        cmd.Parameters.Add("@autor", SqlDbType.NVarChar).Value = libro.Autor;
        cmd.Parameters.Add("@editorial", SqlDbType.NVarChar).Value = (object?)libro.Editorial ?? DBNull.Value;
        cmd.Parameters.Add("@isbn", SqlDbType.NVarChar).Value = libro.ISBN;
        cmd.Parameters.Add("@foto", SqlDbType.NVarChar).Value = (object?)libro.Foto ?? DBNull.Value;
        con.Open();
        cmd.ExecuteNonQuery();
    }

    public void Edit(IM253E07Libro libro)
    {
        using var con = new SqlConnection(_connectionString);
        var cmd = new SqlCommand("UPDATE [IM253E07Libro] SET [Autor]=@autor,[Editorial]=@editorial,[ISBN]=@isbn,[Foto]=@foto WHERE [Id]=@id", con);
        cmd.Parameters.Add("@id", SqlDbType.UniqueIdentifier).Value = libro.Id;
        cmd.Parameters.Add("@autor", SqlDbType.NVarChar).Value = libro.Autor;
        cmd.Parameters.Add("@editorial", SqlDbType.NVarChar).Value = (object?)libro.Editorial ?? DBNull.Value;
        cmd.Parameters.Add("@isbn", SqlDbType.NVarChar).Value = libro.ISBN;
        cmd.Parameters.Add("@foto", SqlDbType.NVarChar).Value = (object?)libro.Foto ?? DBNull.Value;
        con.Open();
        cmd.ExecuteNonQuery();
    }

    public void Delete(Guid id)
    {
        using var con = new SqlConnection(_connectionString);
        var cmd = new SqlCommand("DELETE FROM [IM253E07Libro] WHERE [Id]=@id", con);
        cmd.Parameters.Add("@id", SqlDbType.UniqueIdentifier).Value = id;
        con.Open();
        cmd.ExecuteNonQuery();
    }
}
