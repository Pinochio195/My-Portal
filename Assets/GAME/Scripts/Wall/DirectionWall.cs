using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionWall : MonoBehaviour
{
    public enum Direction
    {
        _90=90,
        _Negative90=-90,
        _0=0,
        _180=180
    }

    public Direction _direction;
}