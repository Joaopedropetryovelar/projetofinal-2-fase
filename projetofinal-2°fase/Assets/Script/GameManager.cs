using UnityEngine;
using UnityEngine.Video;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int Pontos = 0;
    public int Pontos2 = 0;

    public TextMeshProUGUI textPontos;
    public TextMeshProUGUI textPontos2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    public void addPlayer1(int qtd)
    {
        Pontos += qtd;

        if (Pontos < 0)
        {
            Pontos = 0;
        }

        textPontos.text = "pontos" + Pontos;
        Debug.Log("pontos : " + Pontos);
    }

    public void addPlayer2(int qtd)
    {
        Pontos2 += qtd;

        if (Pontos2 < 0)
        {
            Pontos2 = 0;
        }

        textPontos2.text = "pontos" + Pontos2;
        Debug.Log("pontos : " + Pontos2);
    }

}
