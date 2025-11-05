using UnityEngine;
using System.Collections;

public class Player1 : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 7f;

    private bool isGrounded = true;
    private bool isKicking = false;

    private Rigidbody2D rb;
    private Animator anim;
    private Vector3 originalScale;

    private string currentAnim = "";

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        originalScale = transform.localScale;
    }

    void Update()
    {
        float move = 0f;

        // Movimento A/D
        if (Input.GetKey(KeyCode.A)) move = -1f;
        if (Input.GetKey(KeyCode.D)) move = 1f;

        // Aplicar movimento e parar quando move == 0
        rb.linearVelocity = new Vector2(move * speed, rb.linearVelocity.y);

        // Virar sprite
        if (move > 0) transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);
        else if (move < 0) transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);

        // Animação de movimento
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
        yield return new WaitForSeconds(0.5f);
        isKicking = false;

        // Voltar para Idle ou Run
        if (Mathf.Abs(rb.linearVelocity.x) > 0.1f)
            PlayAnim("Run");
        else
            PlayAnim("Idle");
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Ignorar colisão com a bola
        if (collision.gameObject.CompareTag("bola"))
            return;

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
