using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{

    int _iD;
    [SerializeField] int _destinyID;

    Router _myRouter;

    [SerializeField] KeyCode _keyToSend;

    // Start is called before the first frame update
    void Start()
    {
        var routers = GetComponentsInParent<Router>();

        if (routers == null) return;

        foreach (var item in routers) 
        {
            if (item == this) continue;

            item.nodes.Add(this);

            _iD = item.nodes.Count - 1;

            _myRouter = item;

            break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(_keyToSend)) _myRouter.SendTo(_destinyID, new ChangeColorPackage());
    }

    public void Receiver(NodePackage package)
    {
        package.Execute(this);
    }
}
