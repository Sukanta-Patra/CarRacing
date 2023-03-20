using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class CameraFollow : MonoBehaviour
{
    private Transform player;
    [SerializeField] private Vector3 adjustment;
    [SerializeField] private float speed;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void Update()
    {

        Vector3 newPos = player.position - adjustment;
        transform.position = Vector3.Lerp(transform.position, newPos, speed * Time.deltaTime);
        transform.LookAt(player);
    }
    
    
    void LateUpdate()
    {

    }
}
