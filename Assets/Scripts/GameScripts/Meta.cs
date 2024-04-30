using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meta : NetworkBehaviour
{
    [SerializeField] GameObject victoryScreen, defeatScreen;
    [SerializeField] NetworkBool victoryDeclared = false;

    private void OnTriggerEnter(Collider other)
    {
        // Verificar si la victoria ya ha sido declarada o si este objeto no tiene autoridad de estado
        if (victoryDeclared || !Object.HasStateAuthority)
            return;

        if (other.CompareTag("Player"))
        {
            // Deshabilitar el script del jugador para evitar que continúe interactuando
            other.GetComponent<PlayerModel>().enabled = false;

            // Marcar la victoria como declarada y notificar a todos los clientes
            victoryDeclared = true;
            RpcDeclareVictory();
        }
    }

    // RPC para notificar a todos los clientes que la victoria ha sido declarada
    [Rpc(RpcSources.All, RpcTargets.All)]
    void RpcDeclareVictory()
    {
        if (Object.HasStateAuthority)
        {
            // Activar el panel de victoria en todos los clientes
            victoryScreen.SetActive(true);
        }
        else
        {
            // Si este cliente no es el propietario de la autoridad de estado, activar el panel de derrota solo en este cliente
            defeatScreen.SetActive(true);
        }
    }
}