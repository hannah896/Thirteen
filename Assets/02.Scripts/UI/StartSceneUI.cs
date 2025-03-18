using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneUI : MonoBehaviour
{
    public void StartButton()
    {
        AudioManager.instance.PlaySFX("Button");
        SceneManager.LoadScene("MainScene");
    }

    public void ExitButton()
    {
        AudioManager.instance.PlaySFX("Button");
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
