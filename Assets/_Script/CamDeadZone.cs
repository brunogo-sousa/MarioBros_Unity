using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamDeadZone : MonoBehaviour
{
    private GameObject positionsDeadZone;
    private GameObject deadZone;
    void Start()
    {
        positionsDeadZone = GameObject.Find("PositionsDeadZone");
        deadZone = GameObject.Find("DeadZone");
    }

    void Update()
    {
        deadZone.transform.position = positionsDeadZone.transform.position;
    }
}
