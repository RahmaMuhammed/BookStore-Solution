using AutoMapper;
using BookStore.Core.Entities;
using BookStore.Core.Repositories;
using BookStore.Core.Specifications;
using BookStore.DTOs;
using BookStore.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;

namespace BookStore.Controllers
{
    public class GenreController : ApiBaseController
    {
        private readonly IGenericRepository<BookGenre> _gener;
        private readonly IMapper _mapper;

        public GenreController(IGenericRepository<BookGenre> gener, IMapper mapper) 
        {
            _gener = gener;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(typeof(BookGenreDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IReadOnlyList<BookGenreDto>>> GetAllGenres()
        {
            var spec = new BaseSpecifications<BookGenre>();
            var genre = await _gener.GetAllAsync(spec);
            var MapGener = _mapper.Map<IReadOnlyList<BookGenre>, IReadOnlyList<BookGenreDto>>(genre);
            return Ok(MapGener);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GenreDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GenreDto>> GetGenreByName(int id)
        {
            var spec = new GenreSpecifications(id);
            var genre = await _gener.GetByIdAsync(spec);
            if (genre == null) return NotFound(new ApiResponse(404));
            var MapGener = _mapper.Map<BookGenre, GenreDto>(genre);
            return Ok(MapGener);
        }
    }
}
