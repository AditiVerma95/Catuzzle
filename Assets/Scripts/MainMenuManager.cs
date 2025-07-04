using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour {
    private Stack<GameObject> stack;

    [SerializeField] private Button backButton;
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject settingPanel;
    [SerializeField] private GameObject howToPlayPanel;
    private GameObject currentPanel;
    public static MainMenuManager Instance;

    private void Awake() {
        Instance = this;
    }
    
    private void Start() {
        stack = new Stack<GameObject>();
        currentPanel = startPanel;
    }
    
    public void Play() {
        SceneManager.LoadScene(1);
    }

    public void Menu() {
        SceneManager.LoadScene(0);
    }
    public void Settings() {
        ChangePanel(settingPanel);
    }

    public void HowToPlay() {
        ChangePanel(howToPlayPanel);
    }

    public void Exit() {
        Application.Quit();
    }

    public void Back() {
        currentPanel.SetActive(false);
        currentPanel = stack.Pop();
        currentPanel.SetActive(true);
    }

    private void ChangePanel(GameObject panel) {
        stack.Push(currentPanel);
        currentPanel.SetActive(false);
        currentPanel = panel;
        currentPanel.SetActive(true);
    }

    
}