using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public static int CharacterTwo = 1;
    public Animator transition;

    public void Character1() // Sets character as character 1 and loads the scene. Helps so that when replaying it knows which scene to open.
    {
        CharacterTwo = 1;
        Debug.Log("Character 1");
        StartCoroutine(LoadScene1());

    }

    public void Character2() // Sets character as character 2 and loads the scene. Helps so that when replaying it knows which scene to open.
    {
        CharacterTwo = 2;
        Debug.Log("Character 2");
        StartCoroutine(LoadScene2());

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
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(2);
    }

    public void SelectChar() 
    {
        FindObjectOfType<AudioManager>().Play("characterselect");
    }
    public void SelectMenu()
    {
        FindObjectOfType<AudioManager>().Play("select");
    }
}
