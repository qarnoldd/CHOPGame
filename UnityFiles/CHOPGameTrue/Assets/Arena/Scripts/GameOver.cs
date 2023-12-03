using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject GameOverUI;
    public GameObject gameplayScore;
    public PlayerControl playerControl;
    public Animator transition;


    public void Resume() //resumes the game, but not available in the game over screen.
    {
        gameplayScore.SetActive(true);
        GameOverUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }
    public void Replay() //Reloads level based on which character was first selected in the menu, hence setting the public static variable to 1 or 2 at the start.
    {
        gameplayScore.SetActive(true);
        Time.timeScale = 1f;
        if (MainMenu.CharacterTwo == 1)
        {
            Debug.Log("Character 1");
            StartCoroutine(LoadScene1());
        }
        if (MainMenu.CharacterTwo == 2)
        {
            Debug.Log("Character 2");
            StartCoroutine(LoadScene2());
        }
    }

    public void Pause() //function to pause game and show UI
    {
        gameplayScore.SetActive(false);
        GameOverUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMenu() //Loads menu
    {
        Time.timeScale = 1f;
        StartCoroutine(LoadSceneMenu());
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator LoadScene1() //Coroutine for transition and loading of scene 1
    {
        transition.SetTrigger("end");
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(1);
    }
    IEnumerator LoadScene2() //Coroutine for transition and loading of scene 2
    {
        transition.SetTrigger("end");
        Debug.Log("Character 2 Loading");
        yield return new WaitForSeconds(1.5f);
        Debug.Log("Character 1 Loading Scene");
        SceneManager.LoadScene(2);
    }
    IEnumerator LoadSceneMenu() //Coroutine for transition and loading of menu scene
    {
        transition.SetTrigger("end");
        Debug.Log("Menu Loading");
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(0);
    }
    void SelectMenu()
    {
        FindObjectOfType<AudioManager>().Play("select");
    }
}
