using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnArea
{
    public Vector2 center;
    public Vector2 size;

    public SpawnArea(Vector2 _center, Vector2 _size)
    {
        center = _center;
        size = _size;
    }
}
