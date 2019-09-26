using System.Collections.Generic;
using UnityEngine;

public class VirtuellControllerProt2 : MonoBehaviour
{
    IProvider inputProvider;
    InputPackage inputPackage;

    //private PlayerControll player;
    private PlayerMovement player;
    [SerializeField] private CameraFollow cam;

    [SerializeField] List<Texture2D> controller;
    
    bool debugOn = false;

    private void Awake()
    {
        //player = GetComponent<PlayerControll>();
        player = GetComponent<PlayerMovement>();
        inputProvider = gameObject.GetComponent<PlayerInput>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F4))
        {
            debugOn = !debugOn;
        }
        player.InputPackage = inputProvider.GetPackage();
        if (cam != null)
        {
            cam.InputPackage = inputProvider.GetPackage();
        }
    }

    private void OnGUI()
    {
        inputPackage = inputProvider.GetPackage();

        if (debugOn)
        {
            GUI.DrawTexture(new Rect(35, 100, 455, 275), controller[0]);
            GUI.DrawTexture(new Rect(115, 150, 70, 70), controller[1]);
            if(inputPackage.MoveHorizontal >= 0.1f || 
               inputPackage.MoveHorizontal <= -0.1f || 
               inputPackage.MoveVertical >= 0.1f || 
               inputPackage.MoveVertical <= -0.1f)
            {
                GUI.DrawTexture(new Rect(130 + (inputPackage.MoveHorizontal * 15), 165 - (inputPackage.MoveVertical * 15), 40, 40),
                                controller[2],
                                ScaleMode.ScaleToFit,
                                true,
                                0,
                                Color.red,
                                0,
                                0);
            }
            else
            {
                GUI.DrawTexture(new Rect(130, 165, 40, 40), controller[2]);
            }
            GUI.DrawTexture(new Rect(280, 215, 70, 70), controller[1]);
            if (inputPackage.CameraHorizontal >= 0.1f ||
                inputPackage.CameraHorizontal <= -0.1f ||
                inputPackage.CameraVertical >= 0.1f ||
                inputPackage.CameraVertical <= -0.1f)
            {
                GUI.DrawTexture(new Rect(295 + (inputPackage.CameraHorizontal * 15), 230 + (inputPackage.CameraVertical * 15), 40, 40), 
                                controller[2],
                                ScaleMode.ScaleToFit,
                                true,
                                0,
                                Color.red,
                                0,
                                0);
            }
            else
            {
                GUI.DrawTexture(new Rect(295, 230, 40, 40), controller[2]);
            }
            if (inputPackage.CrossHorizontal >= 0.1f ||
                inputPackage.CrossHorizontal <= -0.1f ||
                inputPackage.CrossVertical >= 0.1f ||
                inputPackage.CrossVertical <= -0.1f)
            {
                if(inputPackage.CrossHorizontal >= 0.1f)
                {
                    GUI.DrawTexture(new Rect(170, 210, 70, 70), 
                                    controller[6],
                                    ScaleMode.ScaleToFit,
                                    true,
                                    0,
                                    Color.red,
                                    0,
                                    0);
                }
                if (inputPackage.CrossHorizontal <= -0.1f)
                {
                    GUI.DrawTexture(new Rect(170, 210, 70, 70),
                                    controller[5],
                                    ScaleMode.ScaleToFit,
                                    true,
                                    0,
                                    Color.red,
                                    0,
                                    0);
                }
                if (inputPackage.CrossVertical >= 0.1f)
                {
                    GUI.DrawTexture(new Rect(170, 210, 70, 70),
                                    controller[7],
                                    ScaleMode.ScaleToFit,
                                    true,
                                    0,
                                    Color.red,
                                    0,
                                    0);
                }
                if (inputPackage.CrossVertical <= -0.1f)
                {
                    GUI.DrawTexture(new Rect(170, 210, 70, 70),
                                    controller[4],
                                    ScaleMode.ScaleToFit,
                                    true,
                                    0,
                                    Color.red,
                                    0,
                                    0);
                }
            }
            else
            {
                GUI.DrawTexture(new Rect(170, 210, 70, 70), controller[3]);
            }
            if(inputPackage.InputX)
            {
                GUI.DrawTexture(new Rect(335, 170, 30, 30), 
                                controller[8],
                                ScaleMode.ScaleToFit,
                                true,
                                0,
                                Color.red,
                                0,
                                0);
            }
            else
            {
                GUI.DrawTexture(new Rect(335, 170, 30, 30), controller[8]);
            }
            if(inputPackage.InputA)
            {
                GUI.DrawTexture(new Rect(365, 195, 30, 30),
                                controller[8],
                                ScaleMode.ScaleToFit,
                                true,
                                0,
                                Color.red,
                                0,
                                0);
            }
            else
            {
                GUI.DrawTexture(new Rect(365, 195, 30, 30), controller[8]);
            }
            if(inputPackage.InputB)
            {
                GUI.DrawTexture(new Rect(395, 170, 30, 30), 
                                controller[8], 
                                ScaleMode.ScaleToFit,
                                true,
                                0,
                                Color.red,
                                0,
                                0);
            }
            else
            {
                GUI.DrawTexture(new Rect(395, 170, 30, 30), controller[8]);
            }
            if(inputPackage.InputY)
            {
                GUI.DrawTexture(new Rect(365, 140, 30, 30), 
                                controller[8], 
                                ScaleMode.ScaleToFit,
                                true,
                                0,
                                Color.red,
                                0,
                                0);
            }
            else
            {
                GUI.DrawTexture(new Rect(365, 140, 30, 30), controller[8]);
            }
            if(inputPackage.SelectButton)
            {
                GUI.DrawTexture(new Rect(220, 175, 20, 20), 
                                controller[9], 
                                ScaleMode.ScaleToFit,
                                true,
                                0,
                                Color.red,
                                0,
                                0);
            }
            else
            {
                GUI.DrawTexture(new Rect(220, 175, 20, 20), controller[9]);
            }
            if (inputPackage.StartButton)
            {
                GUI.DrawTexture(new Rect(280, 175, 20, 20), 
                                controller[9], 
                                ScaleMode.ScaleToFit,
                                true,
                                0,
                                Color.red,
                                0,
                                0);
            }
            else
            {
                GUI.DrawTexture(new Rect(280, 175, 20, 20), controller[9]);
            }

            GUI.DrawTexture(new Rect(245, 120, 30, 30), controller[10]);

            if (inputPackage.BumberLeft)
            {
                GUI.DrawTexture(new Rect(110, 65, 75, 40),
                                controller[11],
                                ScaleMode.ScaleToFit,
                                true,
                                0,
                                Color.red,
                                0,
                                0);
            }
            else
            {
                GUI.DrawTexture(new Rect(110, 65, 75, 40), controller[11]);
            }
            if (inputPackage.BumberRight)
            {
                GUI.DrawTexture(new Rect(335, 65, 75, 40), 
                                controller[12], 
                                ScaleMode.ScaleToFit,
                                true,
                                0,
                                Color.red,
                                0,
                                0);
            }
            else
            {
                GUI.DrawTexture(new Rect(335, 65, 75, 40), controller[12]);
            }
            if(inputPackage.TriggerLeft >= 0.4f)
            {
                GUI.DrawTexture(new Rect(130, 10, 50, 65), 
                                controller[13], 
                                ScaleMode.ScaleToFit,
                                true,
                                0,
                                Color.red,
                                0,
                                0);
            }
            else
            {
                GUI.DrawTexture(new Rect(130, 10, 50, 65), controller[13]);
            }
            if (inputPackage.TriggerRight >= 0.4f)
            {
                GUI.DrawTexture(new Rect(340, 10, 50, 65), 
                                controller[14], 
                                ScaleMode.ScaleToFit,
                                true,
                                0,
                                Color.red,
                                0,
                                0);
            }
            else
            {
                GUI.DrawTexture(new Rect(340, 10, 50, 65), controller[14]);
            }

            //GUILayout.Label("Horizontal Movement: " + inputPackage.MoveHorizontal.ToString(), style);
            //GUILayout.Label("Vertical Movement: " + inputPackage.MoveVertical.ToString(), style);
            //GUILayout.Label("Trigger Left: " + inputPackage.TriggerLeft.ToString(), style);
            //GUILayout.Label("Trigger Right: " + inputPackage.TriggerRight.ToString(), style);
            //GUILayout.Label("Cross Horizontal: " + inputPackage.CrossHorizontal, style);
            //GUILayout.Label("Cross Vertical: " + inputPackage.CrossVertical, style);
            //GUILayout.Label("Camera Horizontal: " + inputPackage.CameraHorizontal, style);
            //GUILayout.Label("Camera Vertical: " + inputPackage.CameraVertical, style);
            //GUILayout.Label("Left Bumper: " + inputPackage.BumberLeft, style);
            //GUILayout.Label("Right Bumper: " + inputPackage.BumberRight, style);
            //GUILayout.Label("X: " + inputPackage.InputX, style);
            //GUILayout.Label("A: " + inputPackage.InputA, style);
            //GUILayout.Label("B: " + inputPackage.InputB, style);
            //GUILayout.Label("Y: " + inputPackage.InputY, style);
            //GUILayout.Label("Start: " + inputPackage.StartButton, style);
            //GUILayout.Label("Select: " + inputPackage.SelectButton, style);
            //GUILayout.Label("Move Button: " + inputPackage.MoveButton, style);
            //GUILayout.Label("Camera Button: " + inputPackage.CameraButton, style);
        }
    }
}