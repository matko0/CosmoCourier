using UnityEngine;
using UnityEngine.UI;

public class OxygenGenerator : MonoBehaviour
{
    public Slider oxygenSlider; // Reference to the generator's UI slider
    public float maxOxygen = 1f; // Maximum oxygen capacity
    public float depletionRate; // Rate at which oxygen depletes per second

    void Start()
    {
        if (oxygenSlider != null)
        {
            oxygenSlider.maxValue = maxOxygen;
            oxygenSlider.value = maxOxygen; // Start with a full generator for testing
        }
    }

    void Update()
    {
        DepleteOxygen();
    }

    public void AddOxygen(float amount)
    {
        if (oxygenSlider != null)
        {
            oxygenSlider.value = Mathf.Clamp(oxygenSlider.value + amount, 0f, maxOxygen);
        }
    }

    private void DepleteOxygen()
    {
        if (oxygenSlider != null)
        {
            oxygenSlider.value = Mathf.Clamp(oxygenSlider.value - depletionRate * Time.deltaTime, 0f, maxOxygen);
        }
    }
}
