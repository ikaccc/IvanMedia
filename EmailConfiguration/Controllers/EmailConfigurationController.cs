using Microsoft.AspNetCore.Mvc;
using EmailConfiguration.Services.Interfaces;
using EmailConfiguration.Models;

namespace EmailConfiguration.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmailConfigurationController : ControllerBase
{
    private readonly IEmailConfigurationService _configurationService;

    public EmailConfigurationController(IEmailConfigurationService configurationService)
    {
        _configurationService = configurationService;
    }

    [HttpGet("GetConfigurationByClientId/{clientId}")]
    public async Task<IActionResult> GetConfigurationByClientId(int clientId)
    {
        var configuration = await _configurationService.GetConfigurationByClientIdAsync(clientId);
        if (configuration == null)
        {
            return NotFound();
        }
        return Ok(configuration);
    }   

    [HttpGet]
    public async Task<IActionResult> GetAllConfigurations()
    {
        var configurations = await _configurationService.GetAllConfigurationsAsync();
        return Ok(configurations);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetConfigurationById(int id)
    {
        var configuration = await _configurationService.GetConfigurationByIdAsync(id);
        if (configuration == null)
        {
            return NotFound();
        }
        return Ok(configuration);
    }

    [HttpPost]
    public async Task<IActionResult> CreateConfiguration([FromBody] ClientEmailConfiguration configuration)
    {
        if (configuration == null)
        {
            return BadRequest("Configuration is null.");
        }

        return Ok(await _configurationService.CreateConfigurationAsync(configuration));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateConfiguration([FromBody] ClientEmailConfiguration configuration)
    {
        if (configuration == null)
        {
            return BadRequest("Configuration is null.");
        }

        await _configurationService.UpdateConfigurationAsync(configuration);
        
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteConfiguration(int id)
    {
        var result = await _configurationService.DeleteConfigurationAsync(id);
        if (result)
        {
            return NoContent();
        }
        return NotFound();
    }
}
