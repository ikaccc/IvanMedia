using System.Xml.Serialization;
using Common.XmlService.Models;

namespace Common.XmlService;

public class XmlService
{
    public static Clients DeserializeClients(string xml)
    {
        var serializer = new XmlSerializer(typeof(Clients));
        using (var reader = new StringReader(xml))
        {
            return (Clients)serializer.Deserialize(reader);
        }
    }

}

