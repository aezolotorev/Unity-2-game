using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JSONData : ISetData
{
    string _path = Path.Combine(Application.dataPath, "JSONData.xml");

public void Save(PlayerData player)
    {
        string FileJSON = JsonUtility.ToJson(player);
        File.WriteAllText(_path, FileJSON);
    }
    public PlayerData Load()
    {
        string temp = File.ReadAllText(_path);
        return JsonUtility.FromJson<PlayerData>(temp);
    }
}
