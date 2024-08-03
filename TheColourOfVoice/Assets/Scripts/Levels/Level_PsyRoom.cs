using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Level_PsyRoom : MonoBehaviour
{
    public GameObject player;
    private void Start()
    {
        /*player.transform.DOShakeRotation(1f, new Vector3(0.1f, 0.1f, 0), 10, 180, false)
            .SetLoops(-1, LoopType.Incremental);*/
        
        player.transform.DOShakePosition(1f, new Vector3(0.1f, 0.1f, 0f), 10, 180, false)
            .SetLoops(1, LoopType.Incremental);
    }
}
