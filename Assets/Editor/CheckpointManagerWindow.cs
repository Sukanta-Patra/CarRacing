using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CheckpointManagerWindow : EditorWindow
{
    [MenuItem("Tools/Checkpoint Editor")]
    public static void Open()
    {
        GetWindow<CheckpointManagerWindow>();
    }

    public Transform checkpointRoot;
    public string tagString;
    public string layer;
    public string name;

    private void OnGUI()
    {
        SerializedObject obj = new SerializedObject(this);

        EditorGUILayout.PropertyField(obj.FindProperty("checkpointRoot"));

        if (checkpointRoot == null)
        {
            EditorGUILayout.HelpBox("Checkpoint Root must be assigned.", MessageType.Warning);
        }
        else
        {
            EditorGUILayout.BeginVertical("Checkpoint");
            drawButtons();
            EditorGUILayout.EndVertical();
        }

        obj.ApplyModifiedProperties();
    }

    private void drawButtons()
    {
        //drawing textfield
        GUILayout.Label("Tag");
        tagString = EditorGUILayout.TextField(tagString);

        GUILayout.Label("GameObject Name");
        name = EditorGUILayout.TextField(name);

        if (doesTagExist(tagString))
        {
            if (GUILayout.Button("Create Checkpoint"))
            {
                createCheckpoint();
                //Debug.Log(tagString);
            }
            if (Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<Checkpoint>())
            {
                if (GUILayout.Button("Create Checkpoint Before"))
                {
                    createCheckpointBefore();
                }
                if (GUILayout.Button("Create Checkpoint After"))
                {
                    createCheckpointAfter();
                }
                if (GUILayout.Button("Remove Checkpoint"))
                {
                    removeCheckpoint();
                }

            }
        }
        else
        {
            if (tagString != "")
            {
                EditorGUILayout.HelpBox("Please create a new tag as " + tagString, MessageType.Warning);
            }
            else
            {
                EditorGUILayout.HelpBox("Tag cannot be blank", MessageType.Warning);
            }
        }
    }
    public static bool doesTagExist(string aTag)
    {
        try
        {
            GameObject.FindGameObjectsWithTag(aTag);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private void createCheckpoint()
    {

        GameObject checkpointObj = new GameObject(name+ " " + checkpointRoot.childCount, typeof(Checkpoint));
        checkpointObj.transform.SetParent(checkpointRoot, false);
        checkpointObj.tag = tagString;
        Checkpoint checkpoint = checkpointObj.GetComponent<Checkpoint>();
        BoxCollider collider = checkpointObj.AddComponent<BoxCollider>();
        checkpointObj.transform.localScale = new Vector3(3f, 1f, 1f);
        collider.isTrigger = true;
        checkpoint.root = checkpointRoot;
        if (checkpointRoot.childCount > 1)
        {
            int index = checkpoint.transform.GetSiblingIndex();
            checkpoint.checkpointIndex = index;
            checkpoint.lastCheckpointIndex = index;
            checkpointObj.transform.localScale = checkpointRoot.transform.GetChild(index - 1).GetComponent<Transform>().localScale;
            checkpoint.transform.position = checkpointRoot.transform.GetChild(index - 1).transform.position;
            checkpoint.transform.forward = checkpointRoot.transform.GetChild(index - 1).transform.forward;
        }
        for (int i = 0; i < checkpointRoot.childCount; i++)
        {
            checkpointRoot.GetChild(i).GetComponent<Checkpoint>().lastCheckpointIndex = checkpointRoot.childCount - 1;
        }
        Selection.activeGameObject = checkpointObj.gameObject;
    }

    private void createCheckpointBefore()
    {
        GameObject checkpointObj = new GameObject(name + " " + checkpointRoot.childCount, typeof(Checkpoint));
        checkpointObj.transform.SetParent(checkpointRoot, false);
        checkpointObj.tag = tagString;
        BoxCollider collider = checkpointObj.AddComponent<BoxCollider>();
        collider.isTrigger = true;

        Checkpoint selectedCheckpoint = Selection.activeGameObject.GetComponent<Checkpoint>();
        int index = selectedCheckpoint.transform.GetSiblingIndex();
        checkpointObj.transform.localScale = checkpointRoot.transform.GetChild(index).GetComponent<Transform>().localScale;

        Checkpoint newCheckpoint = checkpointObj.GetComponent<Checkpoint>();
        newCheckpoint.root = checkpointRoot;

        checkpointObj.transform.position = selectedCheckpoint.transform.position;
        checkpointObj.transform.forward = selectedCheckpoint.transform.forward;

        if(selectedCheckpoint.checkpointIndex != 0)
        {
            //int index = selectedCheckpoint.checkpointIndex;
            newCheckpoint.checkpointIndex = selectedCheckpoint.checkpointIndex;
            selectedCheckpoint.checkpointIndex += 1;            
        }
        newCheckpoint.transform.SetSiblingIndex(selectedCheckpoint.transform.GetSiblingIndex());
        for (int i = newCheckpoint.transform.GetSiblingIndex(); i < checkpointRoot.childCount; i++)
        {
            checkpointRoot.GetChild(i).GetComponent<Checkpoint>().checkpointIndex = i;
            checkpointRoot.GetChild(i).name = name + " " + i;
        }
        for (int i = 0; i < checkpointRoot.childCount; i++)
        {
            checkpointRoot.GetChild(i).GetComponent<Checkpoint>().lastCheckpointIndex = checkpointRoot.childCount - 1;
        }
        Selection.activeGameObject = newCheckpoint.gameObject;

    }

    private void createCheckpointAfter()
    {
        GameObject checkpointObj = new GameObject(name + " " + checkpointRoot.childCount, typeof(Checkpoint));
        checkpointObj.transform.SetParent(checkpointRoot, false);
        checkpointObj.tag = tagString;
        BoxCollider collider = checkpointObj.AddComponent<BoxCollider>();
        collider.isTrigger = true;

        Checkpoint selectedCheckpoint = Selection.activeGameObject.GetComponent<Checkpoint>();
        int index = selectedCheckpoint.transform.GetSiblingIndex();
        checkpointObj.transform.localScale = checkpointRoot.transform.GetChild(index).GetComponent<Transform>().localScale;

        Checkpoint newCheckpoint = checkpointObj.GetComponent<Checkpoint>();
        newCheckpoint.root = checkpointRoot;

        checkpointObj.transform.position = selectedCheckpoint.transform.position;
        checkpointObj.transform.forward = selectedCheckpoint.transform.forward;

        if (selectedCheckpoint.checkpointIndex != 0)
        {
            //int index = selectedCheckpoint.checkpointIndex;

            newCheckpoint.checkpointIndex = selectedCheckpoint.checkpointIndex + 1;
            
        }
        newCheckpoint.transform.SetSiblingIndex(selectedCheckpoint.transform.GetSiblingIndex() + 1);
        for (int i = newCheckpoint.transform.GetSiblingIndex(); i < checkpointRoot.childCount; i++)
        {
            checkpointRoot.GetChild(i).GetComponent<Checkpoint>().checkpointIndex = i;
            checkpointRoot.GetChild(i).name = name + " " + i;
        }
        for (int i = 0; i < checkpointRoot.childCount; i++)
        {
            checkpointRoot.GetChild(i).GetComponent<Checkpoint>().lastCheckpointIndex = checkpointRoot.childCount - 1;
        }

        Selection.activeGameObject = newCheckpoint.gameObject;
    }

    private void removeCheckpoint()
    {
        Checkpoint selectedCheckpoint = Selection.activeGameObject.GetComponent<Checkpoint>();

        if (selectedCheckpoint.checkpointIndex != checkpointRoot.childCount - 1)
        {
            int nextCheckpointIndex = selectedCheckpoint.transform.GetSiblingIndex() + 1;
            Selection.activeGameObject = checkpointRoot.GetChild(nextCheckpointIndex).gameObject;
            for (int i = nextCheckpointIndex; i < checkpointRoot.childCount; i++)
            {
                checkpointRoot.GetChild(i).GetComponent<Checkpoint>().checkpointIndex = i - 1;
                checkpointRoot.GetChild(i).name = name + " " + (i - 1);
            }

        }
        else
        {
            Selection.activeGameObject = checkpointRoot.GetChild(selectedCheckpoint.transform.GetSiblingIndex() - 1).gameObject;
        }
        DestroyImmediate(selectedCheckpoint.gameObject);
        for (int i = 0; i < checkpointRoot.childCount; i++)
        {
            checkpointRoot.GetChild(i).GetComponent<Checkpoint>().lastCheckpointIndex = checkpointRoot.childCount - 1;
        }
    }

}
