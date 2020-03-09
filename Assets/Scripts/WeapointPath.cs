
using System.Collections.Generic;
using UnityEngine;

public class WeapointPath : MonoBehaviour
{
    public List<Transform> nodes = new List<Transform>();
    public Vector3 currNode;
    public Vector3 prevNode;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (transform.childCount > 0)
        {
            foreach(Transform T in transform)
            {
                if (!nodes.Contains(T))
                {
                    nodes.Add(T);
                }
            }
        }
        if (nodes.Count > 2)
        {
            for(int i=0; i < nodes.Count; i++)
            {
                currNode = nodes[i].position;
                if (i > 0)
                {
                    prevNode = nodes[i - 1].position;

                }
                else if(i==0 && nodes.Count > 1)
                {
                    prevNode = nodes[nodes.Count - 1].position;
                }
                Gizmos.color = Color.red;
                Gizmos.DrawLine(prevNode, currNode);
                Gizmos.color = Color.blue;
                Gizmos.DrawCube(currNode, Vector3.one);
            }
        }
    }
}
