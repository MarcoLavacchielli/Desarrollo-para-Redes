using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChangeColorPackage : NodePackage
{

    private Color _color;

    public ChangeColorPackage()
    {
        _color = Random.ColorHSV();
    }

    public override void Execute(Node node)
    {
        node.GetComponent<Renderer>().material.color = _color;
    }
}
