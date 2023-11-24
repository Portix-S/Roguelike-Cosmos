using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Player;

public class MenuScript : MonoBehaviour
{
    public GameObject MenuCanvas;
    public GameObject InstructionCanvas;
    public GameObject CreditsCanvas;

    public PlayerData playerData;

    public void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
       
        MenuCanvas.SetActive(true);
        InstructionCanvas.SetActive(false);
        CreditsCanvas.SetActive(false);
    }
    public void StartGame()
    {
        //SceneManager.LoadScene("DestroyEnemy Scene");
        foreach (PlayerModifiers v in playerData.temp_modifier)
        {

            v.value = 0;
        }


        SceneManager.LoadScene("Portix 3");
    }

    public void Instructions()
    {
        MenuCanvas.SetActive(false);
        InstructionCanvas.SetActive(true);
        CreditsCanvas.SetActive(false);
    }

    public void Credits()
    {
        MenuCanvas.SetActive(false);
        InstructionCanvas.SetActive(false);
        CreditsCanvas.SetActive(true);
    }

    public void BackToMenu()
    {
        MenuCanvas.SetActive(true);
        InstructionCanvas.SetActive(false);
        CreditsCanvas.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}