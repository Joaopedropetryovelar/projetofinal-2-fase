using UnityEngine;
using System.Collections;

public class Player2 : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 7f;
    public float chuteForca = 10f;

    private bool isGrounded = true;
    private bool isKicking = false;

    private Rigidbody2D rb;
    private Animator anim;
    private Vector3 originalScale;
    private string currentAnim = "";

    private GameObject bola;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        originalScale = transform.localScale;

        bola = GameObject.FindGameObjectWithTag("bola");
    }

    void Update()
    {
        float move = 0f;

        // Movimento ← →
        if (Input.GetKey(KeyCode.LeftArrow)) move = -1f;
        if (Input.GetKey(KeyCode.RightArrow)) move = 1f;

        // Aplicar movimento
        rb.linearVelocity = new Vector2(move * speed, rb.linearVelocity.y);

        // Virar sprite
        if (move > 0) transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);
        else if (move < 0) transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);

        // Animação de movimento
        if (!isKicking && isGrounded)
        {
            if (move != 0)
                PlayAnim("Correr");
            else
                PlayAnim("Parado");
        }

        // Pular (↑)
        if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            PlayAnim("Pulo");
            isGrounded = false;
        }

        // Chutar (↓)
        if (Input.GetKeyDown(KeyCode.DownArrow) && isGrounded && !isKicking)
        {
            StartCoroutine(Chute());
        }
    }

    private IEnumerator Chute()
    {
        isKicking = true;
        PlayAnim("Kick");

        yield return new WaitForSeconds(0.2f);

        if (bola != null)
        {
            float distancia = Vector2.Distance(transform.position, bola.transform.position);

            if (distancia < 2f)
            {
                Rigidbody2D rbBola = bola.GetComponent<Rigidbody2D>();
                if (rbBola != null)
                {
                    Vector2 direcao;
                    if (transform.localScale.x < 0)
                        direcao = Vector2.left + Vector2.up * 0.3f;
                    else
                        direcao = Vector2.right + Vector2.up * 0.3f;

                    rbBola.AddForce(direcao.normalized * chuteForca, ForceMode2D.Impulse);
                }
            }
        }

        yield return new WaitForSeconds(0.3f);
        isKicking = false;

        if (Mathf.Abs(rb.linearVelocity.x) > 0.1f)
            PlayAnim("Correr");
        else
            PlayAnim("Parado");
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            if (!isKicking)
            {
                if (Mathf.Abs(rb.linearVelocity.x) > 0.1f)
                    PlayAnim("Correr");
                else
                    PlayAnim("Parado");
            }
        }
    }

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
