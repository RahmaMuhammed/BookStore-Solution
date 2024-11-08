using AutoMapper;
using BookStore.Core.Entities;
using BookStore.Core.Repositories;
using BookStore.Core.Specifications;
using BookStore.DTOs;
using BookStore.Errors;
using BookStore.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer.Server;

namespace BookStore.Controllers
{
    public class FormatController : ApiBaseController
    {
        private readonly IGenericRepository<BookFormat> _formats;
        private readonly IMapper _mapper;

        public FormatController(IGenericRepository<BookFormat> formats, IMapper mapper)
        {
            _formats = formats;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(typeof(BookFormatDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IReadOnlyList<BookFormatDto>>> GetAllFormatsAsync()
        {
            var spec = new BaseSpecifications<BookFormat>();
            var format = await _formats.GetAllAsync(spec);
            var MapFormat = _mapper.Map<IReadOnlyList<BookFormat>, IReadOnlyList<BookFormatDto>>(format);
            return Ok(MapFormat);
        }
        [HttpGet("{Name}")]
        [ProducesResponseType(typeof(FormatDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<FormatDto>> GetByNameAsync()
        {
            var spec = new BaseSpecifications<BookFormat>();
            var format = await _formats.GetByNameAsync(spec);
            if (format == null) return NotFound(new ApiResponse(404));
            var MapFormat = _mapper.Map<BookFormat,FormatDto>(format);
            return Ok(MapFormat);
        }
    }
}
