
using UnityEditor;
using UnityEngine;

public class Window:EditorWindow
{
    public GameObject botPref;
    public string _name = "Bot";
    public int objectCounter;
    public float radius=20;

    [MenuItem("Создание префабов/Окно генератора ботов")]
    public static void showWindow()
    {
        GetWindow(typeof(Window));
    }
    private void OnGUI()
    {
        EditorGUILayout.LabelField("Настройки", EditorStyles.boldLabel);
        botPref = EditorGUILayout.ObjectField("Префаб бота", botPref, typeof(GameObject), true) as GameObject;
        objectCounter = EditorGUILayout.IntSlider("Количество объектов", objectCounter, 1, 200);
        radius = EditorGUILayout.Slider("Радиус", radius, 10, 100);

        if(GUILayout.Button("Сгенерировать ботов"))
        {
            if (botPref)
            {
                GameObject Main = new GameObject("Mainbot");
                for(int i=0; i < objectCounter; i++)
                {
                    float angle = i * Mathf.PI * 2 / objectCounter;
                    Vector3 _position = (new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle))*radius);
                    GameObject temp = Instantiate(botPref, _position, Quaternion.identity);
                    temp.transform.parent = Main.transform;
                    temp.name += "(" + i + ")";
                }
            }
        }

    }

}

