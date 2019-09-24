using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(VirtuellController))]
public class PlayerInput : MonoBehaviour, IProvider
{

    public InputPackage GetPackage()
    {
        InputPackage inputPackage = new InputPackage();
        inputPackage.MoveHorizontal = InputHorizontalAxis();
        inputPackage.MoveVertical = InputVerticalAxis();
        inputPackage.TriggerLeft = InputLeftTriggerAxis();
        inputPackage.TriggerRight = InputRightTriggerAxis();
        inputPackage.CrossHorizontal = InputCrossHorizontal();
        inputPackage.CrossVertical = InputCrossVertical();
        inputPackage.CameraHorizontal = InputCameraHorizontal();
        inputPackage.CameraVertical = InputCameraVertical();

        inputPackage.BumberLeft = InputLeftBumper();
        inputPackage.BumberRight = InputRightBumper();
        inputPackage.InputX = InputX();
        inputPackage.InputA = InputA();
        inputPackage.InputB = InputB();
        inputPackage.InputY = InputY();
        inputPackage.MoveButton = InputMoveButton();
        inputPackage.CameraButton = InputCameraButton();
        inputPackage.StartButton = InputStartButton();
        inputPackage.SelectButton = InputSelectButton();
        return inputPackage;
    }

    private float InputHorizontalAxis()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        return horizontal;
    }

    private float InputVerticalAxis()
    {
        float vertical = Input.GetAxisRaw("Vertical");
        return vertical;
    }

    private float InputLeftTriggerAxis()
    {
        float triggerLeft = Input.GetAxisRaw("TriggerLeft");
        return triggerLeft;
    }

    private float InputRightTriggerAxis()
    {
        float triggerRight = Input.GetAxisRaw("TriggerRight");
        return triggerRight;
    }

    private float InputCrossHorizontal()
    {
        float crossHorizontal = Input.GetAxisRaw("CrossHorizontal");
        return crossHorizontal;
    }

    private float InputCrossVertical()
    {
        float crossVertical = Input.GetAxisRaw("CrossVertical");
        return crossVertical;
    }

    private float InputCameraHorizontal()
    {
        float cameraHorizontal = Input.GetAxisRaw("CameraHorizontal");
        return cameraHorizontal;
    }

    private float InputCameraVertical()
    {
        float cameraVertical = Input.GetAxisRaw("CameraVertical");
        return cameraVertical;
    }

    private bool InputLeftBumper()
    {
        bool inputLeftBumper = false;
        if(Input.GetButton("LeftBumper"))
        {
            inputLeftBumper = true;
        }
        return inputLeftBumper;
    }

    private bool InputRightBumper()
    {
        bool inputRightBumper = false;
        if (Input.GetButton("RightBumper"))
        {
            inputRightBumper = true;
        }
        return inputRightBumper;
    }

    private bool InputX()
    {
        bool inputX = false;
        if(Input.GetButton("X"))
        {
            inputX = true;            
        }
        return inputX;
    }

    private bool InputA()
    {
        bool inputA = false;
        if (Input.GetButton("A"))
        {
            inputA = true;
        }
        return inputA;
    }

    private bool InputB()
    {
        bool inputB = false;
        if (Input.GetButton("B"))
        {
            inputB = true;
        }
        return inputB;
    }

    private bool InputY()
    {
        bool inputY = false;
        if (Input.GetButton("Y"))
        {
            inputY = true;
        }
        return inputY;
    }

    private bool InputStartButton()
    {
        bool startButton = false;
        if(Input.GetButton("Start"))
        {
            startButton = true;
        }
        return startButton;
    }

    private bool InputSelectButton()
    {
        bool selectButton = false;
        if(Input.GetButton("Select"))
        {
            selectButton = true;
        }
        return selectButton;
    }

    private bool InputMoveButton()
    {
        bool moveButton = false;
        if(Input.GetButton("MoveButton"))
        {
            moveButton = true;
        }
        return moveButton;
    }

    private bool InputCameraButton()
    {
        bool cameraButton = false;
        if(Input.GetButton("CameraButton"))
        {
            cameraButton = true;
        }
        return cameraButton;
    }
}