using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Player : MonoBehaviour
{
    public enum ControlType 
    { 
    Manual,
    AI
    };

    [SerializeField] private ControlType controlType = ControlType.Manual;

    public float bestLapTime { get; private set; } = Mathf.Infinity;
    public float lastLapTime { get; private set; } = 0f;
    public float currentLapTime { get; private set; } = 0f;
    public int currentLap { get; private set; } = 0;

    private float lapTimer;
    private int lastCheckpoint = 0;

    private Transform checkpointRoot;
    private int checkpointsCount;
    private int checkpointLayer;
    private Car car;

    /*[SerializeField] private Transform followTarget;
    private Vector3 initialTargetPosition;
    private Quaternion initialTargetRotation;*/
    // Start is called before the first frame update
    void Awake()
    {
        checkpointRoot = GameObject.Find("Checkpoints").GetComponent<Transform>();
        checkpointsCount = checkpointRoot.childCount;
        checkpointLayer = LayerMask.NameToLayer("Checkpoint");
        car = gameObject.GetComponent<Car>();
        /*followTarget = GameObject.FindGameObjectWithTag("FollowTarget").GetComponent<Transform>();
        initialTargetPosition = followTarget.localPosition;
        initialTargetRotation = followTarget.localRotation;*/
    }

    // Update is called once per frame
    void Update()
    {
        currentLapTime = lapTimer > 0 ? Time.time - lapTimer : 0;
        GameManager.instance.setCurrentLapTimeText(currentLapTime);
        switch (controlType)
        {
            case ControlType.Manual:
                car.steer = InputManager.instance.steerInput;
                car.throttle = InputManager.instance.throttleInput;
                /*if (car.throttle <= -0.3f)
                {
                    reverseCamera();
                }
                else
                {
                    forwardCamera();
                }*/
                break;

            case ControlType.AI:
                // AI logic
                break;

        }
    }

   /* private void reverseCamera()
    {
        Vector3 revPos = new Vector3(initialTargetPosition.x, initialTargetPosition.y, -initialTargetPosition.z);
        followTarget.localPosition = revPos;
        followTarget.localRotation = Quaternion.Euler(initialTargetRotation.eulerAngles.x, -180f, initialTargetRotation.eulerAngles.z);
    }

    private void forwardCamera()
    {
        followTarget.localPosition = initialTargetPosition;
        followTarget.localRotation = Quaternion.Euler(initialTargetRotation.eulerAngles.x, 0f, initialTargetRotation.eulerAngles.z);
    }*/

    private void startLap()
    {
        currentLap++;
        GameManager.instance.setLapText(currentLap);
        car.resetCheckpoint();
        lapTimer = Time.time;
    }

    private void endLap()
    {
        lastLapTime = Time.time - lapTimer;
        //Debug.Log("LastLapTime:" + lastLapTime);
        bestLapTime = Mathf.Min(lastLapTime, bestLapTime);
        //Debug.Log("BestLapTime:" + bestLapTime);
        
        GameManager.instance.setBestLapTimeText(bestLapTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Checkpoint"))
        {
            if (car.getCurrentCheckpoint() + 1 == other.GetComponent<Checkpoint>().checkpointIndex)
            {
                //normal checkpoint crossed
                car.setNextCheckpoint();
                //Debug.Log("Passed");
            }
            else if (other.GetComponent<Checkpoint>().checkpointIndex == 0)
            {
                if (car.getCurrentCheckpoint() == other.GetComponent<Checkpoint>().lastCheckpointIndex && other.GetComponent<Checkpoint>().checkpointIndex == 0)
                {
                    //lap completed
                    endLap();
                    //start new lap 
                    startLap();
                    //Debug.Log("Lap Complete!");
                }
                else if (currentLap == 0)
                {
                    //starting first lap
                    startLap();
                }

            }

        }
    }

}
