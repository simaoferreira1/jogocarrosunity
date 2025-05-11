using UnityEngine;
using TMPro;

public class VelocimetroUI : MonoBehaviour
{
    public Rigidbody carroRb;
    public TextMeshProUGUI velocimetroText;

    void Update()
    {
        if (carroRb != null && velocimetroText != null)
        {
            // Calcular a velocidade apenas no plano XZ (ignora Y)
            Vector3 velocidadePlano = new Vector3(carroRb.linearVelocity.x, 0f, carroRb.linearVelocity.z);
            float velocidade = velocidadePlano.magnitude * 3.6f; // m/s → km/h

            // Atualizar o texto do velocímetro
            velocimetroText.text = "Velocidade: " + velocidade.ToString("F1") + " km/h";
        }
    }
}
