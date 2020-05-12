using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : PlatformState
{
    public Transform[] MovePositions;
    private int point;
    private bool inverse;

    public override void Init(PlatformController pc)
    {
        point = 1;
        inverse = false;

        MovePositions = pc.Positions.GetComponentsInChildren<Transform>();
    }

    public override void CheckTransition(PlatformController pc)
    {

    }

    public override void FixedUpdate(PlatformController pc)
    {
        if (point == MovePositions.Length - 1)
        {
            inverse = true;
        }
        if (point == 1)
        {
            inverse = false;
        }

        if (inverse)
        {
            if (pc.transform.position == MovePositions[point].transform.position)
            {
                point--;
                pc.transform.position = Vector3.MoveTowards(pc.transform.position, MovePositions[point].transform.position, pc.platform.maxSpeed * Time.deltaTime);
            }
            else
            {
                pc.transform.position = Vector3.MoveTowards(pc.transform.position, MovePositions[point].transform.position, pc.platform.maxSpeed * Time.deltaTime);
            }
        }
        else
        {
            if (pc.transform.position == MovePositions[point].transform.position)
            {
                point++;
                pc.transform.position = Vector3.MoveTowards(pc.transform.position, MovePositions[point].transform.position, pc.platform.maxSpeed * Time.deltaTime);
            }
            else
            {
                pc.transform.position = Vector3.MoveTowards(pc.transform.position, MovePositions[point].transform.position, pc.platform.maxSpeed * Time.deltaTime);
            }
        }
    }

    public override void Update(PlatformController pc)
    {

    }
}






/*
pc.platform.position = new Vector3(pc.platform.position.x, pc.platform.referenceHeight + pc.platform.amplitude Mathf.Sin(pc.platform.t), 0);
pc.platform.t += pc.platform.maxSpeed * Time.deltaTime;

if (pc.transform.position != pc.platform.position1.transform.position && pc.transform.position != pc.platform.position2.transform.position || pc.transform.position == pc.platform.position2.transform.position) 
{
    pc.transform.position = Vector3.MoveTowards(pc.transform.position + pc.platform.direction, pc.platform.position1.transform.position, pc.platform.maxSpeed * Time.deltaTime);
}
if (pc.transform.position == pc.platform.position1.transform.position && pc.transform.position != pc.platform.position2.transform.position)
{
    pc.transform.position = Vector3.MoveTowards(pc.transform.position + pc.platform.direction, pc.platform.position2.transform.position, pc.platform.maxSpeed * Time.deltaTime);
}
if (pc.transform.position == pc.platform.position2.transform.position)
{
    /pc.transform.position = Vector3.MoveTowards(pc.transform.position + pc.platform.direction, pc.platform.position1.transform.position, pc.platform.maxSpeed * Time.deltaTime);
}*/
