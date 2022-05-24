using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour
{
    [System.Serializable]
    public class TransformArray
    {
        public List<Transform> transforms;
    }

    public TikTakToe tikTakToe;

    [SerializeField]
    private Transform[] cells;
    [SerializeField]
    private TransformArray[] startCells;

    [SerializeField]
    private Transform arm;
    [SerializeField]
    private float speed = 3f;
    [SerializeField]
    private GameObject[] stonePrefab;
    [SerializeField]
    private Transform[] fingers;
    [SerializeField]
    private Transform hand;

    private Vector2Int nextPos;
    public Vector2Int lastPos { get; private set; }

    private Coroutine movingRoutine;
    private Transform stone;
    public bool isMoving => movingRoutine != null;

    private List<GameObject> stones = new List<GameObject>();

    void Awake()
    {
        MoveToPickup();
    }

    public void SetNextPos(Vector2Int pos, bool pickup)
    {
        if (pos.x < -1 || pos.y < 0 || pos.x > 3 || pos.y > 2)
        {
            return;
        }
        nextPos = pos;

        movingRoutine = StartCoroutine(Moving(pickup));
    }

    IEnumerator Moving(bool pickup)
    {
        Vector3 pos;
        if (nextPos.x == -1)
        {
            int rnd = Random.Range(0, startCells[0].transforms.Count);
            stone = startCells[0].transforms[rnd];
            pos = stone.position;
            startCells[0].transforms.RemoveAt(rnd);
        }
        else if (nextPos.x == 3)
        {
            int rnd = Random.Range(0, startCells[1].transforms.Count);
            stone = startCells[1].transforms[rnd];
            pos = stone.position;
            startCells[1].transforms.RemoveAt(rnd);
        }
        else
        {
            pos = cells[nextPos.x + nextPos.y * 3].position;
        }

        while (Vector3.Distance(arm.position, pos) > 0.01f)
        {
            arm.position = Vector3.MoveTowards(arm.position, pos, Time.deltaTime * speed);
            yield return null;
        }
        lastPos = nextPos;

        movingRoutine = pickup ? StartCoroutine(PickUp()) : null;
    }

    public void PlaceStone()
    {
        Transform cell = cells[nextPos.x + nextPos.y * 3];
        tikTakToe.SetCell(nextPos);

        movingRoutine = StartCoroutine(PlaceDown());
    }

    IEnumerator PlaceDown()
    {
        float goal1 = 0.25f;
        while (!Mathf.Approximately(hand.localPosition.y, goal1))
        {
            hand.localPosition = new Vector3(hand.localPosition.x, Mathf.MoveTowards(hand.localPosition.y, goal1, Time.deltaTime * 1f), hand.localPosition.z);

            yield return null;
        }

        float goal2 = 0.75f;
        while (!Mathf.Approximately(fingers[1].localPosition.x, goal2))
        {
            for (int i = 0; i < fingers.Length; i++)
            {
                fingers[i].localPosition = new Vector3(Mathf.MoveTowards(fingers[i].localPosition.x, goal2 * (i == 0 ? -1f : 1f), Time.deltaTime * 1f), fingers[i].localPosition.y, fingers[i].localPosition.z);
            }

            yield return null;
        }

        stone.SetParent(null, true);
        yield return null;

        goal1 = 2f;
        while (!Mathf.Approximately(hand.localPosition.y, goal1))
        {
            hand.localPosition = new Vector3(hand.localPosition.x, Mathf.MoveTowards(hand.localPosition.y, goal1, Time.deltaTime * 1f), hand.localPosition.z);

            yield return null;
        }

        movingRoutine = null;
        stone = null;
        if (!tikTakToe.gameOver)
        {
            MoveToPickup();
        }
    }

    private void MoveToPickup()
    {
        Vector2Int start = tikTakToe.playersTurn ? new Vector2Int(-1, 1) : new Vector2Int(3, 1);
        SetNextPos(start, true);
    }

    IEnumerator PickUp()
    {
        float goal1 = 0.25f;
        while (!Mathf.Approximately(hand.localPosition.y, goal1))
        {
            hand.localPosition = new Vector3(hand.localPosition.x, Mathf.MoveTowards(hand.localPosition.y, goal1, Time.deltaTime * 1f), hand.localPosition.z);

            yield return null;
        }

        float goal2 = 0.5f;
        while (!Mathf.Approximately(fingers[1].localPosition.x, goal2))
        {
            for (int i = 0; i < fingers.Length; i++)
            {
                fingers[i].localPosition = new Vector3(Mathf.MoveTowards(fingers[i].localPosition.x, goal2 * (i == 0 ? -1f : 1f), Time.deltaTime * 1f), fingers[i].localPosition.y, fingers[i].localPosition.z);
            }

            yield return null;
        }

        stone.SetParent(hand, true);
        yield return null;

        goal1 = 2f;
        while (!Mathf.Approximately(hand.localPosition.y, goal1))
        {
            hand.localPosition = new Vector3(hand.localPosition.x, Mathf.MoveTowards(hand.localPosition.y, goal1, Time.deltaTime * 1f), hand.localPosition.z);

            yield return null;
        }

        movingRoutine = null;
    }
}