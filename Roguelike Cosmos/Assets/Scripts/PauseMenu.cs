using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{

    public GameObject pauseMenu;
    public static bool isPaused;
    public GameObject playerUI;
    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }


    public void PauseGame(bool showPauseMenu = true)
    {
        playerUI.SetActive(false);
        pauseMenu.SetActive(showPauseMenu);
        isPaused = true;
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        playerUI.SetActive(true);
        pauseMenu.SetActive(false);
        isPaused = false;
        Time.timeScale = 1f;
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        isPaused = false;
        SceneManager.LoadScene("MenuPrincipal");
    }
}
