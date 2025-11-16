using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    [SerializeField] private GameObject painelPause;
    [SerializeField] private GameObject botaoPause;
    [SerializeField] private string nomeCenaMenu = "Menu";
    private bool pausado = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pausado) Continuar();
            else Pausar();
        }
    }

    public void Continuar()
    {
        painelPause.SetActive(false);
        botaoPause.SetActive(true);
        Time.timeScale = 1f;
        pausado = false;
    }

    public void Pausar()
    {
        painelPause.SetActive(true);
        botaoPause.SetActive(false);
        Time.timeScale = 0f;
        pausado = true;
    }

    public void VoltarMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(nomeCenaMenu);
    }
}
