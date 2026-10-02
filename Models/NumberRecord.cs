namespace WebApi.Models;

public record NumberRecordGet(int Id, int Numero, int Resultado, DateTime Fecha)
{
    // Constructor vacio que Dapper requiere para SqlLite para crear el objeto y luego convertir los datos
    public NumberRecordGet() : this(0, 0, 0, default) { }
}
public record NumberRecordSet(int Numero, int Resultado);