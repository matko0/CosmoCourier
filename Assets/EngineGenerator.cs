using UnityEngine;
using UnityEngine.UI;

public class EngineController : MonoBehaviour
{
    public Slider fuelSlider; // Reference to the engine's UI slider
    public float maxFuel = 100f; // Maximum fuel capacity
    public float depletionRate = 1f; // Rate at which fuel depletes per second

    void Start()
    {
        if (fuelSlider != null)
        {
            fuelSlider.maxValue = maxFuel;
            fuelSlider.value = maxFuel; // Start with full fuel for testing
        }
    }

    void Update()
    {
        DepleteFuel();
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

            if (fuelSlider.value <= 0f)
            {
                // Implement engine shutdown logic here
                Debug.Log("Engine has run out of fuel!");
                // Example: Disable engine or trigger game over condition
            }
        }
    }
}
