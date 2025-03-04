using System.Collections.Generic;
using UnityEngine;

public class BubbleMatch : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private static List<GameObject> matchedBubbles = new List<GameObject>();

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
     {
        Debug.LogError("SpriteRenderer no encontrado en " + gameObject.name);
     }

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bubble"))
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
               rb.simulated = false; 
                rb.bodyType = RigidbodyType2D.Kinematic; // Se detiene
            }
            transform.parent = collision.transform.parent; // Se une a la cuadrícula

            // Verifica si hay coincidencias y explota burbujas
            CheckForMatches();
        }
    }

    void CheckForMatches()
    {
 if (spriteRenderer == null || spriteRenderer.sprite == null)
    {
        return; // Evita errores si la burbuja no tiene sprite
    }

    matchedBubbles.Clear();
    FindMatchingBubbles(transform.position, spriteRenderer.sprite);

    Debug.Log("🔍 Burbujas conectadas del mismo color: " + matchedBubbles.Count);

    if (matchedBubbles.Count >= 3)
    {
        foreach (GameObject bubble in matchedBubbles)
        {
            Debug.Log("💥 Destruyendo burbuja: " + bubble.name);
            Destroy(bubble);
        }
    }
  }
    void FindMatchingBubbles(Vector2 position, Sprite targetSprite)
{
    Collider2D[] colliders = Physics2D.OverlapCircleAll(position, 0.9f); // Reducimos el radio para evitar errores

    foreach (Collider2D collider in colliders)
    {
        BubbleMatch bubble = collider.GetComponent<BubbleMatch>();

        if (bubble != null && bubble.spriteRenderer.sprite == targetSprite && !matchedBubbles.Contains(bubble.gameObject))
        {
            matchedBubbles.Add(bubble.gameObject);
            Debug.Log("🔗 Encontrada burbuja conectada: " + bubble.name);

            bubble.FindMatchingBubbles(bubble.transform.position, targetSprite);
        }
    }
}

}

