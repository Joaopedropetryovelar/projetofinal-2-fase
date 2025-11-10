using UnityEngine;
using System.Collections;

public class Player1 : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 7f;
    public float chuteForca = 10f; // força do chute

    private bool isGrounded = true;
    private bool isKicking = false;

    private Rigidbody2D rb;
    private Animator anim;
    private Vector3 originalScale;

    private string currentAnim = "";

    private GameObject bola; // referência para a bola

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        originalScale = transform.localScale;

        // procura automaticamente a bola pela tag "bola" (minúsculo)
        bola = GameObject.FindGameObjectWithTag("bola");
    }

    void Update()
    {
        float move = 0f;

        // Movimento A/D
        if (Input.GetKey(KeyCode.A)) move = -1f;
        if (Input.GetKey(KeyCode.D)) move = 1f;

        // Aplicar movimento
        rb.linearVelocity = new Vector2(move * speed, rb.linearVelocity.y);

        // Virar sprite
        if (move > 0) transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);
        else if (move < 0) transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);

        // Animações de movimento
        if (!isKicking && isGrounded)
        {
            if (move != 0)
                PlayAnim("Run");
            else
                PlayAnim("Idle");
        }

        // Pular (W)
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            PlayAnim("Jump");
            isGrounded = false;
        }

        // Chutar (S)
        if (Input.GetKeyDown(KeyCode.S) && isGrounded && !isKicking)
        {
            StartCoroutine(Chute());
        }
    }

    private IEnumerator Chute()
    {
        isKicking = true;
        PlayAnim("Chute");

        // Espera um pouco antes de aplicar o chute
        yield return new WaitForSeconds(0.2f);

        // Chuta a bola se estiver próxima
        if (bola != null)
        {
            float distancia = Vector2.Distance(transform.position, bola.transform.position);

            // Só chuta se estiver perto o suficiente
            if (distancia < 2f)
            {
                Rigidbody2D rbBola = bola.GetComponent<Rigidbody2D>();
                if (rbBola != null)
                {
                    // Direção do chute (pra frente e um pouco pra cima)
                    Vector2 direcao;
                    if (transform.localScale.x < 0)
                        direcao = Vector2.left + Vector2.up * 0.3f;
                    else
                        direcao = Vector2.right + Vector2.up * 0.3f;

                    // Aplica a força do chute
                    rbBola.AddForce(direcao.normalized * chuteForca, ForceMode2D.Impulse);
                }
            }
        }

        yield return new WaitForSeconds(0.3f);
        isKicking = false;

        // Voltar para Idle ou Run
        if (Mathf.Abs(rb.linearVelocity.x) > 0.1f)
            PlayAnim("Run");
        else
            PlayAnim("Idle");
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Detectar chão
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            if (!isKicking)
            {
                if (Mathf.Abs(rb.linearVelocity.x) > 0.1f)
                    PlayAnim("Run");
                else
                    PlayAnim("Idle");
            }
        }
    }

    // Função segura para tocar animação apenas se for diferente da atual
    void PlayAnim(string stateName)
    {
        if (currentAnim == stateName) return;

        int layer = 0;
        if (anim.HasState(layer, Animator.StringToHash(stateName)))
        {
            anim.Play(stateName, layer);
            currentAnim = stateName;
        }
        else
        {
            Debug.LogWarning($"⚠️ Estado '{stateName}' não encontrado no Animator de {gameObject.name}");
        }
    }
}
