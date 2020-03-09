using UnityEngine;
using System.Collections;

public class SetObject : MonoBehaviour
{
    public static int Set = 0;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Set == 1)
        {
            gameObject.GetComponent<Renderer>().materials[0].shader = Shader.Find("Toon/Basic Outline");
            
        }
        if (Set == 0)
        {
            gameObject.GetComponent<Renderer>().materials[0]. shader = Shader.Find("Legacy Shaders/Bumped Specular");
        }

    }
}
