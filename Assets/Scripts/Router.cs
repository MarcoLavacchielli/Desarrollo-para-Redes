using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Router : Node
{

    public List<Node> nodes = new List<Node>();


    public override void Rename(string ID, int index)
    {
        base.Rename(ID, index);

        for (int i = 0;i < nodes.Count; i++)
        {
            nodes[i].Rename(ownID, i);
        }
    }

    public override void Receiver(string ID, NodePackage package)
    {
       SendTo(ID, package);
    }

    public void Register(Node n)
    {
        nodes.Add(n);
        n.Rename(ownID, nodes.Count - 1);
    }

    public void SendTo(string ID, NodePackage package)
    {
        if(ID.StartsWith(ownID))
        {
            for (int i = 0; i < nodes.Count; i++) 
            {
                if (ID.StartsWith(nodes[i].ownID))
                {
                    nodes[i].Receiver(ID, package);
                }

            }
        }
        else myRouter?.SendTo(ID, package);

        //Condicional ternario
        //if ?
        //else :
        //else if : condicion ?
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        foreach (Node node in nodes)
        {
            Gizmos.DrawLine(transform.position, node.transform.position);
        }
    }
}
