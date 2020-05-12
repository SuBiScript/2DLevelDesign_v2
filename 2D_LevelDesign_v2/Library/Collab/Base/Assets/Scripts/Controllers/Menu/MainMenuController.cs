using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {

    //[HideInInspector] public MenuModel menu;
    public GameObject selector, list;
    int index = 0;

    void Start ()
    {
        Screen.SetResolution(1024, 1024, true);
        Draw();
        Time.timeScale = 1f;
        //FindObjectOfType<AudioManager>().Play("MainMenuMusic");
    }


    void Update()
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

        if (Input.GetKeyDown("return"))
        {
            //FindObjectOfType<AudioManager>().Play("Enter");
            Action();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainScene");
        }
    }

    void Draw()
    {
        Transform option = list.transform.GetChild(index);
        selector.transform.position = option.position;
    }

    void Action()
    {
        //FindObjectOfType<AudioManager>().Play("enter");
        Transform option = list.transform.GetChild(index);
   
        if(option.gameObject.name == "Exit")
        {
            Application.Quit();
        }
        else
        {
            SceneManager.LoadScene(option.gameObject.name);
        }

        /*if (option.gameObject.name == "MainLevel")
        {
            FindObjectOfType<AudioManager>().Stop("MainMenuMusic");
            FindObjectOfType<AudioManager>().Play("MainLevelMusic");
        }*/

//OPCIÓ DE TANCAR DINS DEL EDITOR
#if UNITY_EDITOR
        if (option.gameObject.name == "Exit")
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
#endif

    }
}
