using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public static bool GameIsPaused = false;
    public GameObject PauseMenuUI;
    public Animator transition;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) //Pauses game if escape is pressed, and unpauses as well.
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
    public void Resume() //Serves same purpose as unpausing when pressing escape.
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Pause() //Pauses the game.
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }
    public void LoadMenu() //Loads the menu coroutine.
    {
        Time.timeScale = 1f;
        StartCoroutine(LoadSceneMenu());
    }
    public void QuitGame() //Quits the game. How dare you!!!
    {
        Application.Quit();
    }
    IEnumerator LoadScene1() //Plays transition then loads the scene.
    {
        transition.SetTrigger("end");
        Debug.Log("Character 1 Loading");
        yield return new WaitForSeconds(1.5f);
        Debug.Log("Character 1 Loading Scene");
        SceneManager.LoadScene(1);
    }
    IEnumerator LoadScene2() //Plays transition then loads the scene.
    {
        transition.SetTrigger("end");
        Debug.Log("Character 2 Loading");
        yield return new WaitForSeconds(1.5f);
        Debug.Log("Character 1 Loading Scene");
        SceneManager.LoadScene(2);
    }
    IEnumerator LoadSceneMenu() //Plays transition then loads the scene.
    {
        transition.SetTrigger("end");
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(0);
    }
    public void SelectMenu() //Select sound effect.
    {
        FindObjectOfType<AudioManager>().Play("select");
    }
}
