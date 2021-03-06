using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TikTakToe : MonoBehaviour
{
    public enum FieldStates : int
    {
        Empty,
        Player,
        AI,
    }

    public bool playersTurn = true;
    public bool gameOver;
    private int turn;

    private FieldStates[,] fields = new FieldStates[3, 3];

    public UnityEvent<int> onWinCondition;
    public UnityEvent<int> onTurnEnd;

    void Start()
    {
        onTurnEnd?.Invoke((int)(playersTurn ? FieldStates.Player : FieldStates.AI));
    }

    public IList<Vector2Int> GetFreeCells()
    {
        List<Vector2Int> freeCells = new List<Vector2Int>();
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                if (fields[x, y] == FieldStates.Empty)
                {
                    freeCells.Add(new Vector2Int(x, y));
                }
            }
        }
        return freeCells;
    }

    public void SetCell(Vector2Int cellPos)
    {
        if (GetCell(cellPos) != FieldStates.Empty)
        {
            return;
        }
        if (gameOver)
        {
            return;
        }

        fields[cellPos.x, cellPos.y] = playersTurn ? FieldStates.Player : FieldStates.AI;

        EndTurn();
    }

    private void EndTurn()
    {
        FieldStates winner = CheckForWinner();
        if (winner != FieldStates.Empty)
        {
            gameOver = true;
            Debug.Log(winner + " is the winner");
            onWinCondition?.Invoke((int)winner);
        }
        turn++;
        if (turn >= 9 && winner == FieldStates.Empty)
        {
            gameOver = true;
            Debug.Log("no winner");
        }
        playersTurn = !playersTurn;
        onTurnEnd?.Invoke((int)(playersTurn ? FieldStates.Player : FieldStates.AI));
    }

    private FieldStates CheckForWinner()
    {
        for (int x = 0; x < 3; x++)
        {
            if (Check3(new Vector2Int(x, 0), new Vector2Int(x, 1), new Vector2Int(x, 2)))
            {
                return GetCell(new Vector2Int(x, 0));
            }
        }
        for (int y = 0; y < 3; y++)
        {
            if (Check3(new Vector2Int(0, y), new Vector2Int(1, y), new Vector2Int(2, y)))
            {
                return GetCell(new Vector2Int(0, y));
            }
        }
        if (Check3(new Vector2Int(0, 0), new Vector2Int(1, 1), new Vector2Int(2, 2)))
        {
            return GetCell(new Vector2Int(1, 1));
        }
        if (Check3(new Vector2Int(2, 0), new Vector2Int(1, 1), new Vector2Int(0, 2)))
        {
            return GetCell(new Vector2Int(1, 1));
        }
        return FieldStates.Empty;
    }

    private bool Check3(Vector2Int cell1Pos, Vector2Int cell2Pos, Vector2Int cell3Pos)
    {
        FieldStates cell1 = GetCell(cell1Pos);
        FieldStates cell2 = GetCell(cell2Pos);
        FieldStates cell3 = GetCell(cell3Pos);

        return cell1 != FieldStates.Empty && cell1 == cell2 && cell2 == cell3;
    }

    public FieldStates GetCell(int x, int y)
    {
        return GetCell(new Vector2Int(x, y));
    }

    public FieldStates GetCell(Vector2Int cellPos)
    {
        if (cellPos.x < 0 || cellPos.y < 0 || cellPos.x > 2 || cellPos.y > 2)
        {
            throw new System.ArgumentOutOfRangeException("Index can only be between 0 and 2");
        }
        return fields[cellPos.x, cellPos.y];
    }

    public void Reset()
    {
        turn = 1;
        gameOver = false;
        playersTurn = true;
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                fields[x, y] = FieldStates.Empty;
            }
        }
    }
}
