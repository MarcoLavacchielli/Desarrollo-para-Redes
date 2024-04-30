using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEndScreens : MonoBehaviour
{


    [Header("Canvas")]//
    public GameObject victoryScreen;
    public GameObject defeatScreen;
    //

    public void gano()
    {
        victoryScreen.SetActive(true);
        Destroy(defeatScreen);
    }

    public void perdio()
    {
        defeatScreen.SetActive(true);
        Destroy(victoryScreen);
    }


}
