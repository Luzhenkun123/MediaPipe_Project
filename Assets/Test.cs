using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Transform target;
    public Direction direction;
    private void Update()
    {
        switch (direction)
        {
            case Direction.Up:
                transform.rotation = Quaternion.LookRotation(target.forward) * Quaternion.Euler(90, 0,0);
                break;
            case Direction.Down:
                transform.rotation = Quaternion.LookRotation(target.forward) * Quaternion.Euler(-90, 0, 0);
                break;
            case Direction.Left:
                transform.rotation = Quaternion.LookRotation(target.forward)*Quaternion.Euler(0,90,0);
                break;
            case Direction.Right:
                transform.rotation = Quaternion.LookRotation(target.forward) * Quaternion.Euler(0, -90, 0);
                break;
            case Direction.Forward:
                transform.rotation = Quaternion.LookRotation(target.forward);
                break;
            case Direction.Back:
                transform.rotation = Quaternion.LookRotation(-target.forward);
                break;
        }
    }
}
