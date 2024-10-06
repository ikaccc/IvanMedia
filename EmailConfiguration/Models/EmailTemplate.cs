using System;
namespace EmailConfiguration.Models;

public class EmailTemplate
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string HtmlContent { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}


