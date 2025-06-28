using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadSceneAdditivly(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void CloseTab(string sceneName)
    {

    }

    public void Quit()
    {
        Application.Quit();
    }
}
