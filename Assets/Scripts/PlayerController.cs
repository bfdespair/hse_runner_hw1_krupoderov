using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float baseForwardSpeed = 5f;
    [SerializeField] private float laneOffset = 2f;
    [SerializeField] private float laneChangeSpeed = 10f;
    [SerializeField] private float jumpForce = 7f;

    [Header("Jump")]
    [SerializeField] private int maxJumps = 2;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance = 0.1f;

    [Header("Health")]
    [SerializeField] private int maxHealth = 3;

    [Header("Bonuses")]
    [SerializeField] private float speedBonusMultiplier = 2f;
    [SerializeField] private float speedBonusDuration = 3f;
    [SerializeField] private float invulnerabilityDuration = 3f;

    private Rigidbody rb;
    private int currentHealth;
    private int currentLane = 1;
    private int jumpsUsed = 0;

    private float currentSpeed;
    private bool isSpeedBonusActive = false;
    private float speedBonusTimer = 0f;

    private bool isInvulnerable = false;
    private float invulnerabilityTimer = 0f;

    private GameManager gameManager;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        currentHealth = maxHealth;
        currentSpeed = baseForwardSpeed;
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        HandleInput();
        HandleBonusesTimers();
    }

    private void FixedUpdate()
    {
        MoveForward();
        MoveToLane();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            ChangeLane(-1);

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            ChangeLane(1);

        if (Input.GetKeyDown(KeyCode.Space))
            TryJump();
    }

    private void MoveForward()
    {
        Vector3 velocity = rb.velocity;
        velocity.z = currentSpeed;
        rb.velocity = velocity;
    }

    private void MoveToLane()
    {
        float targetX = (currentLane - 1) * laneOffset;
        Vector3 pos = rb.position;
        float newX = Mathf.Lerp(pos.x, targetX, Time.fixedDeltaTime * laneChangeSpeed);
        rb.MovePosition(new Vector3(newX, pos.y, pos.z));
    }

    private void ChangeLane(int dir)
    {
        currentLane = Mathf.Clamp(currentLane + dir, 0, 2);
    }

    private void TryJump()
    {
        if (IsGrounded())
        {
            jumpsUsed = 0;
        }

        if (jumpsUsed < maxJumps)
        {
            Vector3 vel = rb.velocity;
            vel.y = 0;
            rb.velocity = vel;

            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpsUsed++;
        }
    }


    private bool IsGrounded()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        return Physics.Raycast(ray, groundCheckDistance, groundLayer);
    }

    private void HandleBonusesTimers()
    {
        if (isSpeedBonusActive)
        {
            speedBonusTimer -= Time.deltaTime;
            if (speedBonusTimer <= 0)
            {
                isSpeedBonusActive = false;
                currentSpeed = baseForwardSpeed;
            }
        }

        if (isInvulnerable)
        {
            invulnerabilityTimer -= Time.deltaTime;
            if (invulnerabilityTimer <= 0)
                isInvulnerable = false;
        }
    }

    public void TakeDamage(int damage)
    {
        if (isInvulnerable) return;

        currentHealth -= damage;
        currentHealth = Mathf.Max(0, currentHealth);

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        gameManager.OnPlayerDied();
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
    }

    public void ActivateSpeedBonus()
    {
        isSpeedBonusActive = true;
        speedBonusTimer = speedBonusDuration;
        currentSpeed = baseForwardSpeed * speedBonusMultiplier;
    }

    public void ActivateInvulnerability()
    {
        isInvulnerable = true;
        invulnerabilityTimer = invulnerabilityDuration;
    }

    public void IncreaseDifficulty(float multiplier)
    {
        baseForwardSpeed *= multiplier;
        if (!isSpeedBonusActive)
            currentSpeed = baseForwardSpeed;
    }
    public int CurrentHealth => currentHealth;
}
