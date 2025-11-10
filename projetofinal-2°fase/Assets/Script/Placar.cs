using UnityEngine;
using TMPro;

public class Placar : MonoBehaviour
{
    [Header("Referências UI")]
    public TextMeshProUGUI placarTexto; // arrastar o texto do Canvas aqui

    [Header("Nomes dos jogadores")]
    public string nomeTime1 = "João";
    public string nomeTime2 = "Arthur";

    [Header("Referências dos jogadores e bola")]
    public Transform player1;   // arraste o objeto do player 1 aqui
    public Transform player2;   // arraste o objeto do player 2 aqui
    public Transform bola;      // arraste a bola aqui

    [Header("Posições iniciais")]
    private Vector3 posInicialPlayer1;
    private Vector3 posInicialPlayer2;
    private Vector3 posInicialBola;

    private int golsTime1 = 0;
    private int golsTime2 = 0;

    void Start()
    {
        // Salva as posições iniciais
        posInicialPlayer1 = player1.position;
        posInicialPlayer2 = player2.position;
        posInicialBola = bola.position;

        AtualizarPlacar();
    }

    public void RegistrarGol(string tagDoGol)
    {
        if (tagDoGol == "Gol1")
        {
            golsTime2++; // bola entrou no gol do time 1 → ponto pro time 2
        }
        else if (tagDoGol == "Gol2")
        {
            golsTime1++; // bola entrou no gol do time 2 → ponto pro time 1
        }

        AtualizarPlacar();
        ResetarPosicoes();
    }

    void AtualizarPlacar()
    {
        placarTexto.text = nomeTime2 + " " + golsTime2 + "  x  " + golsTime1 + " " + nomeTime1;
    }

    void ResetarPosicoes()
    {
        // Volta jogadores e bola para o início
        player1.position = posInicialPlayer1;
        player2.position = posInicialPlayer2;
        bola.position = posInicialBola;

        // Remove qualquer velocidade que restar na bola (se tiver Rigidbody2D)
        Rigidbody2D rb = bola.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }
}
