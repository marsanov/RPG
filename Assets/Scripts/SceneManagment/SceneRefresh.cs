using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneRefresh : MonoBehaviour
{
    public void ReloadCurrentScene()
    {
        // Получаем имя текущей сцены
        string sceneName = SceneManager.GetActiveScene().name;
        // Загружаем её саму родимую
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
