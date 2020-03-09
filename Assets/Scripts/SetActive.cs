using UnityEngine;
using System.Collections;

public class SetActive : MonoBehaviour
{
    private RaycastHit Hit;
    private Shader Outline;
    private Shader Base;
    private Transform Main;
    private GameObject TempGO;

    void Start()
    {
        Outline = Shader.Find("Toon/Basic Outline");
        Base = Shader.Find("Legacy Shaders/Bumped Specular");
        Cursor.visible = false;
        Main = Camera.main.transform;//Добавляем ссылку на главную камеру
    }
    private void Select(bool On)
    {
        if (TempGO)  //Если ссылка на объект есть
        {
            if (On)  //Включаем обводку
            {
                TempGO.GetComponent<Renderer>().material.shader = Outline;
            }
            else
            {
                TempGO.GetComponent<Renderer>().material.shader = Base;
            }
        }
    }
    void Update()
    {
        //луч выпускается из точки в которой находится камера по направлению оси Z
        if (Physics.Raycast(Main.position, Main.forward, out Hit, 5f))
        {
            Collider target = Hit.collider;  //получаем ссылку на коллайдер
            if (target.tag == "Weapon")
            {
                if (!TempGO)  //Если ссылки нет
                {
                    TempGO = target.gameObject;//Добавляем ссылку на текщий объект в который попал луч
                }

                Select(true);//Включаем обводку
                //Если тег тот же но сменился айдишник объекта, например у нас 5-6 однотипных объектов
                if (TempGO.GetInstanceID() != target.gameObject.GetInstanceID())
                {
                    Select(false);//снимаем обводку с последнего выбранного объекта
                    TempGO = null;//обнуляем ссылку
                }
            }
            else
            {
                Select(false);//снимаем обводку с последнего выбранного объекта
            }
        }
        else
        {
            Select(false);//снимаем обводку с последнего выбранного объекта
        }
    }



}


