using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

    public static MenuManager Instance;
    
    [SerializeField] private GameObject backButton;
   

    [SerializeField] private GameObject menuStartPanel;    
    
    [SerializeField] private GameObject presetImageChosenPanel;
    [SerializeField] private GameObject gridStylePanel;
    [SerializeField] private GameObject gridSizePanel;
    
    [SerializeField] private GameObject loadingScreenUI;
    [SerializeField] private Slider progressBar;


    [SerializeField] private TextMeshProUGUI gridStyleText;
    [SerializeField] private List<string> gridStyleList;
    private int gridStylePointer = 0;
    
    [SerializeField] private PatternBoard patternBoard;

    private int currentPresetImageIndex = 0;
    [SerializeField] private RawImage presetImagePreview;
    
    

    private GameObject activePanel;
    
    private Stack<GameObject> stack;
    
    private void Awake() {
        Instance = this;
    }

    

    private void Start() {
        backButton.SetActive(false);
        activePanel = menuStartPanel;
        stack = new Stack<GameObject>();
        Application.targetFrameRate = 60;
    }

    public void NewGame() {
        GameInfoStaticData.isLoading = false;
        ImageStyle();
        backButton.SetActive(true);
    }
    
    public void ImageStyle() {
        PresetImageChosen();
    }
    
   
    public void PresetImageChosen() {
        ChangeScreen(presetImageChosenPanel);
        presetImagePreview.texture = ImageImporter.Instance.presetImages[currentPresetImageIndex];
    }

    public void GridStyle() {
        GameInfoStaticData.isImageStyle = false;
        GameInfoStaticData.userImage = null;
        ChangeScreen(gridSizePanel);
    }

    public void Next() {
        GameInfoStaticData.isImageStyle = true;
        GameInfoStaticData.patternType = "Default";
        ChangeScreen(gridSizePanel);
    }

    public void NextPresetImage() {
        if (currentPresetImageIndex == 5) {
            currentPresetImageIndex = -1;
        }

        currentPresetImageIndex++;
        presetImagePreview.texture = ImageImporter.Instance.presetImages[currentPresetImageIndex];
    }
    
    public void PreviousPresetImage() {
        if (currentPresetImageIndex == 0) {
            currentPresetImageIndex = 6;
        }

        currentPresetImageIndex--;
        presetImagePreview.texture = ImageImporter.Instance.presetImages[currentPresetImageIndex];
    }

    public void ChoosePresetImage() {
        ImageImporter.Instance.SavePresetImage(currentPresetImageIndex);
        Next();
    }

    public void Quit() {
        Application.Quit();
    }

    public void Back() {
        AudioManager.Instance.buttonTapAudioSource.Play();
        activePanel.SetActive(false);
        GameObject poppedPanel = stack.Pop();
        activePanel = poppedPanel;
        activePanel.SetActive(true);
        if (stack.Count == 0) {
            backButton.SetActive(false);
        }
    }

    private void ChangeScreen(GameObject panel) {
        AudioManager.Instance.buttonTapAudioSource.Play();
        stack.Push(activePanel);
        activePanel.SetActive(false);
        activePanel = panel;
        activePanel.SetActive(true);
    }
    
    public void SelectBoardSize(int val) {
        GameInfoStaticData.gridSize = val;
        if (!GameInfoStaticData.isImageStyle) {
            ChangeScreen(gridStylePanel);
            patternBoard.GeneratePattern(GameInfoStaticData.gridSize, GameInfoStaticData.patternType);
            return;
        }
        LoadScene();
    }

    /*public void ChooseStyle() {
        AudioManager.Instance.buttonTapAudioSource.Play();
        ImageImporter.Instance.DeleteSavedImage();
        LoadScene();
    }*/

    public void NextStyle() {
        AudioManager.Instance.buttonTapAudioSource.Play();
        if (gridStylePointer == gridStyleList.Count - 1) {
            gridStylePointer = -1;
        }
        gridStylePointer++;
        string style = gridStyleList[gridStylePointer];
        gridStyleText.text = style;
        GameInfoStaticData.patternType = style;
        patternBoard.GeneratePattern(GameInfoStaticData.gridSize, GameInfoStaticData.patternType);
    }

    public void BackStyle() {
        AudioManager.Instance.buttonTapAudioSource.Play();
        if (gridStylePointer == 0) {
            gridStylePointer = gridStyleList.Count;
        }
        gridStylePointer--;
        string style = gridStyleList[gridStylePointer];
        gridStyleText.text = style;
        GameInfoStaticData.patternType = style;
        patternBoard.GeneratePattern(GameInfoStaticData.gridSize, GameInfoStaticData.patternType);
    }
    
    

    private void LoadScene() {
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync() {
        loadingScreenUI.SetActive(true);

        AsyncOperation operation = SceneManager.LoadSceneAsync(1);
        operation.allowSceneActivation = false;

        float fakeProgress = 0f;

        while (fakeProgress < 1f)
        {
            // Calculate actual target progress (up to 0.9f)
            float target = Mathf.Clamp01(operation.progress / 0.9f);

            // After reaching 0.9, we fake the rest (to 1.0)
            if (fakeProgress < target)
            {
                fakeProgress += Time.deltaTime * 0.5f; // Slow smooth increase
            }
            else if (operation.progress >= 0.9f)
            {
                fakeProgress += Time.deltaTime * 0.2f; // Slower near the end
            }

            fakeProgress = Mathf.Clamp01(fakeProgress);

            if (progressBar != null)
                progressBar.value = fakeProgress;

            yield return null;
        }

        // Optional: wait at 100% for a moment
        yield return new WaitForSeconds(0.5f);

        // Finally activate the scene
        operation.allowSceneActivation = true;
    }

    
}
