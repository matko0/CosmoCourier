using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerInteraction : MonoBehaviour
{
    public float interactDistance = 2f; // Default distance to interact with objects
    public float holeInteractDistance = 3f; // Distance to interact with holes
    public float tankInteractDistance = 2f; // Distance to interact with oxygen tanks
    public float generatorInteractDistance = 2f; // Distance to interact with oxygen generators
    public float bulletBoxInteractDistance = 2f; // Distance to interact with bullet box
    public float cannonInteractDistance = 2f; // Distance to interact with cannon
    public float fuelTankInteractDistance = 2f; // Distance to interact with fuel tanks or containers
    public float electricityInteractDistance = 2f; // Distance to interact with electricity objects
    public Slider progressBar; // Reference to the player's action progress bar
    public float interactionTime = 2f; // Time to hold E for interaction
    public Transform headPosition; // Reference to the player's head position
    private Coroutine currentInteraction;
    private bool holdingTank; // Flag to indicate if the player is holding an oxygen tank
    private bool holdingBullet; // Flag to indicate if the player is holding a bullet
    private bool holdingFuel; // Flag to indicate if the player is holding fuel
    private bool nearEngine; // Flag to indicate if the player is near an engine

    void Start()
    {
        if (progressBar != null)
        {
            progressBar.gameObject.SetActive(false); // Initially hide the progress bar
        }
    }

    void Update()
    {
        if (headPosition != null && progressBar != null)
        {
            progressBar.transform.position = headPosition.position;
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
                else if (collider.CompareTag("Engine") && holdingFuel && nearEngine)
                {
                    currentInteraction = StartCoroutine(InsertFuelIntoEngine(collider));
                    break;
                }
            }
        }

        // Check for interaction with electricity
        if (Input.GetKeyDown(KeyCode.E))
        {
            Collider2D[] electricityColliders = Physics2D.OverlapCircleAll(transform.position, electricityInteractDistance);
            foreach (Collider2D collider in electricityColliders)
            {
                if (collider.CompareTag("Electricity"))
                {
                    currentInteraction = StartCoroutine(RepairElectricity(collider));
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

        // Deactivate the hole object
        collider.gameObject.SetActive(false);
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

    IEnumerator InsertFuelIntoEngine(Collider2D collider)
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

        // Perform logic to interact with the engine (example: activate fuel insertion)
        EngineController engine = collider.GetComponent<EngineController>();
        if (engine != null)
        {
            engine.AddFuel(0.2f); // Implement InsertFuel method in EngineController
        }
    }

    IEnumerator RepairElectricity(Collider2D collider)
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

        // Deactivate the electricity object
        collider.gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Engine"))
        {
            nearEngine = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Engine"))
        {
            nearEngine = false;
        }
    }
}
