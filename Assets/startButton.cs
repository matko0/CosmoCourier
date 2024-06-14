using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGames : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("level"); // Replace "GameplaySceneName" with your actual scene name
    }
}
