using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class something : MonoBehaviour
{
    public float moveSpeed;
    public UnityEngine.Transform player;
    // Start is called before the first frame update
    void Start()
    {
        Vector2 dir = (player.position - transform.position).normalized;
        transform.Translate(dir * moveSpeed * Time.deltaTime);
    }
}
