using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipalManager : MonoBehaviour
{
    [SerializeField] private string Jogo;
    [SerializeField] private GameObject painelMenuInicial;


    

    public void Jogar()
    {
        SceneManager.LoadScene(Jogo);
    }

    public void SairJogo()
    {
        Debug.Log("sair do jogo");
        Application.Quit();
    }

     public void Pause()
    {
        
    }
}
