
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movimento")]
    public float velocidade = 5f;

    [Header("Sprites da animação")]
    public Sprite[] spritesAndando;

    [Header("Velocidade da animação")]
    public float velocidadeAnimacao = 0.15f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private Vector2 movimento;
    private int indiceSprite = 0;
    private float contadorAnimacao = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        if (rb == null)
            Debug.LogError("Falta Rigidbody2D!");

        if (sr == null)
            Debug.LogError("Falta SpriteRenderer!");

        if (spritesAndando.Length == 0)
            Debug.LogWarning("Adicione os sprites da animação!");
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        movimento = Vector2.zero;

        if (Keyboard.current.aKey.isPressed ||
            Keyboard.current.leftArrowKey.isPressed)
            movimento.x = -1;

        if (Keyboard.current.dKey.isPressed ||
            Keyboard.current.rightArrowKey.isPressed)
            movimento.x = 1;

        if (Keyboard.current.wKey.isPressed ||
            Keyboard.current.upArrowKey.isPressed)
            movimento.y = 1;

        if (Keyboard.current.sKey.isPressed ||
            Keyboard.current.downArrowKey.isPressed)
            movimento.y = -1;

        movimento = movimento.normalized;

        Animacao();
    }

    void FixedUpdate()
    {
        if (rb != null)
        {
            rb.MovePosition(
                rb.position + movimento * velocidade * Time.fixedDeltaTime
            );
        }
    }

    void Animacao()
    {
        if (sr == null || spritesAndando == null ||
            spritesAndando.Length == 0)
            return;

        if (movimento == Vector2.zero)
        {
            indiceSprite = 0;
            contadorAnimacao = 0;
            sr.sprite = spritesAndando[0];
            return;
        }

        contadorAnimacao += Time.deltaTime;

        if (contadorAnimacao >= velocidadeAnimacao)
        {
            contadorAnimacao = 0;

            indiceSprite = (indiceSprite + 1) % spritesAndando.Length;

            sr.sprite = spritesAndando[indiceSprite];
        }
    }
}
