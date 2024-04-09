using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondPlayerController : IController
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

        Vector3 dir = Vector3.zero;

        if (Input.GetKey(KeyCode.I)) dir += Vector3.forward;
        else if (Input.GetKey(KeyCode.K)) dir += Vector3.back;

        if (Input.GetKey(KeyCode.J)) dir += Vector3.forward;
        else if (Input.GetKey(KeyCode.L)) dir += Vector3.back;

        _m.Move(dir);
    }

    public void RemoveModel()
    {
        /*Limpiar la referencia al model*/
        _m = null;
    }


}
