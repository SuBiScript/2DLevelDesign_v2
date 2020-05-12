using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{

    public static bool GameIsPaused, gameOver;
    public GameObject pauseMenuUI, selector, list;
    private GameObject player;
    int index = 0;

    void Awake()
    {
        GameIsPaused = false;
        gameOver = false;
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Draw();
    }

    void Update()
    {
        if (GameIsPaused)
        {
            bool up = Input.GetKeyDown("up") || Input.GetKeyDown("w");
            bool down = Input.GetKeyDown("down") || Input.GetKeyDown("s");

            if (up)
            {
                index--;
                FindObjectOfType<AudioManager>().Play("Select");
            }

            if (down)
            {
                index++;
                FindObjectOfType<AudioManager>().Play("Select");
            }

            if (index > list.transform.childCount - 1) index = 0;
            else if (index < 0) index = list.transform.childCount - 1;

            if (up || down) Draw();

            if (Input.GetKeyDown("return")) Action();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            FindObjectOfType<AudioManager>().Play("Enter");
            if (!gameOver)
            {
                if (GameIsPaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
        }
    }
    void Resume()
    {
        GameIsPaused = false;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameManager.gameMaster.resumeGame.Invoke();
        //player.gameObject.SetActive(true);
    }

    void Pause()
    {
        GameIsPaused = true;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameManager.gameMaster.pauseGame.Invoke();
        //player.gameObject.SetActive(false);
    }

    void Draw()
    {
        Transform option = list.transform.GetChild(index);
        selector.transform.position = option.position;
    }

    void Action()
    {
        FindObjectOfType<AudioManager>().Play("Enter");
        Transform option = list.transform.GetChild(index);

        if (option.gameObject.name == "Play")
        {
            Resume();
        }
        if (option.gameObject.name == "MainScene")
        {
            SceneManager.LoadScene("MainScene");
            Time.timeScale = 1f;
            FindObjectOfType<AudioManager>().Stop("BossMusic");
            FindObjectOfType<AudioManager>().Stop("MenuMusic");
            FindObjectOfType<AudioManager>().Play("LevelMusic");
        }
        if (option.gameObject.name == "Menu")
        {
            FindObjectOfType<AudioManager>().Stop("LevelMusic");
            FindObjectOfType<AudioManager>().Stop("BossMusic");
            FindObjectOfType<AudioManager>().Play("MenuMusic");
            SceneManager.LoadScene("MainMenu");
        }
        if (option.gameObject.name == "Exit")
        {
            Application.Quit();
        }

        //OPCIÓ DE TANCAR DINS DEL EDITOR
#if UNITY_EDITOR
        if (option.gameObject.name == "Exit")
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
#endif
    }
}