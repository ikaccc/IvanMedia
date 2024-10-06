using System;
using System.Xml.Serialization;

namespace Common.XmlService.Models;

[XmlRoot("Clients")]
public class Clients
{
    [XmlElement("Client")]
    public List<Client> ClientList { get; set; }
}
