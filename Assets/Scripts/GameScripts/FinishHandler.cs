using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class FinishHandler : MonoBehaviour
{
    
    public static FinishHandler Instance { get; private set; }

    [SerializeField] GameObject _finishCanvasPrefab;

    List<FinishCanvas> _finishCanvasList = new List<FinishCanvas>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void CreateFinishScreen(PlayerModel target)
    {
        Instantiate(_finishCanvasPrefab);
    }

}
