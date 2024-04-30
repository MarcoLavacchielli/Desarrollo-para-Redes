using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meta : NetworkBehaviour
{
    /*[SerializeField]
    private List<PlayerModel> playerScreensList = new List<PlayerModel>();*/
    [SerializeField]
    private PlayerModel firstPlayerModel;

    [SerializeField] private NetworkBool UnoGano;
    [SerializeField] private NetworkBool DosGano;

    //[SerializeField] GameObject victoryScreen, defeatScreen;

    [SerializeField] private NetworkBool termino = false;


    private GameManager gameManager;

    /* public void AddPlayerModel(PlayerModel playerModel)
     {
         playerScreensList.Add(playerModel);
     }

     private void Update()
     {

         if (termino == false)
         {
             listManagement();
         }
         if (termino == true)
         {
             playerScreensList = null;
         }

     }

     void listManagement()
     {
         PlayerModel[] playerModelsInScene = FindObjectsOfType<PlayerModel>();
         foreach (PlayerModel playerModel in playerModelsInScene)
         {
             // Chequear si el playerModel ya está en la lista antes de agregarlo
             if (!playerScreensList.Contains(playerModel))
             {
                 AddPlayerModel(playerModel);
             }
         }
     }*/

    // Método para obtener la referencia al GameManager
    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verificamos si el otro objeto tiene un componente PlayerModel
        PlayerModel playerModel = other.GetComponent<PlayerModel>();
        if (playerModel != null)
        {
            // Verificamos si el jugador está en la lista del GameManager
            if (gameManager.playerScreensList.Contains(playerModel))
            {
                // Accedemos a la lista del GameManager
                int playerIndex = gameManager.playerScreensList.IndexOf(playerModel);

                if (playerIndex == 0)
                {
                    UnoGano = true;
                    DosGano = false;
                    RpcDeclareVictory();
                }
                else if (playerIndex == 1)
                {
                    UnoGano = false;
                    DosGano = true;
                    RpcDeclareVictory();
                }
            }
        }
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    void RpcDeclareVictory()
    {
        if (UnoGano)
        {
            gameManager.playerScreensList[0].Gano();
            gameManager.playerScreensList[1].Perdio();
            termino = true;
        }
        else if (DosGano)
        {
            gameManager.playerScreensList[1].Gano();
            gameManager.playerScreensList[0].Perdio();
            termino = true;
        }
    }

}