using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public delegate void OnLevelEvent(Level level);

    public enum GameState
    {
        Menu,
        Game
    }

    public static OnLevelEvent _onLevelStart = (Level l) => { };
    public static OnLevelEvent _onLevelEnd = (Level l) => { };

    private static GameManager _instance;

    public Professor _professorPrefab;

    public CameraUISelectionManager _currentCameraUISelectionManager;
    public ControlStationManager _currentControlStationManager;
    public GameState _currentGameState;
    public CCTVCameraController _currentCCTVCamera;

    public int _firstLevel;

    public static Level _currentLevel;
    public static Professor _currentProfessor;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        LoadScene(_firstLevel);
        
        //_currentControlStationManager.SetScreenMode("menu");
    }

    private void OnLevelStart(Level level)
    {
        //_currentControlScreenManager.SetScreenMode("menu");
        _currentControlStationManager.SetScreenMode("game");
        StartCoroutine(FuckingCamerasAreABitch(level));
        _currentCCTVCamera.GoToCamera(level._floors[0]._rootRoom.GetRandomCamera());
        _currentLevel.SpawnProfessor(0);
        _onLevelStart(level);
    }

    private void OnLevelEnd(Level level)
    {
        _currentControlStationManager.TurnOffAllScreens();
        _onLevelEnd(level);
    }

    private IEnumerator FuckingCamerasAreABitch(Level level)
    {
        yield return new WaitForSeconds(2);
        _currentCameraUISelectionManager.SetActiveRoom(level._floors[0]._rootRoom);
    }

    public static Professor GetProfessorPrefab()
    {
        return _instance._professorPrefab;
    }
    
    public static void LoadScene(int sceneIndex)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);

        op.completed += (AsyncOperation operation) => {
            Scene scene = SceneManager.GetSceneByBuildIndex(sceneIndex);
            SceneManager.SetActiveScene(scene);
        };
        
    }

    public static void UnloadCurrentScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        AsyncOperation op = SceneManager.UnloadSceneAsync(scene);
        op.completed += (AsyncOperation operation) => {
            DeregisterLevel(_currentLevel);
            SceneManager.SetActiveScene(SceneManager.GetSceneAt(0));
        };
    }

    public static void RegisterLevel(Level level)
    {
        _currentLevel = level;
        _instance.OnLevelStart(level);
    }

    public static void DeregisterLevel(Level level)
    {
        _currentLevel = null;
        _instance.OnLevelEnd(level);
    }
    
    public static CameraUISelectionManager GetCameraUI()
    {
        return _instance._currentCameraUISelectionManager;
    }

    public static CCTVCameraController GetCCTVCamera()
    {
        return _instance._currentCCTVCamera;
    }

}
