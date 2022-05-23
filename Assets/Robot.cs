using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour
{
    public TikTakToe tikTakToe;

    [SerializeField]
    private Transform[] cells;

    [SerializeField]
    private Transform[] startCells;

    [SerializeField]
    private Transform hand;
    [SerializeField]
    private float speed = 3f;
    [SerializeField]
    private GameObject[] stonePrefab;

    [SerializeField]
    private Vector2Int nextPos;
    public Vector2Int lastPos { get; private set; }

    private Coroutine movingRoutine;

    private List<GameObject> stones = new List<GameObject>();

    public void SetNextPos(Vector2Int pos)
    {
        if (movingRoutine != null)
        {
            return;
        }
        if (pos.x < -1 || pos.y < 0 || pos.x > 3 || pos.y > 2)
        {
            return;
        }
        nextPos = pos;

        movingRoutine = StartCoroutine(Moving());
    }

    IEnumerator Moving()
    {
        Vector3 pos;
        if (nextPos.x == -1)
        {
            pos = startCells[0].position;
        }
        else if (nextPos.x == 3)
        {
            pos = startCells[1].position;
        }
        else
        {
            pos = cells[nextPos.x + nextPos.y * 3].position;
        }

        while (Vector3.Distance(hand.position, pos) > 0.01f)
        {
            hand.position = Vector3.MoveTowards(hand.position, pos, Time.deltaTime * speed);
            yield return null;
        }
        lastPos = nextPos;
        movingRoutine = null;
    }

    public bool PlaceStone()
    {
        if (movingRoutine != null)
        {
            return false;
        }

        tikTakToe.SetCell(nextPos);
        Transform cell = cells[nextPos.x + nextPos.y * 3];
        stones.Add(Instantiate(stonePrefab[tikTakToe.playersTurn ? 0 : 1], cell.position, Quaternion.identity, cell));
        return true;
    }

    public void Reset()
    {
        tikTakToe.Reset();
        foreach (var instance in stones)
        {
            Destroy(instance);
        }
        stones.Clear();
    }
}