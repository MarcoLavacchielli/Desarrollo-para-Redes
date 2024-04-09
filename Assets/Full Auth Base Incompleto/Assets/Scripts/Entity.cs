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

    /*Crear un Action publico, se utilizara para updatear todo lo que se agregue a este*/

    protected virtual void Awake()
    {
        /*Buscar el ServerFullAuth y guardarlo en la variable server*/


        OnConnectedServer();
    }

    public virtual void ExecuteUpdate()
    {
        /*Ejecutar el Action*/
    }

    public void OnConnectedServer()
    {
        ChangeColor(Color.green);

        /*Agregarse al server*/
    }

    public void OnDisconnectedServer()
    {
        ChangeColor(Color.red);

        /*Removerse del server*/


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
