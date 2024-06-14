using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerInteraction : MonoBehaviour
{
    public float electricityInteractDistance = 2f; // Distance to interact with electricity
    public float holeInteractDistance = 3f; // Distance to interact with holes
    public float tankInteractDistance = 2f; // Distance to interact with oxygen tanks
    public float generatorInteractDistance = 2f; // Distance to interact with oxygen generators
    public float fuelTankInteractDistance = 2f; // Distance to interact with fuel tanks or containers
    public float engineInteractDistance = 2f; // Distance to interact with engine
    public Slider progressBar; // Reference to the player's action progress bar
    public float interactionTime = 2f; // Time to hold E for interaction
    public Transform headPosition; // Reference to the player's head position
    public Text timerText; // Reference to the UI Text for timer
    public Text highscoreText; // Reference to the UI Text for highscore
    private Coroutine currentInteraction;
    private bool holdingTank; // Flag to indicate if the player is holding an oxygen tank
    private bool holdingFuel; // Flag to indicate if the player is holding fuel
    private bool gameEnded; // Flag to track if game over condition has been triggered
    private float timer; // Timer to track how long the player has been alive
    private float highscore; // Highscore to track the longest survival time

    void Start()
    {
        if (progressBar != null)
        {
            progressBar.gameObject.SetActive(false); // Initially hide the progress bar
        }

        if (timerText != null)
        {
            timerText.text = "Time Alive: 0s";
        }

        // Load highscore from PlayerPrefs
        highscore = PlayerPrefs.GetFloat("Highscore", 0f);
    }

    void Update()
    {
        if (gameEnded)
        {
            return; // Exit update loop if game over condition is met
        }

        // Update timer
        timer += Time.deltaTime;
        UpdateTimerUI();

        if (headPosition != null && progressBar != null)
        {
            progressBar.transform.position = headPosition.position;
        }

        // Check for interaction with electricity
        if (Input.GetKeyDown(KeyCode.E))
        {
            Collider2D[] electricityColliders = Physics2D.OverlapCircleAll(transform.position, electricityInteractDistance);
            foreach (Collider2D collider in electricityColliders)
            {
                if (collider.CompareTag("Electricity"))
                {
                    currentInteraction = StartCoroutine(FixElectricity(collider));
                    break;
                }
            }
        }

        // Check for interaction with holes
        if (Input.GetKeyDown(KeyCode.E))
        {
            Collider2D[] holeColliders = Physics2D.OverlapCircleAll(transform.position, holeInteractDistance);
            foreach (Collider2D collider in holeColliders)
            {
                if (collider.CompareTag("Hole"))
                {
                    currentInteraction = StartCoroutine(RepairHole(collider));
                    break;
                }
            }
        }

        // Check for interaction with oxygen tanks
        if (Input.GetKeyDown(KeyCode.E))
        {
            Collider2D[] tankColliders = Physics2D.OverlapCircleAll(transform.position, tankInteractDistance);
            foreach (Collider2D collider in tankColliders)
            {
                if (collider.CompareTag("OxygenTank") && !holdingTank)
                {
                    currentInteraction = StartCoroutine(TakeOxygenTank(collider));
                    break;
                }
            }
        }

        // Check for interaction with oxygen generators
        if (Input.GetKeyDown(KeyCode.E))
        {
            Collider2D[] generatorColliders = Physics2D.OverlapCircleAll(transform.position, generatorInteractDistance);
            foreach (Collider2D collider in generatorColliders)
            {
                if (collider.CompareTag("OxygenGenerator") && holdingTank)
                {
                    currentInteraction = StartCoroutine(DeliverOxygenToGenerator(collider));
                    break;
                }
            }
        }

        // Check for interaction with fuel tank or container
        if (Input.GetKeyDown(KeyCode.E))
        {
            Collider2D[] fuelTankColliders = Physics2D.OverlapCircleAll(transform.position, fuelTankInteractDistance);
            foreach (Collider2D collider in fuelTankColliders)
            {
                if (collider.CompareTag("FuelTank") && !holdingFuel)
                {
                    currentInteraction = StartCoroutine(TakeFuel(collider));
                    break;
                }
            }
        }

        // Check for interaction with engines
        if (Input.GetKeyDown(KeyCode.E))
        {
            Collider2D[] engineColliders = Physics2D.OverlapCircleAll(transform.position, engineInteractDistance);
            foreach (Collider2D collider in engineColliders)
            {
                if (collider.CompareTag("Engine") && holdingFuel)
                {
                    currentInteraction = StartCoroutine(DeliverFuelToEngine(collider));
                    break;
                }
            }
        }

        // Stop interaction if E key is released
        if (Input.GetKeyUp(KeyCode.E) && currentInteraction != null)
        {
            StopCoroutine(currentInteraction);
            progressBar.gameObject.SetActive(false);
        }
    }

    IEnumerator FixElectricity(Collider2D collider)
    {
        progressBar.gameObject.SetActive(true);
        float elapsedTime = 0f;
        while (elapsedTime < interactionTime)
        {
            if (!Input.GetKey(KeyCode.E))
            {
                progressBar.gameObject.SetActive(false);
                yield break; // Exit the coroutine if E key is released
            }
            elapsedTime += Time.deltaTime;
            progressBar.value = Mathf.Clamp01(elapsedTime / interactionTime);
            yield return null;
        }

        // After interaction time, disable the electricity GameObject if collider still exists
        progressBar.gameObject.SetActive(false);
        if (collider != null && collider.gameObject != null)
        {
            collider.gameObject.SetActive(false);
        }
    }

    IEnumerator RepairHole(Collider2D collider)
    {
        progressBar.gameObject.SetActive(true);
        float elapsedTime = 0f;
        while (elapsedTime < interactionTime)
        {
            if (!Input.GetKey(KeyCode.E))
            {
                progressBar.gameObject.SetActive(false);
                yield break;
            }
            elapsedTime += Time.deltaTime;
            progressBar.value = Mathf.Clamp01(elapsedTime / interactionTime);
            yield return null;
        }
        progressBar.gameObject.SetActive(false);
        collider.gameObject.SetActive(false); // Disable the hole GameObject
    }

    IEnumerator TakeOxygenTank(Collider2D collider)
    {
        progressBar.gameObject.SetActive(true);
        float elapsedTime = 0f;
        while (elapsedTime < interactionTime)
        {
            if (!Input.GetKey(KeyCode.E))
            {
                progressBar.gameObject.SetActive(false);
                yield break;
            }
            elapsedTime += Time.deltaTime;
            progressBar.value = Mathf.Clamp01(elapsedTime / interactionTime);
            yield return null;
        }
        holdingTank = true;
        progressBar.gameObject.SetActive(false);
    }

    IEnumerator DeliverOxygenToGenerator(Collider2D collider)
    {
        progressBar.gameObject.SetActive(true);
        float elapsedTime = 0f;
        while (elapsedTime < interactionTime)
        {
            if (!Input.GetKey(KeyCode.E))
            {
                progressBar.gameObject.SetActive(false);
                yield break;
            }
            elapsedTime += Time.deltaTime;
            progressBar.value = Mathf.Clamp01(elapsedTime / interactionTime);
            yield return null;
        }
        holdingTank = false;
        progressBar.gameObject.SetActive(false);
        OxygenGenerator generator = collider.GetComponent<OxygenGenerator>();
        if (generator != null)
        {
            generator.AddOxygen(0.4f); // Adjust this value as needed
        }
    }

    IEnumerator TakeFuel(Collider2D collider)
    {
        progressBar.gameObject.SetActive(true);
        float elapsedTime = 0f;
        while (elapsedTime < interactionTime)
        {
            if (!Input.GetKey(KeyCode.E))
            {
                progressBar.gameObject.SetActive(false);
                yield break;
            }
            elapsedTime += Time.deltaTime;
            progressBar.value = Mathf.Clamp01(elapsedTime / interactionTime);
            yield return null;
        }
        holdingFuel = true;
        progressBar.gameObject.SetActive(false);
    }

    IEnumerator DeliverFuelToEngine(Collider2D collider)
    {
        progressBar.gameObject.SetActive(true);
        float elapsedTime = 0f;
        while (elapsedTime < interactionTime)
        {
            if (!Input.GetKey(KeyCode.E))
            {
                progressBar.gameObject.SetActive(false);
                yield break;
            }
            elapsedTime += Time.deltaTime;
            progressBar.value = Mathf.Clamp01(elapsedTime / interactionTime);
            yield return null;
        }
        holdingFuel = false;
        progressBar.gameObject.SetActive(false);
        EngineController engine = collider.GetComponent<EngineController>();
        if (engine != null)
        {
            engine.AddFuel(0.4f); // Adjust this value as needed
        }
    }

    void UpdateTimerUI()
    {
        if (timerText != null)
        {
            timerText.text = "Time Alive: " + Mathf.FloorToInt(timer).ToString() + "s";
        }
    }
}