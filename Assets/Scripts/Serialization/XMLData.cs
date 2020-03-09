using System;
using System.IO;
using UnityEngine;
using System.Xml;

public class XMLData : ISetData
{
    string _path = Path.Combine(Application.dataPath, "XMLData.xml");
    public void Save(PlayerData player)
    {
        XmlDocument xmlDoc = new XmlDocument();
        XmlNode rootNode = xmlDoc.CreateElement("Player");
        xmlDoc.AppendChild(rootNode);

        XmlElement element = xmlDoc.CreateElement("Name");
        element.SetAttribute("value", player.Name);
        rootNode.AppendChild(element);

        element = xmlDoc.CreateElement("Health");
        element.SetAttribute("value", player.Health.ToString());
        rootNode.AppendChild(element);

        element = xmlDoc.CreateElement("Visible");
        element.SetAttribute("value", player.Visible.ToString());
        rootNode.AppendChild(element);

        xmlDoc.Save(_path);
        


    }
    public PlayerData Load()
    {
        var result = new PlayerData();
        if (!File.Exists(_path))
        {
            Debug.Log("File not exist");
            return result;
        }
        using (XmlTextReader reader= new XmlTextReader(_path))
        {
            string key = "Name";
            while (reader.Read())
            {
                if (reader.IsStartElement(key))
                {
                    result.Name = reader.GetAttribute("value");
                }

                key = "Health";

                if (reader.IsStartElement(key))
                {
                    Int32.TryParse(reader.GetAttribute("value"), out result.Health);
                    
                }
                key = "Visible";
                if (reader.IsStartElement(key))
                {
                    Boolean.TryParse(reader.GetAttribute("value"), out result.Visible);
                }
                
            }
            return result;
        }
    }
}
