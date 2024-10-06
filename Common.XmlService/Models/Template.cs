using System;
using System.Xml.Serialization;

namespace Common.XmlService.Models;

public class Template
{
    [XmlAttribute("Id")]
    public string Id { get; set; }

    [XmlElement("Name")]
    public string Name { get; set; }

    [XmlElement("MarketingData")]
    public string MarketingData { get; set; }
}