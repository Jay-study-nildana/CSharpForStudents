using AutoMapper;
using WebAPI.DTOs;
using WebAPI.Models;

namespace WebAPI.Config
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<ComicBookDTO, ComicBook>();
                config.CreateMap<ComicBook, ComicBookDTO>();
            });
            return mappingConfig;
        }
    }
}
