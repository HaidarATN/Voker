using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int score;

    public PlayerAttribute playerAtt;

    public PlayerMovementController playerMovementController;

    public SpellController playerSpellController;

    public ScoreHistoryManager scoreHistoryManager;

    [HideInInspector]
    public bool isEnd;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        StartCoroutine(GameStateChecker());
        //playerName
    }

    // Update is called once per frame
    IEnumerator GameStateChecker()
    {
        while (true)
        {
            if(playerAtt.healthPoints <= 0)
            {
                OnPlayerDead();
                break;
            }

            yield return new WaitForSeconds(0.01f);
        }
    }

    void OnPlayerDead()
    {
        isEnd = true;
        playerMovementController.enabled = false;
        playerSpellController.enabled = false;
        scoreHistoryManager.AssignNewHistory(score);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        //Show End UI;
    }
}
