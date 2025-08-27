using System;
using AutoMapper;
using BibliotecaAPI.DTOs;
using BibliotecaAPI.Entidades;

namespace BibliotecaAPI.Utilidades
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Autor, AutorDTO>()
                .ForMember(dto => dto.NombreCompleto,
                    config => config.MapFrom(autor => $"{autor.Nombres} {autor.Apellidos}"));
            CreateMap<AutorCreacionDTO, Autor>();
            CreateMap<Libro, LibroDTO>();
            CreateMap<LibroCreacionDTO, Libro>();
        }
    }
}
