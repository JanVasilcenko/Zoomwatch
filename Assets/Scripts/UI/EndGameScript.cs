using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameScript : MonoBehaviour
{
    public GameObject endCredits;

    public TextMeshProUGUI quitBtn;

    public TextMeshProUGUI backToMenuBtn;
    
    // Start is called before the first frame update
    void Start()
    {
        endCredits.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.player))
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
