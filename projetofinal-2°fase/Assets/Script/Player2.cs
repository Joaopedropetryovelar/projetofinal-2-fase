using UnityEngine;
using System.Collections;

public class Player2 : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 7f;
    private bool isGrounded = true;
    private bool isKicking = false;

    private Rigidbody2D rb;
    private Animator anim;
    private Vector3 originalScale;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        originalScale = transform.localScale;
    }

    void Update()
    {
        float move = 0f;

        // Movimento ← e →
        if (Input.GetKey(KeyCode.LeftArrow)) move = -1f;
        if (Input.GetKey(KeyCode.RightArrow)) move = 1f;

        // Aplicar movimento
        rb.linearVelocity = new Vector2(move * speed, rb.linearVelocity.y);

        // Virar sprite
        if (move > 0) transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);
        else if (move < 0) transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);

        // Se estiver chutando, não trocar animação
        if (!isKicking)
        {
            // Alternar animações de movimento
            if (isGrounded)
            {
                if (move != 0)
                    anim.Play("Correr");   // animação de correr
                else
                    anim.Play("Parado");   // animação parada
            }

            // Pular (↑)
            if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
            {
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                anim.Play("Pulo");         // animação de pulo
                isGrounded = false;
            }

            // Chutar (↓)
            if (Input.GetKeyDown(KeyCode.DownArrow) && isGrounded)
            {
                StartCoroutine(Kick());
            }
        }
    }

    private IEnumerator Kick()
    {
        isKicking = true;
        anim.Play("Kick"); // animação de chute
        yield return new WaitForSeconds(0.5f);
        isKicking = false;

        // Volta para Idle ou Run automaticamente
        if (Mathf.Abs(rb.linearVelocity.x) > 0.1f)
            anim.Play("Correr");
        else
            anim.Play("Parado");
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            if (!isKicking)
            {
                if (Mathf.Abs(rb.linearVelocity.x) > 0.1f)
                    anim.Play("Correr");
                else
                    anim.Play("Parado");
            }
        }
    }
}
