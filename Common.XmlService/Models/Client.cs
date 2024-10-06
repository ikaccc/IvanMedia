using System;
using System.Xml.Serialization;

namespace Common.XmlService.Models;

public class Client
{
    [XmlAttribute("ID")]
    public string Id { get; set; }

    [XmlElement("Template")]
    public Template Template { get; set; }
}
