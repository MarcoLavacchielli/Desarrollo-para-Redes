using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPlayerController : IController
{
    /*Crear una referencia Entity*/

    public void SetModel(Entity m)
    {
        /*Setear a la referencia el parametro de la funcion*/

        /*Agregar al Action que updatea en el model la funcion ListenKeys*/
    }

    public void ListenKeys()
    {
        /*Si no hay un model referenciado, retornar*/

        /*Llamar a la funcion move del model y pasarle los axis a moverse 
         (o que escuche determinadas teclas y pase la direccion para cada uno, es a gusto)*/
    }

    public void RemoveModel()
    {
        /*Limpiar la referencia al model*/
    }


}
