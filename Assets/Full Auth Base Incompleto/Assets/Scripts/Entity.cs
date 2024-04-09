using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Es una funcion abstract porque realmente no va a estar seteada en ningun objeto, simplemente va a utilizarse como herencia.
//Cada script que sea un player va a heredar de esta clase
public abstract class Entity : MonoBehaviour
{
    protected ServerFullAuth server;
    protected float speed;
    internal Vector3 position;

    /*Crear un Action publico, se utilizara para updatear todo lo que se agregue a este*/
    public event Action onUpdate = delegate { };

    protected virtual void Awake()
    {
        /*Buscar el ServerFullAuth y guardarlo en la variable server*/
        server = FindAnyObjectByType<ServerFullAuth>();


        OnConnectedServer();
    }

    public virtual void ExecuteUpdate()
    {
        /*Ejecutar el Action*/
        onUpdate();
    }

    public void OnConnectedServer()
    {
        ChangeColor(Color.green);

        /*Agregarse al server*/
        server.AddPlayer(this);
    }

    public void OnDisconnectedServer()
    {
        ChangeColor(Color.red);

        /*Removerse del server*/

        server.RemovePlayer(this);


        StartCoroutine(DestroyPlayer());
    }

    IEnumerator DestroyPlayer()
    {
        yield return new WaitForSeconds(2.5f);
        Destroy(gameObject);
    }

    void ChangeColor(Color newColor)
    {
        GetComponent<Renderer>().material.color = newColor;
    }

    public abstract void Move(Vector3 dir);
}
