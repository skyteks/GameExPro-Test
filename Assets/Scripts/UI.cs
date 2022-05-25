using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField]
    private Text winnerText;

    private void Awake()
    {
        winnerText.transform.parent.gameObject.SetActive(false);
    }

    public void SetWinner(int value)
    {
        winnerText.transform.parent.gameObject.SetActive(true);
        TikTakToe.FieldStates winner = (TikTakToe.FieldStates)value;
        switch (winner)
        {
            case TikTakToe.FieldStates.Empty:
                winnerText.text = "Draw";
                break;
            case TikTakToe.FieldStates.Player:
            case TikTakToe.FieldStates.AI:
                winnerText.text = string.Concat(winner.ToString(), " wins!");
                break;
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
