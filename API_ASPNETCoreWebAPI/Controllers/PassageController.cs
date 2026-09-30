using API_ASPNETCoreWebAPI.Models;
using API_ASPNETCoreWebAPI.DTOs;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using API_ASPNETCoreWebAPI.Repository;
using API_ASPNETCoreWebAPI.Interfaces;

namespace API_ASPNETCoreWebAPI.Controllers;

[ApiController]
[Route("api/passages")]
public class PassagesController : ControllerBase
{
    private readonly IPassageRepository _repository;

    // "injecting interface to database" for extra flexibility, which
    // will theoretically allow me to use a different database later on
    public PassagesController(IPassageRepository repository)
    {
        _repository = repository;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPassage(string id)
    {
        var response = await _repository.GetPassageFromDatabaseAsync(id);

        if (response == null)
        {
            return NotFound(new { message = $"Passage ID '{id}' was not found." });
        }

        return Ok(response);
    }
}
