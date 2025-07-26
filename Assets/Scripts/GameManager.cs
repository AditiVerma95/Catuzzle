using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public static GameManager Instance;
    
    [SerializeField] private Board board;
    [SerializeField] private PatternBoard patternBoard;
    public TimerAndSteps timerAndSteps;
    [SerializeField] private GameOver gameOver;
    
    private void Awake() {
        Instance = this;
    }

    private void Start() {
        if (!GameInfoStaticData.isLoading) {
            StartNewGame();
        }
    }

    private void StartNewGame() {
        GameInfoStaticData.userImage = ImageImporter.Instance.LoadSavedImage();
        board.InitializeNewGame(GameInfoStaticData.gridSize, GameInfoStaticData.patternType);
        patternBoard.GeneratePattern(GameInfoStaticData.gridSize, GameInfoStaticData.patternType);
        StartCoroutine(ShuffleBoard());
    }

    private IEnumerator ShuffleBoard() {
        yield return StartCoroutine(board.ShuffleBoard());
        timerAndSteps.isTimerStarted = true;
        timerAndSteps.isStepsStarted = true;
    }

    public void MoveTile(Tile tile) {
        StartCoroutine(MoveTileOnBoard(tile));
    }

    private IEnumerator MoveTileOnBoard(Tile tile) {
        yield return StartCoroutine(board.MoveTile(tile));
        if (CompareBoardAndPaternBoard()) {
            gameOver.StartGameOver(timerAndSteps.steps, timerAndSteps.elapsedTime);
        }
    }

    private bool CompareBoardAndPaternBoard() {
        for (int i = 0; i < board.tiles.Length; i++) {
            for (int j = 0; j < board.tiles[i].Length; j++) {
                if (board.tiles[i][j].value != patternBoard.pattern[i][j].value) {
                    return false;
                }
            }
        }
        return true;
    }
    
}
