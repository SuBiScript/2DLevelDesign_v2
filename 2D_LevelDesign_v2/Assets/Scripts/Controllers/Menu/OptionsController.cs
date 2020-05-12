using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsController : MonoBehaviour
{
    public GameObject selector, list;
    int index = 0;

    void Start()
    {
        Screen.SetResolution(1024, 1024, true);
        Draw();
        Time.timeScale = 1f;
    }


    void Update()
    {
        bool up = Input.GetKeyDown("up") || Input.GetKeyDown("w");
        bool down = Input.GetKeyDown("down") || Input.GetKeyDown("s");

        if (up)
        {
            index--;
        }
        if (down)
        {
            index++;
        }

        if (index > list.transform.childCount - 1) index = 0;
        else if (index < 0) index = list.transform.childCount - 1;

        if (up || down) Draw();

        if (Input.GetKeyDown("return"))
        {
            FindObjectOfType<AudioManager>().Play("Enter");
            Action();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            FindObjectOfType<AudioManager>().Play("Enter");
            SceneManager.LoadScene("MainMenu");
        }
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

        SceneManager.LoadScene(option.gameObject.name);

    }
}
