using UnityEngine;

public class Bola : MonoBehaviour
{
    public Placar placar; // arraste o GameController com Placar neste campo
    private Rigidbody2D rb;
    private Vector2 posInicial;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        posInicial = transform.position; // centro do campo (posição inicial)
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // detecta se entrou em um trigger com tag Gol1 ou Gol2
        if (other.CompareTag("Gol1") || other.CompareTag("Gol2"))
        {
            if (placar != null)
            {
                placar.RegistrarGol(other.tag);
            }
            else
            {
                Debug.LogError("Bola: referência a Placar não atribuída no Inspector!");
            }

            // reposiciona / "reseta" a bola
            ResetarBola();
        }
    }

    // Reposiciona a bola e zera velocidade
    private void ResetarBola()
    {
        transform.position = posInicial;
        if (rb != null)
            rb.linearVelocity = Vector2.zero;
    }
}
