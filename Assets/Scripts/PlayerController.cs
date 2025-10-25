using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    public float shootInterval = 1f;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public Renderer playerRenderer;
    public Color currentColor = Color.blue;
    public float rotationSpeed = 5f;

    [HideInInspector] public bool isGameOver = false;
    private float shootTimer;

    void Start()
    {
        if (playerRenderer == null)
            playerRenderer = GetComponent<Renderer>();

        playerRenderer.material = new Material(playerRenderer.material);

        ApplyColor(currentColor);
        shootTimer = shootInterval;
    }

    void Update()
    {
        if (isGameOver) return;

        Enemy nearest = GameManager.Instance.GetNearestEnemy(transform.position);
        if (nearest != null)
        {
            Vector3 dir = nearest.transform.position - transform.position;
            dir.y = 0f;
            if (dir.sqrMagnitude > 0.01f)
            {
                Quaternion targetRot = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
            }
        }

        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0f)
        {
            Shoot();
            shootTimer = shootInterval;
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            CycleColor();
        }
    }

    void Shoot()
    {
        if (!bulletPrefab || !firePoint) return;

        GameObject b = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bullet = b.GetComponent<Bullet>();
        if (bullet) bullet.SetColor(currentColor);
    }

    void CycleColor()
    {
        if (currentColor == Color.blue)
            currentColor = Color.green;
        else if (currentColor == Color.green)
            currentColor = Color.white;
        else
            currentColor = Color.blue;

        ApplyColor(currentColor);
    }

    public void ApplyColor(Color c)
    {
        currentColor = c;
        if (playerRenderer)
            playerRenderer.material.color = c;
    }

    public void OnPlayerHitByEnemy()
    {
        if (isGameOver) return;
        isGameOver = true;
        Debug.Log("Game Over!");
        GameManager.Instance.GameOver();
    }
}
