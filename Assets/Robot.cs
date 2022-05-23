using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour
{
    [SerializeField]
    private Transform[] cells;

    [SerializeField]
    private Transform[] startCells;

    private Transform hand;

    private Vector2Int nextPos;
    public Vector2Int lastPos { get; private set; }

    private Coroutine movingRoutine;

    public void SetNextPos(Vector2Int pos)
    {
        if (movingRoutine != null)
        {
            return;
        }
        if (pos.x < -1 || pos.y < 0 || pos.x > 3 || pos.y > 2)
        {

        }
        nextPos = pos;

        movingRoutine = StartCoroutine(Moving());
    }

    private IEnumerator Moving()
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
            hand.position = Vector3.MoveTowards(hand.position, pos, Time.deltaTime);
            yield return null;
        }
        lastPos = nextPos;
        movingRoutine = null;
    }
}
