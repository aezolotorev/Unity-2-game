
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Xml.Serialization;
[Serializable]
public struct Svector3
{
    public float X;
    public float Y;
    public float Z;
    public Svector3(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public static implicit operator Svector3(Vector3 val)
    {
        return new Svector3(val.x, val.y, val.z);
    }
    public static implicit operator Vector3(Svector3 val)
    {
        return new Vector3(val.X, val.Y, val.Z);
    }

}
[Serializable]
public struct SQuaternion
{
    public float X;
    public float Y;
    public float Z;
    public float W;
    public SQuaternion(float x, float y, float z, float w)
    {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }

    public static implicit operator SQuaternion(Quaternion val)
    {
        return new SQuaternion(val.x, val.y, val.z, val.w);
    }
    public static implicit operator Quaternion(SQuaternion val)
    {
        return new Quaternion(val.X, val.Y, val.Z, val.W);
    }

}
public struct SGameObject
{
    public string Name;
    public Svector3 Position;
    public Svector3 Scale;
    public SQuaternion Rotation;
}

public class SaveLVL
{
    [MenuItem("Сохранение шаблона/сохранить сцену",false,1)]
    private static void  SaveScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        List<GameObject> rootObjects = new List<GameObject>();
        scene.GetRootGameObjects(rootObjects);
        List<SGameObject> levelObjects = new List<SGameObject>();
        string savePath = Path.Combine(Application.dataPath, "EditorData.xml");
        foreach(var obj in rootObjects)
        {
            var temp = obj.transform;
            levelObjects.Add(
                new SGameObject
                {
                    Name = obj.name,
                    Position = temp.position,
                    Rotation = temp.rotation,
                    Scale = temp.localScale

                });
        }
        XMLsaviour.Save(levelObjects.ToArray(), savePath);
    }
}

public class XMLsaviour : MonoBehaviour
{

    private static XmlSerializer _formatter;
    static XMLsaviour()
    {
        _formatter = new XmlSerializer(typeof(SGameObject[]));
    }
    public static void Save(SGameObject[] levelObj, string path)
    {
        if (levelObj == null && !String.IsNullOrEmpty(path))
        {
            Debug.Log("Не задан путь или массив пуст");
            return;
        }
        if (levelObj.Length <= 0)
        {
            return;
        }
        using (FileStream fs = new FileStream(path, FileMode.Create))
        {
            _formatter.Serialize(fs, levelObj);
        }
    }

    [MenuItem("Загрузка шаблона/загрузка сцены", false, 1)]
    private static void Load()
    {
        SGameObject[] result;
        using(FileStream fs= new FileStream(Path.Combine(Application.dataPath, "EditorData.xml"), FileMode.Open))
        {
            result = (SGameObject[])_formatter.Deserialize(fs);
        }
        foreach (var o in result)
        {
            var _prefab = Resources.Load<GameObject>(o.Name);
            if (_prefab != null)
            {
                GameObject temp = Instantiate(_prefab, o.Position, o.Rotation);
                temp.name = o.Name;
            }
        }
    }
}
    

