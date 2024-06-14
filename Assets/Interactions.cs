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
    public Slider progressBar; // Reference to the player's action progress bar
    public float interactionTime = 2f; // Time to hold E for interaction
    public Transform headPosition; // Reference to the player's head position
    private Coroutine currentInteraction;
    private bool holdingTank; // Flag to indicate if the player is holding an oxygen tank
    private bool holdingBullet; // Flag to indicate if the player is holding a bullet

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

        // Check for interaction with bullet box
        if (Input.GetKeyDown(KeyCode.E))
        {
            Collider2D[] bulletBoxColliders = Physics2D.OverlapCircleAll(transform.position, bulletBoxInteractDistance);
            foreach (Collider2D collider in bulletBoxColliders)
            {
                if (collider.CompareTag("BulletBox") && !holdingBullet)
                {
                    currentInteraction = StartCoroutine(TakeBullet(collider));
                    break;
                }
            }
        }

        // Check for interaction with cannon
        if (Input.GetKeyDown(KeyCode.E))
        {
            Collider2D[] cannonColliders = Physics2D.OverlapCircleAll(transform.position, cannonInteractDistance);
            foreach (Collider2D collider in cannonColliders)
            {
                if (collider.CompareTag("Cannon") && holdingBullet)
                {
                    currentInteraction = StartCoroutine(DeliverBulletToCannon(collider));
                    break;
                }
            }
        }

        // Check for interaction with electricity
        if (Input.GetKeyDown(KeyCode.E))
        {
            Collider2D[] electricityColliders = Physics2D.OverlapCircleAll(transform.position, interactDistance);
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

    IEnumerator TakeBullet(Collider2D collider)
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
        holdingBullet = true;
        progressBar.gameObject.SetActive(false);
    }

    IEnumerator DeliverBulletToCannon(Collider2D collider)
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
        holdingBullet = false;
        progressBar.gameObject.SetActive(false);
        // Perform cannon firing logic here
        Debug.Log("Bullet fired!"); // Example placeholder logic
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
        Destroy(collider.gameObject); // Remove or deactivate the electricity GameObject
        // Optionally, add logic here to resolve the electricity problem
    }
}
