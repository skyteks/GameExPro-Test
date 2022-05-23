using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Robot robot;
    public TikTakToe tikTakToe;

    void Update()
    {
        if (!tikTakToe.playersTurn || tikTakToe.gameOver)
        {
            return;
        }

        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), -Input.GetAxis("Vertical"));
        if (input.sqrMagnitude > 0f)
        {
            robot.SetNextPos(robot.lastPos + new Vector2Int(Mathf.RoundToInt(input.x), Mathf.RoundToInt(input.y)));
        }

        if (Input.GetButtonDown("Jump"))
        {
            robot.PlaceStone();
        }
    }
}
