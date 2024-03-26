using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Router : MonoBehaviour
{

    public List<Node> nodes = new List<Node>();

    public void SendTo(int ID, NodePackage package)
    {
        if(ID >= 0 && nodes.Count > ID)
        {
            nodes[ID].Receiver(package);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        foreach(Node node in nodes)
        {
            Gizmos.DrawLine(transform.position, node.transform.position);
        }
    }
}
