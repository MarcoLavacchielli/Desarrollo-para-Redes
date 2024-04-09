using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{

    protected override void Awake()
    {
        base.Awake();
        speed = 5;
    }

    public override void Move(Vector3 dir)
    {
        /*Setear la posicion del transform con el vector3 que devuelve la funcion de pedir movimiento del servidor
         (primer parametro que pide es la posicion actual, el segundo es la posicion a la que se movera)*/
    }
}