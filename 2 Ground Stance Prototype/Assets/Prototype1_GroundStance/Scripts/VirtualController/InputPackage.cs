public class InputPackage
{
    private float moveHorizontal = 0;
    private float moveVertical = 0;
    private float triggerLeft = 0;
    private float triggerRight = 0;
    private float crossHorizontal = 0;
    private float crossVertical = 0;
    private float cameraHorizontal = 0;
    private float cameraVertical = 0;

    private bool inputX = false;
    private bool inputA = false;
    private bool inputB = false;
    private bool inputY = false;
    private bool bumperLeft = false;
    private bool bumperRight = false;
    private bool moveButton = false;
    private bool cameraButton = false;
    private bool startButton = false;
    private bool selectButton = false;

    public float MoveHorizontal { get => moveHorizontal; set => moveHorizontal = value; }
    public float MoveVertical { get => moveVertical; set => moveVertical = value; }
    public float TriggerLeft { get => triggerLeft; set => triggerLeft = value; }
    public float TriggerRight { get => triggerRight; set => triggerRight = value; }
    public float CrossHorizontal { get => crossHorizontal; set => crossHorizontal = value; }
    public float CrossVertical { get => crossVertical; set => crossVertical = value; }
    public float CameraHorizontal { get => cameraHorizontal; set => cameraHorizontal = value; }
    public float CameraVertical { get => cameraVertical; set => cameraVertical = value; }

    public bool InputX { get => inputX; set => inputX = value; }
    public bool InputA { get => inputA; set => inputA = value; }
    public bool InputB { get => inputB; set => inputB = value; }
    public bool InputY { get => inputY; set => inputY = value; }
    public bool BumberLeft { get => bumperLeft; set => bumperLeft = value; }
    public bool BumberRight { get => bumperRight; set => bumperRight = value; }
    public bool MoveButton { get => moveButton; set => moveButton = value; }
    public bool CameraButton { get => cameraButton; set => cameraButton = value; }
    public bool StartButton { get => startButton; set => startButton = value; }
    public bool SelectButton { get => selectButton; set => selectButton = value; }
}
