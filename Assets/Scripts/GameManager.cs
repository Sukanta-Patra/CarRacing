using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Texts")]
    [SerializeField] private Text currentLapText;
    [SerializeField] private Text lapText;
    [SerializeField] private Text bestLapTimeText;


    private void Awake()
    {
        instance = this;
        Application.targetFrameRate = 60;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setCurrentLapTimeText(float _currentLapTime)
    {
        currentLapText.text = $"Current Lap:{(int)_currentLapTime / 60}:{(_currentLapTime) % 60:00.000}";
    }

    public void setLapText(int _lap)
    {
        lapText.text = "Lap:" + _lap;
    }

    public void setBestLapTimeText(float _bestLapTime)
    {
        //bestLapTimeText.text = "" + _bestLapTime + ": Best Lap";
        bestLapTimeText.text = $"{(int)_bestLapTime / 60}:{(_bestLapTime) % 60:00.000}: Best Lap";
    }



}
