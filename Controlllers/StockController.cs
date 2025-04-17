using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Microsoft.EntityFrameworkCore;
using api.Interfaces;
using api.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace api.Controlllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly IStockRepository _stockRepo;
        public StockController(ApplicationDBContext context, IStockRepository stockRepo)
        {
            _stockRepo = stockRepo;
            _context = context;
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllStocks([FromQuery] QueryObject query)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var stocks = await _stockRepo.GetAllStocks(query);
            var stockDtos = stocks.Select(s => s.ToStockDto());
            return Ok(stockDtos);
        }


        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetStockById(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var stock = await _stockRepo.GetStockById(id);
            if (stock == null)
            {
                return NotFound();
            }
            return Ok(stock.ToStockDto());
        }


        [HttpPost]
        public async Task<IActionResult> CreateStock([FromBody] CreateStockRequestDto stockDto) 
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var stock = stockDto.ToStockFromCreateDto();
            await _stockRepo.CreateStock(stock);
            return CreatedAtAction(nameof(GetStockById), new { id = stock.Id }, stock.ToStockDto());
        }


        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateStock([FromRoute] int id, [FromBody] UpdateStockRequestDto updateStockDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var stock = await _stockRepo.UpdateStock(id, updateStockDto);
            if (stock == null)
            {
                return NotFound();
            }
            return Ok(stock.ToStockDto());
        }


        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteStock([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var stock = await _stockRepo.DeleteStock(id);
            if (stock == null)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}