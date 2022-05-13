using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameScript : MonoBehaviour
{
    public GameObject endCredits;
    public PlayerInventory playerInventory;
    
    void Start()
    {
        endCredits.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.player) && playerInventory.getKey())
        {
            Time.timeScale = 0f;
            endCredits.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    
    public void QuitGame()
    {
        Application.Quit();

    }
    
    
    public void PlayAgain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
