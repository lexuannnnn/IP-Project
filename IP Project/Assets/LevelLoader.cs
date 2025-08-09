using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
     public Animator transition;
    
    public float transitionTime = 1f;
    public IEnumerator LoadLevel(int levelIndex)
    {
        // Start LevelLoader
        transition.SetTrigger("StartLevelLoader");

        //Wait
        yield return new WaitForSeconds(transitionTime);

        //Load Scene
        SceneManager.LoadScene(levelIndex);
    }
}
