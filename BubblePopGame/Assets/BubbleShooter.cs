using System.Collections;
using UnityEngine;

public class BubbleShooter : MonoBehaviour
{
    public GameObject[] bubblePrefabs;
    public Transform shootPoint;
    public float shootSpeed = 10f;

    private GameObject currentBubble;

    void Start()
    {
        LoadNextBubble();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ShootBubble();
        }
    }

    void LoadNextBubble()
    {
          if (currentBubble != null) return;

    int randomIndex = Random.Range(0, bubblePrefabs.Length);
    GameObject newBubble = Instantiate(bubblePrefabs[randomIndex], shootPoint.position, Quaternion.identity);
    currentBubble = newBubble;

    SpriteRenderer sr = newBubble.GetComponent<SpriteRenderer>();

    if (sr == null)
    {
        Debug.LogError("❌ SpriteRenderer NO encontrado en " + newBubble.name);
    }
    else
    {
        if (sr.sprite == null)
        {
            sr.sprite = bubblePrefabs[randomIndex].GetComponent<SpriteRenderer>().sprite; // Asignar manualmente
            Debug.LogWarning("⚠️ Sprite estaba NULL en " + newBubble.name + " → Se asignó correctamente.");
        }
        else
        {
            Debug.Log("✅ Sprite ya estaba asignado en " + newBubble.name);
        }
    }


    }

    void ShootBubble()
    {
        if (currentBubble == null) return;

        Rigidbody2D rb = currentBubble.GetComponent<Rigidbody2D>();
        if (rb == null) rb = currentBubble.AddComponent<Rigidbody2D>();

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 shootDirection = (mousePos - shootPoint.position).normalized;

        rb.bodyType = RigidbodyType2D.Kinematic;

        StartCoroutine(MoveBubble(rb, shootDirection));

        currentBubble = null;
        Invoke("LoadNextBubble", 0.5f);
    }

    IEnumerator MoveBubble(Rigidbody2D rb, Vector2 direction)
    {
         while (rb != null && rb.gameObject != null) // Verifica que la burbuja siga existiendo
    {
        if (!rb.gameObject.activeInHierarchy) yield break; // Si la burbuja es destruida, salir de la corrutina

        rb.MovePosition(rb.position + direction * shootSpeed * Time.deltaTime);
        yield return null;

       }
    }
}
