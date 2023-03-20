using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSChecker : MonoBehaviour
{
    public int avgFrameRate;
    public Text display_Text;

    private void Start()
    {
        display_Text = gameObject.GetComponent<Text>();
    }

    public void Update()
    {
        float current = 0;
        current = Time.frameCount / Time.time;
        avgFrameRate = (int)(1f / Time.unscaledDeltaTime);
        display_Text.text = " FPS : " + avgFrameRate.ToString();
    }
}
