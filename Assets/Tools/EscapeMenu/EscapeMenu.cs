using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeMenu : MonoBehaviour
{
    [SerializeField] private GameObject _escapeMenu;
    [SerializeField] private bool _stopTime;

    private void Awake()
    {
        _escapeMenu.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_escapeMenu.activeInHierarchy == false)
            {
                _escapeMenu.SetActive(true);
                Time.timeScale = _stopTime ? 0 : 1;
            }
            else
            {
                Resume();
            }
        }
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void Resume()
    {
        _escapeMenu.SetActive(false);
        Time.timeScale = 1;
    }
}
