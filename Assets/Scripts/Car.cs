using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField] private Transform centerOfMass;
    private Rigidbody rigidBody;

    [SerializeField] private float motorTorque = 100f;
    [SerializeField] private float maxSteer = 20f;

    public float steer { get; set; }
    public float throttle { get; set; }

    private Wheel[] wheels;

    
    [SerializeField] private int currentCheckpoint = 0;


    // Start is called before the first frame update
    void Start()
    {
        wheels = gameObject.GetComponentsInChildren<Wheel>();
        rigidBody = gameObject.GetComponent<Rigidbody>();
        rigidBody.centerOfMass = centerOfMass.localPosition;

    }

    // Update is called once per frame
    void FixedUpdate()
    {


        foreach (Wheel wheel in wheels)
        {
            wheel.steerAngle = steer * maxSteer;
            wheel.torque = throttle * motorTorque;
        }
    }

    public void setNextCheckpoint()
    {
        currentCheckpoint++;
    }

    public void resetCheckpoint()
    {
        currentCheckpoint = 0;
    }

    public int getCurrentCheckpoint()
    {
        return currentCheckpoint;
    }

}
