using UnityEngine;aw

public class EnemyAI : MonoBehaviour
{
    private Transform player;
    public float speed = 3f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (player == null)
        {
            SearchForPlayer();
        }
        else
        {
            MoveToPlayer();
        }
    }

    void SearchForPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void MoveToPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        rb.MovePosition(rb.position + direction * speed * Time.deltaTime);
    }
}
