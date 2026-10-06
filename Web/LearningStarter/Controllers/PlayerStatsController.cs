
using System.Linq;
using LearningStarter.Common;
using LearningStarter.Data;
using LearningStarter.Entities;
using Microsoft.AspNetCore.Mvc;

namespace LearningStarter.Controllers;

[ApiController]
[Route("api/playerstats")]
public class PlayerStatsController : ControllerBase
{
    private readonly DataContext _dataContext;

    public PlayerStatsController(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var response = new Response();

        var data = _dataContext.Set<PlayerStats>()
            .Select(playerStats => new PlayerStatsGetDto
            {
                Id = playerStats.Id,
                Player = playerStats.Player,
                Team = playerStats.Team,
                StatType = playerStats.StatType,
                StatValue = playerStats.StatValue,
                GameDate = playerStats.GameDate
            })
            .ToList();

        response.Data = data;
        return Ok(response);
    }

    [HttpPost]
    public IActionResult Create([FromBody] PlayerStatsCreateDto createDto)
    {
        var response = new Response();

        if (string.IsNullOrEmpty(createDto.Team))
        {
            response.AddError(nameof(createDto.Team), "Team is required");
        }

        if (response.HasErrors)
        {
            return BadRequest(response);
        }

        var playerStatsToCreate = new PlayerStats
        {
            Player = createDto.Player,
            Team = createDto.Team,
            StatType = createDto.StatType,
            StatValue = createDto.StatValue,
            GameDate = createDto.GameDate
        };

        _dataContext.Set<PlayerStats>().Add(playerStatsToCreate);
        _dataContext.SaveChanges();

        var playerStatsToReturn = new PlayerStatsGetDto
        {
            Id = playerStatsToCreate.Id,
            Player = playerStatsToCreate.Player,
            Team = playerStatsToCreate.Team,
            StatType = playerStatsToCreate.StatType,
            StatValue = playerStatsToCreate.StatValue,
            GameDate = playerStatsToCreate.GameDate
        };

        response.Data = playerStatsToReturn;
        return Created("", response);
    }

    [HttpPut("{id}")]
    public IActionResult Update(
        [FromBody] PlayerStatsUpdateDto updateDto, int id)
    {
        var response = new Response();

        var playerStatsToUpdate = _dataContext.Set<PlayerStats>()
            .FirstOrDefault(playerStats => playerStats.Id == id);

        if (playerStatsToUpdate == null)
        {
            response.AddError("id", "Player stats not found");
        }

        if (response.HasErrors)
        {
            return BadRequest(response);
        }

        playerStatsToUpdate.Player = updateDto.Player;
        playerStatsToUpdate.Team = updateDto.Team;
        playerStatsToUpdate.StatType = updateDto.StatType;
        playerStatsToUpdate.StatValue = updateDto.StatValue;
        playerStatsToUpdate.GameDate = updateDto.GameDate;

        _dataContext.SaveChanges();

        var playerStatsToReturn = new PlayerStatsGetDto
        {
            Id = playerStatsToUpdate.Id,
            Player = playerStatsToUpdate.Player,
            Team = playerStatsToUpdate.Team,
            StatType = playerStatsToUpdate.StatType,
            StatValue = playerStatsToUpdate.StatValue,
            GameDate = playerStatsToUpdate.GameDate
        };

        response.Data = playerStatsToReturn;
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var response = new Response();

        var playerStatsToDelete = _dataContext.Set<PlayerStats>()
            .FirstOrDefault(playerStats => playerStats.Id == id);

        if (playerStatsToDelete == null)
        {
            response.AddError("id", "Player stats not found");
        }

        if (response.HasErrors)
        {
            return BadRequest(response);
        }

        _dataContext.Set<PlayerStats>().Remove(playerStatsToDelete);
        _dataContext.SaveChanges();

        response.Data = true;
        return Ok(response);
    }
}
