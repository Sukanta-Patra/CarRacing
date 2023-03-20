using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    public string inputSteerAxis = "Horizontal";
    public string inputThottleAxis = "Vertical";

    public float throttleInput { get; private set; }

    public float steerInput { get; private set; }

    public static InputManager instance;

    [Header("Press L in game to toggle on/off")]
    public bool enableController = false;
    private float moveThreshold = 0.1f;
    [SerializeField] private VariableJoystick joystick;
    private bool horizontalAxis = false;
    [SerializeField] private Image axisBackground;
    [SerializeField] private Sprite horizontalAxisImage;
    [SerializeField] private Sprite roundAxisImage;
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!enableController)
        {
            steerInput = Input.GetAxis(inputSteerAxis);
            throttleInput = Input.GetAxis(inputThottleAxis);
        }
        else
        {
            if (!horizontalAxis)
            {
                steerInput = joystick.Horizontal;
                throttleInput = joystick.Vertical;
                /*if (throttleInput < moveThreshold)
                {
                    throttleInput = -1f;
                }
                else if (throttleInput > moveThreshold)
                { 
                throttleInput = 1f;
                }*/
                //Debug.Log(throttleInput);
            }
            else
            {
                steerInput = joystick.Horizontal;
                throttleInput = 1f;
            }
            
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            enableController = !enableController;
        }
    }

    public void toggleHorizontalAxis()
    {
        horizontalAxis = !horizontalAxis;
        if (horizontalAxis)
        {
            axisBackground.sprite = horizontalAxisImage;
            joystick.AxisOptions = AxisOptions.Horizontal;
        }
        else
        {
            axisBackground.sprite = roundAxisImage;
            joystick.AxisOptions = AxisOptions.Both;
        }
    }

}
