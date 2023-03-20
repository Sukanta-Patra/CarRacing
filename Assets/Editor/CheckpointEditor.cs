using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad()]
public class CheckpointEditor
{
    [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected | GizmoType.Pickable)]
    public static void OnDrawSceneGizmo(Checkpoint checkpoint, GizmoType gizmoType)
    {
        if ((gizmoType & GizmoType.Selected) != 0)
        {
            Gizmos.color = Color.red;
        }
        else
        {
            Gizmos.color = Color.yellow * 0.5f;
        }

        Gizmos.DrawSphere(checkpoint.transform.position, 0.1f);

        Gizmos.color = Color.blue;
        Transform root = checkpoint.root;
        
        Gizmos.color = Color.white;
        Gizmos.DrawLine(checkpoint.transform.position + (checkpoint.transform.right * checkpoint.transform.localScale.x / 2f), checkpoint.transform.position - (checkpoint.transform.right * checkpoint.transform.localScale.x / 2f));

        if (root.childCount > 1 && checkpoint.checkpointIndex > 0)
        {
            int prevCheckpoint = checkpoint.transform.GetSiblingIndex() - 1;
            Gizmos.DrawLine(checkpoint.transform.position, root.GetChild(prevCheckpoint).transform.position);            
            Gizmos.color = Color.red;

            Gizmos.DrawLine(checkpoint.transform.position + (checkpoint.transform.right * checkpoint.transform.localScale.x / 2f), root.GetChild(prevCheckpoint).transform.position + (root.GetChild(prevCheckpoint).transform.right * root.GetChild(prevCheckpoint).transform.localScale.x / 2f));

            Gizmos.DrawLine(checkpoint.transform.position - (checkpoint.transform.right * checkpoint.transform.localScale.x / 2f), root.GetChild(prevCheckpoint).transform.position - (root.GetChild(prevCheckpoint).transform.right * root.GetChild(prevCheckpoint).transform.localScale.x / 2f));
        }
    }
}
