using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;

    public float transitionTime = 1f;

    /// <summary>
    /// Lock cursor on load
    /// </summary>
    /// <param name="levelIndex"></param>
    public bool lockCursorOnLoad = true;

    /// <summary>
    /// Cursor Lock mode to apply
    /// </summary>
    /// <param name="levelIndex"></param>
    public CursorLockMode cursorLockMode = CursorLockMode.Locked;

    public IEnumerator LoadLevel(int levelIndex)
    {
        // Lock cursor if enabled
        if (lockCursorOnLoad)
        {
            SetCursorState(cursorLockMode, !lockCursorOnLoad);
        }
        // Start LevelLoader
        transition.SetTrigger("StartLevelLoader");

        //Wait
        yield return new WaitForSeconds(transitionTime);

        //Load Scene
        SceneManager.LoadScene(levelIndex);
    }
    
    /// <summary>
    /// Sets cursor lock state and visibility
    /// </summary>
    /// <param name="lockMode">Lock mode to apply</param>
    /// <param name="visible">Whether cursor should be visible</param>
    public void SetCursorState(CursorLockMode lockMode, bool visible)
    {
        Cursor.lockState = lockMode;
        Cursor.visible = visible;
    }
    
    /// <summary>
    /// Lock cursor to center of screen (for FPS games)
    /// </summary>
    public void LockCursor()
    {
        SetCursorState(CursorLockMode.Locked, false);
    }
    
    /// <summary>
    /// Unlock cursor and make it visible (for menus)
    /// </summary>
    public void UnlockCursor()
    {
        SetCursorState(CursorLockMode.None, true);
    }
}
