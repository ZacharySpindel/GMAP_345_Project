using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverGoalManager : MonoBehaviour
{
    public static GameOverGoalManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            AudioManager.Instance.PlaySFX("Loss HP");
        }
    }

    public void GameOver()
    {
        GameObject goal = GameObject.FindGameObjectWithTag("Goal");
        Goal goalScript = goal.GetComponent<Goal>();
        goalScript.gameOverText.SetActive(true);
    }
}

