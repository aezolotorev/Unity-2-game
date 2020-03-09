using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Heli:BaseObject
{
    [SerializeField]
    

    private void Update()
    {
        _GOInstance.transform.position = _GOInstance.transform.position - Vector3.forward * 20f*Time.deltaTime;
        Destroy(_GOInstance, 100F);
    }
  

}
