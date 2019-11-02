using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolAction : SSAction {

	private enum Direction { EAST, NORTH, WEST, SOUTH };
    private float posX, posZ;
    private float moveLength;
    private float speed = 1.2f;
    private bool isArrivedAnEndPoint = true;
    private Direction direction = Direction.EAST;
    private PatrolData data;

    private PatrolAction() { }

    public static PatrolAction getSSAction(Vector3 location)
    {
        PatrolAction action = CreateInstance<PatrolAction>();
        action.posX = location.x;
        action.posZ = location.z;
        action.moveLength = UnityEngine.Random.Range(4, 7);
        return action;
    }

    public override void Start()
    {
        this.gameObject.GetComponent<Animator>().SetBool("run", true);
        data = this.gameObject.GetComponent<PatrolData>();
    }

    public override void Update()
    {
        if (transform.localEulerAngles.x != 0 || transform.localEulerAngles.z != 0)
        {
            transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
        }
        if(transform.position.y != 0)
        {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }

        goPatrol();

        if(data.ifFollowPlayer && data.areaSign == data.sign)
        {
            this.destory = true;
            this.callback.SSActoinEvent(this, 0, this.gameObject);
        }
    }

    private void goPatrol()
    {
        if (isArrivedAnEndPoint)
        {
            switch (direction)
            {
                case Direction.EAST:
                    posX -= moveLength;
                    break;
                case Direction.NORTH:
                    posZ += moveLength;
                    break;
                case Direction.WEST:
                    posX += moveLength;
                    break;
                case Direction.SOUTH:
                    posZ -= moveLength;
                    break;
            }
            isArrivedAnEndPoint = false;
        }

        this.transform.LookAt(new Vector3(posX, 0, posZ));
        float distance = Vector3.Distance(transform.position, new Vector3(posX, 0, posZ));

        if(distance > 0.9)
        {
            transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(posX, 0, posZ), speed * Time.deltaTime);
        }
        else
        {
            direction += 1;
            if(direction > Direction.SOUTH)
            {
                direction = Direction.EAST;
            }
            isArrivedAnEndPoint = true;
        }
    }
}
