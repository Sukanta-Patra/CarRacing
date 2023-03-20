using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraFollowTarget : MonoBehaviour
{
    [SerializeField] private Transform followTarget;
    //[SerializeField] private Transform player;
    [SerializeField] private float speed;

    // Start is called before the first frame update
    void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    
    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, followTarget.position, speed * Time.deltaTime);
        //transform.LookAt(player);
        Quaternion newRot = Quaternion.Euler(followTarget.rotation.eulerAngles.x, followTarget.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRot, speed * Time.deltaTime);
    }
}
