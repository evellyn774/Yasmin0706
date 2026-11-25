
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Personagem")]
    public Transform personagem;

    [Header("Configurações da câmera")]
    public float suavidade = 5f;
    public Vector3 distancia = new Vector3(0f, 0f, -10f);

    void LateUpdate()
    {
        if (personagem == null)
            return;

        Vector3 posicaoDesejada = personagem.position + distancia;

        transform.position = Vector3.Lerp(
            transform.position,
            posicaoDesejada,
            suavidade * Time.deltaTime
        );
    }
}
