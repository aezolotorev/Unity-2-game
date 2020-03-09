using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class StreamData : ISetData
{
    string _path = Path.Combine(Application.dataPath, "Stream.xyz");

    public void Save(PlayerData player)

    {
        using(StreamWriter tmpWriter= new StreamWriter(_path))
        {
            tmpWriter.WriteLine(player.Name);
            tmpWriter.WriteLine(player.Health);
            tmpWriter.WriteLine(player.Visible);
        }
    }

    public  PlayerData Load()
    {
        var result = new PlayerData();
        if (!File.Exists(_path))
        {
            Debug.Log("File not exist");
            return result;
        }
        using (StreamReader TmpReader = new StreamReader(_path))
        {
            while (!TmpReader.EndOfStream)
            {
                result.Name = TmpReader.ReadLine();
                Int32.TryParse(TmpReader.ReadLine(), out result.Health);
                Boolean.TryParse(TmpReader.ReadLine(), out result.Visible);
                
            }
           
        }
        return result;
    }
}
