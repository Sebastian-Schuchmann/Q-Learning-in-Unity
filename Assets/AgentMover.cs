using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : IEquatable<State>
{
    public int posX = 0;
    public int posY = 0;
    public int targetPosX = 0;
    public int targetPosY = 0;

    public State(Transform Agent, Transform Target)
    {
        this.posX = (int) Agent.position.x;
        this.posY = (int) Agent.position.z;
        this.targetPosX = (int) Target.position.x;
        this.targetPosY = (int) Target.position.z;
    }

    public State(int posX, int posY, int targetPosX, int targetPosY)
    {
        this.posX = posX;
        this.posY = posY;
        this.targetPosX = targetPosX;
        this.targetPosY = targetPosY;
    }

    public bool Equals(State other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return posX == other.posX && posY == other.posY && targetPosX == other.targetPosX && targetPosY == other.targetPosY;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((State) obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = posX;
            hashCode = (hashCode * 397) ^ posY;
            hashCode = (hashCode * 397) ^ targetPosX;
            hashCode = (hashCode * 397) ^ targetPosY;
            return hashCode;
        }
    }
}


public class AgentMover : MonoBehaviour
{
    public Target Target;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) Move(3);
        if (Input.GetKeyDown(KeyCode.DownArrow)) Move(1);
        if (Input.GetKeyDown(KeyCode.LeftArrow)) Move(0);
        if (Input.GetKeyDown(KeyCode.RightArrow)) Move(2);
    }

    public (State state, int reward) Move(int direction)
    {
        int reward = 0;
        Vector2 offset = Vector2.zero;

        switch (direction)
        {
            case 0:
                offset = new Vector2(1, 0);
                break;
            case 1:
                offset = new Vector2(0, 1);
                break;
            case 2:
                offset = new Vector2(-1, 0);
                break;
            case 3:
                offset = new Vector2(0, -1);
                break;
        }

        Vector3 newPos = new Vector3(transform.position.x + offset.x, 0, transform.position.z + offset.y);

        if (Mathf.Abs(newPos.x) <= ArenaScaler.instance.Size && Mathf.Abs(newPos.z) <= ArenaScaler.instance.Size)
        {
            transform.position = newPos;

            if (transform.position.Equals(Target.transform.position))
            {
                Target.Collect();
                reward = 1;
            }
        }

        return (new State(transform, Target.transform), reward);
    }

    public State GetState()
    {
        return new State(transform, Target.transform);
    }
}