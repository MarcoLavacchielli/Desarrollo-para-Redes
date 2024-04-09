using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ServerFullAuth : MonoBehaviour
{
    public Entity prefabPlayer;
    public Vector2 minLimits;
    public Vector2 maxLimits;
    public List<Entity> onlinePlayers = new List<Entity>();
    IController firstController;
    IController secondController;

    int _maxPlayers = 2;

    void Awake()
    {
        //Esto en caso de red se va a instanciar desde cada cliente en si y no
        //desde el servidor, lo importante es que el player no es quien lo crea

        /*Setearle a firstController un nuevo FirstPlayerController*/
        var firstController = GetComponent<FirstPlayerController>();
        firstController = new FirstPlayerController();
        secondController = new SecondPlayerController();
    }

    private void NewPlayer()
    {
        /*Si la cantidad de onlinePlayers es igual a 0
         --Instanciar el prefabPlayer
         --Setearle una posicion random (new Vector3(Random.Range(minLimits.x, maxLimits.x), 1, Random.Range(minLimits.y, maxLimits.y));)
         --A firstController SETearle el MODEL player
         */

        if(onlinePlayers.Count == 0)
        {
            var newPlayer = Instantiate(prefabPlayer);
            newPlayer.transform.position = new Vector3(Random.Range(minLimits.x, maxLimits.x), 1, Random.Range(minLimits.y, maxLimits.y));
            firstController.SetModel(newPlayer);
        }

        if (onlinePlayers.Count == 1)
        {
            var newPlayer = Instantiate(prefabPlayer);
            newPlayer.transform.position = new Vector3(Random.Range(minLimits.x, maxLimits.x), 1, Random.Range(minLimits.y, maxLimits.y));
            secondController.SetModel(newPlayer);
        }
    }

    void Update()
    {
        /*Para cada onlinePlayer
         --ejecutar el executeUpdate*/
        foreach(var player in onlinePlayers)
        {
            player.ExecuteUpdate();
        }

        if (Input.GetKeyDown(KeyCode.F1))
            NewPlayer();
    }

    public void AddPlayer(Entity p)
    {
        /*Si onlinePlayer no contiene a la entidad del parametro
         --Agregarla*/

        if (onlinePlayers.Contains(p)) return;
        onlinePlayers.Add(p);
    }

    public Vector3 RequestMovement(Vector3 actualPos, Vector3 newPos)
    {
        /*Si la nueva posicion en x y en z excede los limites de maxLimits
         --devolver la posicion actual*/

        if (newPos.x > maxLimits.x || newPos.x < minLimits.x || newPos.z > maxLimits.y || newPos.z < minLimits.y) return actualPos;

        /*Devolver la nueva posicion*/

        return newPos; //Ignorar esto, es para que la funcion no tire error por no devolver algo
    }

    public void RemovePlayer(Entity p)
    {
        /*REMOVEr el MODEL de firstController*/
        firstController.RemoveModel();
        secondController.RemoveModel();
        /*Remover a la entidad del parametro de onlinePlayers*/
        onlinePlayers.Remove(p);
    }
}
