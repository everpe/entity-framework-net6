using AutoMapper;
using EFcore.Api.Peliculas.DTOs;
using EFcore.Api.Peliculas.Entidades;
using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace EFcore.Api.Peliculas.Services
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //Mapeos Necesarios Para mostrar la Info relacionada con una Actor
            CreateMap<Actor, ActorDTO>();
            CreateMap<Cine, CineDTO>()
                .ForMember(dto => dto.Latitud, ent => ent.MapFrom(prop => prop.Ubicacion.Y))
                .ForMember(dto => dto.Longitud, ent => ent.MapFrom(prop => prop.Ubicacion.X));



            //Mapeos Necesarios Para mostrar la Info relacionada con una Pelicula
            CreateMap<Genero, GeneroDTO>();

            //Sin ProjectTo
            CreateMap<Pelicula, PeliculaDTO>()
                .ForMember(dto => dto.Cines, ent => ent.MapFrom(//guarda en propiedad Cines del Dto,
                                            peli => peli.SalasCine.Select(sala => sala.Cine)))//los cines correspondientes a SalasCine de la Pelicula 
                .ForMember(dto => dto.Actores, ent => //guarda en propiedad Actores del Dto,
                    ent.MapFrom(peli => peli.PeliculasActores.Select(pAct => pAct.Actor)));//los ACTORES correspondientes a PeliculasActores de la Pelicula 
            // Con ProjectTo
            //CreateMap<Pelicula, PeliculaDTO>()
            //    .ForMember(dto => dto.Generos, ent => ent.MapFrom(prop =>
            //        prop.Generos.OrderByDescending(g => g.Nombre)))
            //    .ForMember(dto => dto.Cines, ent => ent.MapFrom(prop => prop.SalasCine.Select(s => s.Cine)))
            //    .ForMember(dto => dto.Actores, ent =>
            //        ent.MapFrom(prop => prop.PeliculasActores.Select(pa => pa.Actor)));


            //Mapeos Para Creación de Cines y sus relaciones
            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
            CreateMap<CineCreacionDTO, Cine>()
                .ForMember(ent => ent.Ubicacion, 
                    dto => dto.MapFrom(campo => 
                        geometryFactory.CreatePoint(new Coordinate(campo.Longitud, campo.Latitud))));

            CreateMap<OfertaCineCreacionDTO, OfertaCine>();
            CreateMap<SalaCineCreacionDTO, SalaCine>();


            //Creacion Peliculas con Data relacionada de BD
            CreateMap<PeliculaCreacionDTO, Pelicula>()
           .ForMember(ent => ent.Generos,
               dto => dto.MapFrom(campo => campo.Generos.Select(id => new Genero() { Identificador = id })))
           .ForMember(ent => ent.SalasCine,
               dto => dto.MapFrom(campo => campo.SalasDeCine.Select(id => new SalaCine() { Id = id })));

            CreateMap<PeliculaActorCreacionDTO, PeliculaActor>();

            CreateMap<ActorCreacionDTO, Actor>();
        }

    }
}
