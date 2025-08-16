using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;
    public bool lockCursorOnLoad = true;
    public CursorLockMode cursorLockMode = CursorLockMode.Locked;
    PlayerBehavior player;
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Always fade in when a new scene is loaded
        transition.SetTrigger("StartLevelLoader");
    }

    public IEnumerator LoadLevel(int levelIndex)
    {

        // Lock cursor if enabled
        if (lockCursorOnLoad)
        {
            SetCursorState(cursorLockMode, !lockCursorOnLoad);
        }

        // End LevelLoader
        transition.SetTrigger("EndLevelLoader");

        //Wait
        yield return new WaitForSeconds(transitionTime);

        //Load Scene
        SceneManager.LoadSceneAsync(levelIndex);
    }
    

    public void SetCursorState(CursorLockMode lockMode, bool visible)
    {
        Cursor.lockState = lockMode;
        Cursor.visible = visible;
    }
    
    public void LockCursor()
    {
        SetCursorState(CursorLockMode.Locked, false);
    }
    
    public void UnlockCursor()
    {
        SetCursorState(CursorLockMode.None, true);
    }
}