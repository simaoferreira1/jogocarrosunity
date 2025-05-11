using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CarMovement : MonoBehaviour
{
    public float acceleration = 5000f;
    public float maxSpeed = 300f;
    public float turnSpeed = 75f;
    public float drag = 0.98f;
    public float driftFactor = 0.4f;
    public float driftTurnMultiplier = 1.8f;

    public Transform frontLeftWheel;
    public Transform frontRightWheel;
    public Transform rearLeftWheel;
    public Transform rearRightWheel;

    public PhysicsMaterial rearWheelMaterial;
    public float normalFriction = 0.6f;
    public float driftFriction = 0.2f;

    public TextMeshProUGUI velocimetroText;
    public TextMeshProUGUI collisionMessageText;
    public GameObject botaoColisao; // Arraste o botão aqui no Inspector

    private Rigidbody rb;
    private bool isDrifting = false;
    private bool podeMover = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -0.75f, 0);

        if (collisionMessageText != null)
            collisionMessageText.gameObject.SetActive(false);

        if (botaoColisao != null)
            botaoColisao.SetActive(false);
    }

    void FixedUpdate()
    {
        if (!podeMover)
            return;

        float moveInput = Input.GetAxis("Vertical");
        float turnInput = Input.GetAxis("Horizontal");
        bool driftInput = Input.GetKey(KeyCode.Space);

        Vector3 forwardForce = transform.forward * moveInput * acceleration * Time.fixedDeltaTime;

        if (rb.linearVelocity.magnitude < maxSpeed || Vector3.Dot(rb.linearVelocity, forwardForce) < 0)
        {
            rb.AddForce(forwardForce, ForceMode.Acceleration);
        }

        isDrifting = driftInput;

        float rotationSpeed = isDrifting ? turnSpeed * driftTurnMultiplier : turnSpeed;
        float rotation = turnInput * rotationSpeed * Time.fixedDeltaTime;

        if (Mathf.Abs(moveInput) > 0.1f || rb.linearVelocity.magnitude > 0.5f)
        {
            Quaternion turnOffset = Quaternion.Euler(0, rotation, 0);
            rb.MoveRotation(rb.rotation * turnOffset);
        }

        rb.linearVelocity *= drag;

        ApplyDrift(driftInput);

        float wheelRotation = rb.linearVelocity.magnitude * 360f * Time.fixedDeltaTime;
        RotateWheels(wheelRotation);

        AtualizarVelocimetro();
    }

    private void AtualizarVelocimetro()
    {
        if (velocimetroText != null)
        {
            float velocidadeKmh = rb.linearVelocity.magnitude * 3.6f;
            velocimetroText.text = Mathf.RoundToInt(velocidadeKmh) + " km/h";
        }
    }

    private void ApplyDrift(bool isDrifting)
    {
        if (isDrifting)
        {
            Vector3 forwardVelocity = transform.forward * Vector3.Dot(rb.linearVelocity, transform.forward);
            Vector3 rightVelocity = transform.right * Vector3.Dot(rb.linearVelocity, transform.right);
            rb.linearVelocity = forwardVelocity + rightVelocity * driftFactor;

            AdjustRearFriction(driftFriction);
        }
        else
        {
            AdjustRearFriction(normalFriction);
        }
    }

    private void AdjustRearFriction(float frictionValue)
    {
        if (rearWheelMaterial != null)
        {
            rearWheelMaterial.dynamicFriction = frictionValue;
            rearWheelMaterial.staticFriction = frictionValue;
        }
    }

    private void RotateWheels(float rotationAmount)
    {
        frontLeftWheel.Rotate(Vector3.right, rotationAmount);
        frontRightWheel.Rotate(Vector3.right, rotationAmount);
        rearLeftWheel.Rotate(Vector3.right, rotationAmount);
        rearRightWheel.Rotate(Vector3.right, rotationAmount);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            // Mostra a mensagem e o botão
            if (collisionMessageText != null)
                collisionMessageText.gameObject.SetActive(true);

            if (botaoColisao != null)
                botaoColisao.SetActive(true);

            // Impede o movimento do carro
            podeMover = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}