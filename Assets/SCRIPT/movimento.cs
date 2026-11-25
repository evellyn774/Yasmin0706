
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Animação por poses para personagem 2D.
/// Mantém as animações Parado e Andando.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class PersonagemAnimador : MonoBehaviour
{
    [System.Serializable]
    public class Animacao
    {
        public string nome;
        public Sprite[] poses;
        public float quadrosPorSegundo = 8f;
        public bool repetir = true;
    }

    [Header("Animações")]
    public Animacao parado = new Animacao
    {
        nome = "Parado",
        quadrosPorSegundo = 4f,
        repetir = true
    };

    public Animacao andando = new Animacao
    {
        nome = "Andando",
        quadrosPorSegundo = 10f,
        repetir = true
    };

    [Header("Movimento")]
    public float velocidade = 5f;

    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private Vector2 direcao;
    private Animacao atual;
    private float tempo;
    private int indice;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        rb.gravityScale = 0f;
        rb.freezeRotation = true;
    }

    void Start()
    {
        Tocar(parado);
    }

    void Update()
    {
        direcao = Vector2.zero;

        if (Keyboard.current != null)
        {
            float horizontal = 0f;
            float vertical = 0f;

            if (Keyboard.current.aKey.isPressed ||
                Keyboard.current.leftArrowKey.isPressed)
                horizontal = -1f;

            if (Keyboard.current.dKey.isPressed ||
                Keyboard.current.rightArrowKey.isPressed)
                horizontal = 1f;

            if (Keyboard.current.sKey.isPressed ||
                Keyboard.current.downArrowKey.isPressed)
                vertical = -1f;

            if (Keyboard.current.wKey.isPressed ||
                Keyboard.current.upArrowKey.isPressed)
                vertical = 1f;

            direcao = new Vector2(horizontal, vertical).normalized;
        }

        if (direcao.sqrMagnitude > 0.01f)
        {
            Tocar(andando);

            if (direcao.x > 0.01f)
                sr.flipX = false;
            else if (direcao.x < -0.01f)
                sr.flipX = true;
        }
        else
        {
            Tocar(parado);
        }

        AtualizarAnimacao();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = direcao * velocidade;
    }

    void Tocar(Animacao nova)
    {
        if (atual == nova)
            return;

        atual = nova;
        tempo = 0f;
        indice = 0;

        if (atual.poses != null && atual.poses.Length > 0)
            sr.sprite = atual.poses[0];
    }

    void AtualizarAnimacao()
    {
        if (atual == null ||
            atual.poses == null ||
            atual.poses.Length == 0)
            return;

        tempo += Time.deltaTime;

        float duracaoQuadro =
            1f / Mathf.Max(0.01f, atual.quadrosPorSegundo);

        while (tempo >= duracaoQuadro)
        {
            tempo -= duracaoQuadro;

            indice++;

            if (indice >= atual.poses.Length)
            {
                if (atual.repetir)
                    indice = 0;
                else
                    indice = atual.poses.Length - 1;
            }

            sr.sprite = atual.poses[indice];
        }
    }
}
