using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Robot robot;
    public TikTakToe tikTakToe;

    void Update()
    {
        if (!tikTakToe.playersTurn || tikTakToe.gameOver || robot.isMoving)
        {
            return;
        }

        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), -Input.GetAxis("Vertical"));
        Vector2Int move = new Vector2Int(Mathf.RoundToInt(input.x), Mathf.RoundToInt(input.y));
        if (move.sqrMagnitude > 0)
        {
            Vector2Int cell = robot.lastPos + move;
            if (cell.x >= 0 && cell.x < 3)
            {
                robot.SetNextPos(cell, false);
            }
        }

        if (Input.GetButtonDown("Jump"))
        {
            robot.PlaceStone();
        }
    }
}
