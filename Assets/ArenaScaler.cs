using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum WallType
{
    Left,
    Right,
    Front,
    Back
}
[ExecuteInEditMode]
public class ArenaScaler : MonoBehaviour
{
    [Range(1,10)]
    public int Size;

    public static ArenaScaler instance;
    
    public Transform WallLeft;
    public Transform WallLRight;
    public Transform WallFront;
    public Transform WallBack;
    public Transform Floor;

    private void Awake()
    {
        instance = this;
    }

    private void OnValidate()
    {
        float length = (Size + 0.5f) * 2;
        Floor.transform.localScale = new Vector3(length, 1f, length);

        ChangeWallSize(WallLeft, length, WallType.Left);
        ChangeWallSize(WallLRight, length, WallType.Right);
        ChangeWallSize(WallFront, length, WallType.Front);
        ChangeWallSize(WallBack, length, WallType.Back);
    }

    public void ChangeWallSize(Transform wallToChange, float length, WallType type)
    {
        Vector3 WallLeftPos = wallToChange.position;
        
        if(type == WallType.Left) WallLeftPos.x = Size + 1f;
        if(type == WallType.Right) WallLeftPos.x = -(Size + 1f);
        if(type == WallType.Front) WallLeftPos.z = Size + 1f;
        if(type == WallType.Back) WallLeftPos.z = -(Size + 1f);
        
        wallToChange.position = WallLeftPos;
        
        if (type == WallType.Back || type == WallType.Front) length += 2;
        wallToChange.localScale = new Vector3(wallToChange.localScale.x, wallToChange.localScale.y, length);
    }
}
