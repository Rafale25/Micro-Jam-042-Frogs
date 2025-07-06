using UnityEngine;

public class sceneManagerFrog : MonoBehaviour
{
    void Start() {
        var store = new HighscoreStoreWeb();

        // store.GetHighScore();
        // store.SetHighScore(42.69f);

        // WebLocalStorage.HelloWorld();

        // WebLocalStorage.LocalStorageSet("Score", "42.69");

        // TODO: add <> to auto convert to desired format
        // float score = float.Parse(WebLocalStorage.LocalStorageGet("Score"));
        // print($"Score: {score}");

        // WebLocalStorage.LocalStorageRemove("Score");
        // WebLocalStorage.LocalStorageClear();
    }


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
