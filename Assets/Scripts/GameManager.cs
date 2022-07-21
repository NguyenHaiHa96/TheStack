using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private UIManager UI_Manager;
    private BlockSpawner blockSpawner;
    private bool isPlaying = false;
    private bool isGameOver = false;

    // Start is called before the first frame update
    void Awake()
    {
        blockSpawner = FindObjectOfType<BlockSpawner>().GetComponent<BlockSpawner>();
        UI_Manager = FindObjectOfType<UIManager>().GetComponent<UIManager>();
        MovingBlock.IsOverTheEdge += MovingBlock_IsOverTheEdge;       
    }

    void OnDestroy()
    {
        MovingBlock.IsOverTheEdge -= MovingBlock_IsOverTheEdge;
    }

    private void MovingBlock_IsOverTheEdge()
    {
        SetIsGameOver(true);
        blockSpawner.SetIsSpawning(false);
        UI_Manager.Unsubscribe();
    }

    private void SetIsGameOver(bool isOver)
    {
        isGameOver = isOver;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlaying)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isPlaying = true;
                UI_Manager.ShowMenuPanel(false);
                blockSpawner.SpawnBlock();
            }
        }
        else
        {
            UI_Manager.ShowScoreText(true);
            if (Input.GetMouseButtonDown(0))
            {
                if (MovingBlock.CurrentBlock != null)
                {
                    MovingBlock.CurrentBlock.Stop();
                    blockSpawner.SpawnBlock();
                    if (isGameOver)
                    {
                        UI_Manager.ShowGameOverPanel(true);
                        UI_Manager.SetHighScore();
                        UI_Manager.ShowHighScoreText(true);
                        Debug.Log(PlayerPrefs.GetInt("HighScore").ToString());
                        if (Input.GetMouseButtonDown(0))
                        {
                            //Restart();
                        }                      
                    }
                }              
            }
        }       
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
