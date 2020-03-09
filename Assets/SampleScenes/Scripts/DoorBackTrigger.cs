using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBackTrigger : MonoBehaviour
{
    public DoorTwoSide doorTwoSide;



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            doorTwoSide.front = false;
            doorTwoSide.back = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            doorTwoSide.front = false;
            doorTwoSide.back = false;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            doorTwoSide.front = false;
            doorTwoSide.back = true;
        }
    }
}
