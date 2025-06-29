using UnityEngine;

public class sceneManagerFrog : MonoBehaviour
{
    public void Load(string sceneName)
    {
        TransitionManager.Instance.TransitionToScene(sceneName, 0.3f);
    }

    public void QuitGame()
    {
        // save any game data here
    #if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }
}
