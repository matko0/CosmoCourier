using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EngineController : MonoBehaviour
{
    public Slider fuelSlider; // Reference to the engine's UI slider
    public float maxFuel = 1f; // Maximum fuel capacity
    public float depletionRate; // Rate at which fuel depletes per second

    void Start()
    {
        if (fuelSlider != null)
        {
            fuelSlider.maxValue = maxFuel;
            fuelSlider.value = maxFuel; // Start with a full engine for testing
        }
    }

    void Update()
    {
        DepleteFuel();
        CheckGameOver();
    }

    public void AddFuel(float amount)
    {
        if (fuelSlider != null)
        {
            fuelSlider.value = Mathf.Clamp(fuelSlider.value + amount, 0f, maxFuel);
        }
    }

    private void DepleteFuel()
    {
        if (fuelSlider != null)
        {
            fuelSlider.value = Mathf.Clamp(fuelSlider.value - depletionRate * Time.deltaTime, 0f, maxFuel);
        }
    }

    private void CheckGameOver()
    {
        if (fuelSlider != null && fuelSlider.value <= 0f)
        {
            SceneManager.LoadScene("gameOver");
        }
    }
}
