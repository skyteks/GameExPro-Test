using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotInput : MonoBehaviour
{
    public Robot robot;
    public TikTakToe tikTakToe;

    private Coroutine waitRoutine;

    void Update()
    {
        if (tikTakToe.playersTurn || tikTakToe.gameOver)
        {
            return;
        }

        if (!robot.isMoving && waitRoutine == null)
        {
            MakeMove();
        }
    }

    private void MakeMove()
    {
        Vector2Int? cell = null;

        cell = LookForWinOrBlock(TikTakToe.FieldStates.AI);
        if (cell == null)
        {
            cell = LookForWinOrBlock(TikTakToe.FieldStates.Player);
            if (cell == null)
            {
                cell = LookForCorner();
                if (cell == null)
                {
                    cell = LookForOpenSpace();
                }
            }
        }
        if (!cell.HasValue)
        {
            throw new System.Exception("No cell found. This should not be possible.");
        }
        robot.SetNextPos(cell.Value);
        waitRoutine = StartCoroutine(WaitToPlace());
    }

    IEnumerator WaitToPlace()
    {
        yield return new WaitWhile(() => robot.isMoving);
        robot.PlaceStone();
        waitRoutine = null;
    }

    private Vector2Int? LookForOpenSpace()
    {
        IList<Vector2Int> free = tikTakToe.GetFreeCells();
        foreach (Vector2Int c in free)
        {
            if (c != null)
            {
                if (tikTakToe.GetCell(c) == TikTakToe.FieldStates.Empty)
                {
                    return c;
                }
            }
        }

        return null;
    }

    private Vector2Int? LookForCorner()
    {
        if (tikTakToe.GetCell(0, 0) == TikTakToe.FieldStates.AI)
        {
            if (tikTakToe.GetCell(0, 2) == TikTakToe.FieldStates.Empty)
            {
                return new Vector2Int(0, 2);
            }
            if (tikTakToe.GetCell(2, 2) == TikTakToe.FieldStates.Empty)
            {
                return new Vector2Int(2, 2);
            }
            if (tikTakToe.GetCell(2, 0) == TikTakToe.FieldStates.Empty)
            {
                return new Vector2Int(2, 0);
            }
        }

        if (tikTakToe.GetCell(0, 2) == TikTakToe.FieldStates.AI)
        {
            if (tikTakToe.GetCell(0, 0) == TikTakToe.FieldStates.Empty)
            {
                return new Vector2Int(0, 0);
            }
            if (tikTakToe.GetCell(2, 2) == TikTakToe.FieldStates.Empty)
            {
                return new Vector2Int(2, 2);
            }
            if (tikTakToe.GetCell(2, 0) == TikTakToe.FieldStates.Empty)
            {
                return new Vector2Int(2, 0);
            }
        }

        if (tikTakToe.GetCell(2, 2) == TikTakToe.FieldStates.AI)
        {
            if (tikTakToe.GetCell(0, 0) == TikTakToe.FieldStates.Empty)
            {
                return new Vector2Int(0, 2);
            }
            if (tikTakToe.GetCell(0, 2) == TikTakToe.FieldStates.Empty)
            {
                return new Vector2Int(0, 2);
            }
            if (tikTakToe.GetCell(2, 0) == TikTakToe.FieldStates.Empty)
            {
                return new Vector2Int(2, 0);
            }
        }

        if (tikTakToe.GetCell(2, 0) == TikTakToe.FieldStates.AI)
        {
            if (tikTakToe.GetCell(0, 0) == TikTakToe.FieldStates.Empty)
            {
                return new Vector2Int(0, 2);
            }
            if (tikTakToe.GetCell(0, 2) == TikTakToe.FieldStates.Empty)
            {
                return new Vector2Int(0, 2);
            }
            if (tikTakToe.GetCell(2, 2) == TikTakToe.FieldStates.Empty)
            {
                return new Vector2Int(2, 2);
            }
        }

        if (tikTakToe.GetCell(0, 0) == TikTakToe.FieldStates.Empty)
        {
            return new Vector2Int(0, 0);
        }
        if (tikTakToe.GetCell(0, 2) == TikTakToe.FieldStates.Empty)
        {
            return new Vector2Int(0, 2);
        }
        if (tikTakToe.GetCell(2, 0) == TikTakToe.FieldStates.Empty)
        {
            return new Vector2Int(2, 0);
        }
        if (tikTakToe.GetCell(2, 2) == TikTakToe.FieldStates.Empty)
        {
            return new Vector2Int(2, 2);
        }

        return null;
    }

    private Vector2Int? LookForWinOrBlock(TikTakToe.FieldStates mark)
    {
        for (int x = 0; x < 3; x++)
        {
            if (tikTakToe.GetCell(x, 0) == TikTakToe.FieldStates.Empty && tikTakToe.GetCell(x, 1) == mark && tikTakToe.GetCell(x, 2) == mark)
            {
                return new Vector2Int(x, 0);
            }
            if (tikTakToe.GetCell(x, 0) == mark && tikTakToe.GetCell(x, 1) == TikTakToe.FieldStates.Empty && tikTakToe.GetCell(x, 2) == mark)
            {
                return new Vector2Int(x, 1);
            }
            if (tikTakToe.GetCell(x, 0) == mark && tikTakToe.GetCell(x, 1) == mark && tikTakToe.GetCell(x, 2) == TikTakToe.FieldStates.Empty)
            {
                return new Vector2Int(x, 2);
            }
        }

        for (int y = 0; y < 3; y++)
        {
            if (tikTakToe.GetCell(0, y) == TikTakToe.FieldStates.Empty && tikTakToe.GetCell(1, y) == mark && tikTakToe.GetCell(2, y) == mark)
            {
                return new Vector2Int(0, y);
            }
            if (tikTakToe.GetCell(0, y) == mark && tikTakToe.GetCell(1, y) == TikTakToe.FieldStates.Empty && tikTakToe.GetCell(2, y) == mark)
            {
                return new Vector2Int(1, y);
            }
            if (tikTakToe.GetCell(0, y) == mark && tikTakToe.GetCell(1, y) == mark && tikTakToe.GetCell(2, y) == TikTakToe.FieldStates.Empty)
            {
                return new Vector2Int(2, y);
            }
        }

        if (tikTakToe.GetCell(0, 0) == TikTakToe.FieldStates.Empty && tikTakToe.GetCell(1, 1) == mark && tikTakToe.GetCell(2, 2) == mark)
        {
            return new Vector2Int(0, 0);
        }
        if (tikTakToe.GetCell(0, 0) == mark && tikTakToe.GetCell(1, 1) == TikTakToe.FieldStates.Empty && tikTakToe.GetCell(2, 2) == mark)
        {
            return new Vector2Int(1, 1);
        }
        if (tikTakToe.GetCell(0, 0) == mark && tikTakToe.GetCell(1, 1) == mark && tikTakToe.GetCell(2, 2) == TikTakToe.FieldStates.Empty)
        {
            return new Vector2Int(2, 2);
        }

        if (tikTakToe.GetCell(2, 0) == TikTakToe.FieldStates.Empty && tikTakToe.GetCell(1, 1) == mark && tikTakToe.GetCell(0, 2) == mark)
        {
            return new Vector2Int(2, 0);
        }
        if (tikTakToe.GetCell(2, 0) == mark && tikTakToe.GetCell(1, 1) == TikTakToe.FieldStates.Empty && tikTakToe.GetCell(0, 2) == mark)
        {
            return new Vector2Int(1, 1);
        }
        if (tikTakToe.GetCell(2, 0) == mark && tikTakToe.GetCell(1, 1) == mark && tikTakToe.GetCell(0, 2) == TikTakToe.FieldStates.Empty)
        {
            return new Vector2Int(0, 2);
        }

        return null;
    }
}
