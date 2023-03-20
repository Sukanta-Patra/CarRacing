using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FollowTarget : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.forward = player.forward;
        transform.position = player.position - offset;
    }
}
