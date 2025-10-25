using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Bullet : MonoBehaviour
{
    public float speed = 12f;
    public Color color = Color.red;
    public float lifeTime = 5f;

    private Renderer rend;

    void Awake()
    {
        rend = GetComponent<Renderer>();
    }

    void Start()
    {
       
        rend.material.color = color;
        
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    public void SetColor(Color c)
    {
        color = c;
        if (rend) rend.material.color = c;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy")) return;

        Enemy enemy = other.GetComponent<Enemy>();
        if (!enemy) return;

       
        if (ApproximatelySameColor(color, enemy.currentColor))
        {
            enemy.DestroyEnemy();
            Destroy(gameObject);
        }
        else
        {
            
            Destroy(gameObject);
        }
    }

    bool ApproximatelySameColor(Color a, Color b)
    {
        const float tol = 0.05f;
        return Mathf.Abs(a.r - b.r) < tol &&
               Mathf.Abs(a.g - b.g) < tol &&
               Mathf.Abs(a.b - b.b) < tol;
    }
}