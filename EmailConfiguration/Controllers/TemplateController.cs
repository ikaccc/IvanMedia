using EmailConfiguration.Models;
using EmailConfiguration.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmailConfiguration.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TemplateController : ControllerBase
{
     private readonly ITemplateService _templateService;

    public TemplateController(ITemplateService templateService)
    {
        _templateService = templateService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTemplates()
    {
        var templates = await _templateService.GetAllTemplatesAsync();
        return Ok(templates);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTemplateById(int id)
    {
        var template = await _templateService.GetTemplateByIdAsync(id);
        if (template == null)
        {
            return NotFound();
        }
        return Ok(template);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTemplate([FromBody] EmailTemplate template)
    {
        if (template == null)
        {
            return BadRequest("Template is null.");
        }

        return Ok(await _templateService.CreateTemplateAsync(template));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTemplate([FromBody] EmailTemplate template)
    {
        if (template == null)
        {
            return BadRequest("Template is null.");
        }

        var updatedTemplate = await _templateService.UpdateTemplateAsync(template);

        if (updatedTemplate == null)
        {
            return NotFound();
        }
        return Ok(updatedTemplate);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTemplate(int id)
    {
        var result = await _templateService.DeleteTemplateAsync(id);
        
        if (!result)
        {
            return NotFound();
        }
        
        return NoContent();
    }
}

