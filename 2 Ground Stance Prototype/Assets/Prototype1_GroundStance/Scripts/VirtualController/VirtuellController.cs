using System.Collections.Generic;
using UnityEngine;

public class VirtuellController : MonoBehaviour
{
    IProvider inputProvider;
    InputPackage inputPackage;

    [SerializeField] private CameraFollow cam;

    [SerializeField] List<Texture2D> controller;
    
    bool debugOn = false;

    public InputPackage InputPackage { get => inputPackage; set => inputPackage = value; }

    private void Awake()
    {
        inputProvider = gameObject.GetComponent<PlayerInput>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F4))
        {
            debugOn = !debugOn;
        }
        if (cam != null)
        {
            cam.InputPackage = inputProvider.GetPackage();
        }
    }

    public InputPackage GetPackageForActor()
    {
        InputPackage package = inputProvider.GetPackage();
        return package;
    }

    private void OnGUI()
    {
        InputPackage = inputProvider.GetPackage();

        if (debugOn)
        {
            GUI.DrawTexture(new Rect(35, 100, 455, 275), controller[0]);
            GUI.DrawTexture(new Rect(115, 150, 70, 70), controller[1]);
            if(InputPackage.MoveHorizontal >= 0.1f || 
               InputPackage.MoveHorizontal <= -0.1f || 
               InputPackage.MoveVertical >= 0.1f || 
               InputPackage.MoveVertical <= -0.1f)
            {
                GUI.DrawTexture(new Rect(130 + (InputPackage.MoveHorizontal * 15), 165 - (InputPackage.MoveVertical * 15), 40, 40),
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
            if (InputPackage.CameraHorizontal >= 0.1f ||
                InputPackage.CameraHorizontal <= -0.1f ||
                InputPackage.CameraVertical >= 0.1f ||
                InputPackage.CameraVertical <= -0.1f)
            {
                GUI.DrawTexture(new Rect(295 + (InputPackage.CameraHorizontal * 15), 230 + (InputPackage.CameraVertical * 15), 40, 40), 
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
            if (InputPackage.CrossHorizontal >= 0.1f ||
                InputPackage.CrossHorizontal <= -0.1f ||
                InputPackage.CrossVertical >= 0.1f ||
                InputPackage.CrossVertical <= -0.1f)
            {
                if(InputPackage.CrossHorizontal >= 0.1f)
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
                if (InputPackage.CrossHorizontal <= -0.1f)
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
                if (InputPackage.CrossVertical >= 0.1f)
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
                if (InputPackage.CrossVertical <= -0.1f)
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
            if(InputPackage.InputX)
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
            if(InputPackage.InputA)
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
            if(InputPackage.InputB)
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
            if(InputPackage.InputY)
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
            if(InputPackage.SelectButton)
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
            if (InputPackage.StartButton)
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

            if (InputPackage.BumberLeft)
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
            if (InputPackage.BumberRight)
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
            if(InputPackage.TriggerLeft >= 0.4f)
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
            if (InputPackage.TriggerRight >= 0.4f)
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