using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour {

    public GameObject selector, list;
    public Image youWinPanel;

    int index = 0;

    void Start ()
    {
        Time.timeScale = 1f;
        Draw();
    }
	
	void Update ()
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
    void Draw()
    {
        Transform option = list.transform.GetChild(index);
        selector.transform.position = option.position;
    }

    public void SetYouWinPanel()
    {
        youWinPanel.gameObject.SetActive(true);
    }

    void Action()
    {
        FindObjectOfType<AudioManager>().Play("Enter");
        Transform option = list.transform.GetChild(index);

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
            FindObjectOfType<AudioManager>().Play("MenuMusic");
            SceneManager.LoadScene("MainMenu");
        }
        if (option.gameObject.name == "Exit")
        {
            Application.Quit();
        }

#if UNITY_EDITOR
        if (option.gameObject.name == "Exit")
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
#endif
    }
}
