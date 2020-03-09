using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    private Transform Player;

    void Start()
    {
        Player = FindObjectOfType<SinglePlayer>().transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 newPosition = Player.position;
        newPosition.y = transform.position.y;
        transform.position = newPosition;
        transform.rotation = Quaternion.Euler(90f, Player.eulerAngles.y, 0f);
    }
}
