using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Enemy : MonoBehaviour
{
    public Color currentColor = Color.red;
    public float moveSpeed = 2f;
    public Transform player;

    private Renderer rend;
    private bool isAlive = true;

    void Awake()
    {
        rend = GetComponent<Renderer>();
    }

    void Start()
    {
        rend.material.color = currentColor;
        if (!player)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p) player = p.transform;
        }
    }

    void Update()
    {
        if (!isAlive || !player) return;

        Vector3 toPlayer = player.position - transform.position;
        toPlayer.y = 0f;

        transform.position += toPlayer.normalized * moveSpeed * Time.deltaTime;

        if (toPlayer.magnitude < 0.6f)
        {
            PlayerController pc = player.GetComponent<PlayerController>();
            if (pc) pc.OnPlayerHitByEnemy();
        }
    }

    public void SetColor(Color c)
    {
        currentColor = c;
        if (rend) rend.material.color = c;
    }

    public void DestroyEnemy()
    {
        if (!isAlive) return;
        isAlive = false;
        Destroy(gameObject);
    }
}