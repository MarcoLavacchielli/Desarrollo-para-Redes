using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public string ownID;
    [SerializeField] string _destinyID; //0.0 , 0.1, 1.0, 1.1 => 1 numero numero de router, 2 numero el orden de lista de nodo

    public Router myRouter;

    [SerializeField] KeyCode _keyToSend;

    // Start is called before the first frame update
    void Start()
    {
        var routers = GetComponentsInParent<Router>();

        if (routers == null) return;

        foreach (var item in routers) 
        {
            if (item == this) continue;

            item.Register(this);

            myRouter = item;

            break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(_keyToSend)) myRouter.SendTo(_destinyID, new ChangeColorPackage());
    }

    public virtual void Receiver(string ID, NodePackage package)
    {
        package.Execute(this);
    }

    public virtual void Rename(string ID, int index)
    {
        ownID = ID + "." + index;
        name = ID.ToString();
    }
}
