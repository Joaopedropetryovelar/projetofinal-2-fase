using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private Vector3 originalScale;

    [Header("Movimento")]
    public float speed = 5f;
    public float jumpForce = 5f;
    private float move;

    private bool isKicking = false;
    private bool isGround = true; // indica se está no chão

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        originalScale = transform.localScale;

        if (anim == null)
            Debug.LogError(" Nenhum Animator encontrado no Player!");
        if (anim.runtimeAnimatorController == null)
            Debug.LogError(" Nenhum Animator Controller atribuído ao Animator do Player!");

        // Ignora colisão entre Player e Ball (a bola precisa estar no layer "Ball")
        int playerLayer = LayerMask.NameToLayer("Player");
        int ballLayer = LayerMask.NameToLayer("Ball");
        if (playerLayer >= 0 && ballLayer >= 0)
            Physics2D.IgnoreLayerCollision(playerLayer, ballLayer, true);
    }

    void Update()
    {
        if (anim == null || anim.runtimeAnimatorController == null)
            return;

        if (isKicking)
            return;

        move = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(move * speed, rb.linearVelocity.y);

        // Virar o personagem
        if (move > 0)
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        else if (move < 0)
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);

        // Atualiza animação de movimento
        if (!Input.GetKey(KeyCode.Space)) // não muda animação enquanto pula
        {
            if (move != 0 && isGround)
                anim.CrossFade("Run", 0f);
            else if (isGround)
                anim.CrossFade("idle", 0f);
        }

        // Chutar
        if (Input.GetKeyDown(KeyCode.Z))
            StartCoroutine(Chutar());

        // Pular
        Jump();
    }

    private IEnumerator Chutar()
    {
        isKicking = true;
        anim.CrossFade("Chute", 0f);

        yield return new WaitForSeconds(0.4f); // duração do chute

        isKicking = false;

        // Retorna para Run ou Idle
        if (move != 0 && isGround)
            anim.CrossFade("Run", 0f);
        else if (isGround)
            anim.CrossFade("idle", 0f);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            anim.CrossFade("Salto", 0f); // animação de pulo
            isGround = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Detecta chão pelo collider
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = true;
        }
    }
}
