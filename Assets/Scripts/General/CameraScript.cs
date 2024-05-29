using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    public GameObject player;
    private float zOffset = -10;

    void LateUpdate()
    {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, zOffset);    
    }
}
