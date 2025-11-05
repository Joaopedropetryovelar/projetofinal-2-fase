using UnityEngine;
using TMPro;

public class Placar : MonoBehaviour
{
    [Header("Referências UI")]
    public TextMeshProUGUI placarTexto; // arrastar o texto do Canvas aqui

    [Header("Nomes dos jogadores")]
    public string nomeTime1 = "João";
    public string nomeTime2 = "Arthur";

    private int golsTime1 = 0;
    private int golsTime2 = 0;

    void Start()
    {
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
    }

    void AtualizarPlacar()
    {
        // Invertendo a ordem dos nomes no texto exibido
        placarTexto.text = nomeTime2 + " " + golsTime2 + "  x  " + golsTime1 + " " + nomeTime1;
    }
}
