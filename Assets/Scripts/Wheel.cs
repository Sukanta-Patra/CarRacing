using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    [SerializeField] private bool steer;
    [SerializeField] private bool invertSteer;
    [SerializeField] private bool power;

    public float steerAngle { get; set; }
    public float torque { get; set; }

    private WheelCollider wheelCollider;
    private Transform wheelTransform;

    private Vector3 initialRot;
    // Start is called before the first frame update
    void Start()
    {
        wheelCollider = gameObject.GetComponent<WheelCollider>();
        wheelTransform = gameObject.GetComponentInChildren<MeshRenderer>().GetComponent<Transform>();
        initialRot = wheelTransform.rotation.eulerAngles; 
        //Debug.Log(wheelTransform.gameObject.name + initialRot);
    }

    // Update is called once per frame
    void Update()
    {
        wheelCollider.GetWorldPose(out Vector3 pos, out Quaternion rot);
        wheelTransform.position = pos;
        
        wheelTransform.rotation = rot * Quaternion.Euler(initialRot) * Quaternion.Euler(0f,180f,0f);
    }

    private void FixedUpdate()
    {
        if (steer)
        {
            wheelCollider.steerAngle = steerAngle * (invertSteer ? -1 : 1);
            
        }

        if (power)
        {
            wheelCollider.motorTorque = torque;
        }

    }

}
