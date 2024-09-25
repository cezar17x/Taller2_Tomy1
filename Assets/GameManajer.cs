using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager1 : MonoBehaviour
{
    // Cargar el menú principal
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0); // Asegúrate de que esta escena esté en tus Build Settings
        Time.timeScale = 1;
    }


    // Cargar la escena de opciones
    public void LoadOptions()
    {
        SceneManager.LoadScene(""); // Asegúrate de que esta escena esté en tus Build Settings
    }

    // Cargar el nivel 1
    public void LoadLevel1()
    {
        SceneManager.LoadScene(1); // Asegúrate de que esta escena esté en tus Build Settings
    }
    public void GameOver()
    {
        SceneManager.LoadScene(2);
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }
    // Salir del juego
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Detener el juego en el Editor
#else
            Application.Quit(); // Salir del juego en un build
#endif
    }
}