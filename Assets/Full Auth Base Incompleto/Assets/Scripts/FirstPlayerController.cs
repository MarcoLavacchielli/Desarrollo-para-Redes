﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPlayerController : IController
{
    /*Crear una referencia Entity*/

    Entity _m;

    public void SetModel(Entity m)
    {
        /*Setear a la referencia el parametro de la funcion*/

        _m = m;

        /*Agregar al Action que updatea en el model la funcion ListenKeys*/

        _m.onUpdate += ListenKeys;
    }

    public void ListenKeys()
    {
        /*Si no hay un model referenciado, retornar*/

        if (!_m) return;

        /*Llamar a la funcion move del model y pasarle los axis a moverse 
         (o que escuche determinadas teclas y pase la direccion para cada uno, es a gusto)*/

        _m.Move(new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")));
    }

    public void RemoveModel()
    {
        /*Limpiar la referencia al model*/
        _m = null;
    }


}