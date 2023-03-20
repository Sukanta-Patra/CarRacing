using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int checkpointIndex;
    //public Vector3 triggerSize = new Vector3(3f, 1f, 1f);
    public Transform root;
    public int lastCheckpointIndex;

    private void OnTriggerEnter(Collider other)
    {
       
    }


}
