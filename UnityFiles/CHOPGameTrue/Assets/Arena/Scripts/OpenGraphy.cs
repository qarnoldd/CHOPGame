using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenGraphy : MonoBehaviour
{

    public static bool GameIsPaused = false;
    public GameObject Graphy;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tilde))
        {
            Open();
        }
        else
        {
            Close();
        }
    }
    public void Open()
    {
        Graphy.SetActive(true);
    }
    public void Close()
    {
        Graphy.SetActive(false);
    }


}
