using UnityEngine;

public class ContabilizarGol : MonoBehaviour
{
    public int Player1 = 0;
    public int Player2 = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Gol1"))
        {
            AddPlayer2(1);
        }

        if (collision.CompareTag("Gol2"))
        {
            AddPlayer1(1);
        }
    }

    public void AddPlayer1(int qtd)
    {
        Player1 += qtd;
        if (Player1 < 0)
        {
            Player1 = 0;
        }
        Debug.Log("Player1: " + Player1);
    }

    public void AddPlayer2(int qtd)
    {
        Player2 += qtd;
        if (Player2 < 0)
        {
            Player2 = 0;
        }
        Debug.Log("Player2: " + Player2);
    }
}


