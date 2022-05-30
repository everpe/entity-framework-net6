namespace EFcore.Api.Peliculas.DTOs
{
    //DTO de cine para valor ubicación de tipo Point
    public class CineDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }


    }
}
