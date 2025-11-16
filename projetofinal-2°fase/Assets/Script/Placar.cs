using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Placar : MonoBehaviour
{
    [Header("Referências UI")]
    public TextMeshProUGUI placarTexto;

    [Header("Nomes dos jogadores")]
    public string nomeTime1 = "João";
    public string nomeTime2 = "Arthur";

    [Header("Referências dos jogadores e bola")]
    public Transform player1;
    public Transform player2;
    public Transform bola;

    [Header("Posições iniciais")]
    private Vector3 posInicialPlayer1;
    private Vector3 posInicialPlayer2;
    private Vector3 posInicialBola;

    private int golsTime1 = 0;
    private int golsTime2 = 0;

    [Header("Regras do jogo")]
    public int maxGols = 3; 

    private bool jogoFinalizado = false;

    [Header("Áudio")]
    public AudioSource somGol;  // 🔊 SOM DO GOL

    void Start()
    {
        posInicialPlayer1 = player1.position;
        posInicialPlayer2 = player2.position;
        posInicialBola = bola.position;

        AtualizarPlacar();
    }

    public void RegistrarGol(string tagDoGol)
    {
        if (jogoFinalizado) return;

        if (tagDoGol == "Gol1")
        {
            golsTime2++;
        }
        else if (tagDoGol == "Gol2")
        {
            golsTime1++;
        }

        AtualizarPlacar();

        // 🔊 TOCA O SOM DO GOL
        if (somGol != null)
            somGol.Play();

        VerificarFimDeJogo();

        if (!jogoFinalizado)
            ResetarPosicoes();
    }

    void AtualizarPlacar()
    {
        placarTexto.text = nomeTime2 + " " + golsTime2 + "  x  " + golsTime1 + " " + nomeTime1;
    }

    void ResetarPosicoes()
    {
        player1.position = posInicialPlayer1;
        player2.position = posInicialPlayer2;
        bola.position = posInicialBola;

        Rigidbody2D rb = bola.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }

    void ReiniciarJogo()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void VerificarFimDeJogo()
    {
        if (golsTime1 >= maxGols)
        {
            jogoFinalizado = true;
            placarTexto.text = nomeTime1 + " VENCEU! (" + golsTime1 + " x " + golsTime2 + ")";

            Invoke("ReiniciarJogo", 7f);
        }
        else if (golsTime2 >= maxGols)
        {
            jogoFinalizado = true;
            placarTexto.text = nomeTime2 + " VENCEU! (" + golsTime2 + " x " + golsTime1 + ")";

            Invoke("ReiniciarJogo", 7f);
        }
    }
}
