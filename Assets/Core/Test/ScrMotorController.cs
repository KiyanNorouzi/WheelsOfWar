using UnityEngine;
using System.Collections;

public class ScrMotorController : MonoBehaviour {

    private Vector3 accel;
    private float throttle;
    private float deadZone=0.001f;
    private Vector3 myRight;
    private Vector3 velocity;
    private Vector3 flatVelocity;
    private Vector3 relativeVelocity;
    private Vector3 dir;
    private Vector3 flatDir;
    private Vector3 up;
    private Transform tr;
    private Rigidbody rb;
    private Vector3 engineForce;

    private Vector3 turnVec;
    private Vector3 imp;
    private float rev;
    private float actualTurn;
    private float mass;
    private Transform[] wheelTransform=new Transform[4];
    public float actualGrip;
    public float horizontal;
    private float maxSpeedToTurn = 0.2f;

    // the physical transforms for the car's wheel's
    public Transform frontLeftWheel;
    public Transform frontRightWheel;
    public Transform rearLeftWheel;
    public Transform rearRightWheel;


    public Transform LFWheelTransform;
    public Transform RFWheelTransform;

    public float power = 300;
    public float maxSpeed = 50;
    public float carGrip = 70;
    public float turnSpeed = 3.0f;

    private float slideSpeed;
    public float mySpeed;

    private Vector3 right;
    private Vector3 fwd;
    private Vector3 tempVEC;

    public Vector3 centerOfMass = new Vector3(0, -0.7f, 0.35f);

    void Awake()
    {
        // Statements
    }

    void Start()
    {
        Initialize();
    }

    void LateUpdate()
    {
        RotateVisualWheels();
        SoundEngine();
    }

    void Update()
    {
        carPhysicsUpdate();
        CheckInput();
    }

    void FixedUpdate()
    {
        MotorProcces();
    }

    void Initialize()
    {
        tr = transform;
        rb = rigidbody;
        up = tr.up;
        mass = rigidbody.mass;
        fwd = Vector3.forward;
        right = Vector3.right;

        rb.centerOfMass = centerOfMass;

        SetupWheels();
    }

    void SetupWheels()
    {
        wheelTransform[0] = frontLeftWheel;
        wheelTransform[1] = rearLeftWheel;
        wheelTransform[2] = frontRightWheel;
        wheelTransform[3] = rearRightWheel;

        for(int i=0;i<wheelTransform.Length;i++)
            if(wheelTransform[i]==null)
            {
                Debug.LogError("Check Wheel's ,a wheel is null");
                Debug.Break();
            }
    }

    private Vector3 rotationAmount;
    private Vector3 rotYWheelTransform;
    void RotateVisualWheels()
    {
        rotYWheelTransform = LFWheelTransform.localEulerAngles;
        rotYWheelTransform.y=horizontal * 30;
        LFWheelTransform.localEulerAngles = rotYWheelTransform;

        rotYWheelTransform = RFWheelTransform.localEulerAngles;
        rotYWheelTransform.y=horizontal * 30;
        RFWheelTransform.localEulerAngles = rotYWheelTransform;

        rotationAmount = right * (relativeVelocity.z * 1.6f * Time.deltaTime * Mathf.Rad2Deg);

        for (int i = 0; i < wheelTransform.Length;i++)
            wheelTransform[i].Rotate(rotationAmount);
    }

    private float deviceAccelerometerSesitivy = 2;

    void CheckInput()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
        {
            accel = Input.acceleration * deviceAccelerometerSesitivy;

            if (accel.x > deadZone || accel.x < -deadZone)
            {
                horizontal = accel.x;
            }
            else
            {
                horizontal = 0;
            }
            throttle = 0;

            foreach (Touch t in Input.touches)
            {
                if (t.position.x > Screen.width - Screen.width / 3 && t.position.y < Screen.height / 3)
                {
                    throttle = 1;
                }
                else
                    if (t.position.x < Screen.width / 3 && t.position.y < Screen.height / 3)
                    {
                        throttle = -1;
                    }
            }
        }
        else
            if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsWebPlayer || Application.platform == RuntimePlatform.WindowsPlayer)
        {
            horizontal = Input.GetAxis("Horizontal");
            throttle = Input.GetAxis("Vertical");
        }
    }

    public float carDown;

    void carPhysicsUpdate()
    {
        myRight = tr.right;
        velocity = rb.velocity;
        tempVEC =new Vector3(velocity.x, 0, velocity.z);
        flatVelocity = tempVEC;
        if(velocity.magnitude>2)
            velocity.y = -carDown;

        //Debug.Log("velocity : " + velocity.magnitude);

        rb.velocity = velocity;
        dir = tr.TransformDirection(fwd);
        tempVEC=new Vector3(dir.x,0,dir.z);
        flatDir = Vector3.Normalize(tempVEC);
        relativeVelocity = tr.InverseTransformDirection(flatVelocity);
        slideSpeed = Vector3.Dot(myRight, flatVelocity);
        mySpeed = flatVelocity.magnitude;
        rev = Mathf.Sign(Vector3.Dot(flatVelocity, flatDir));
        engineForce = (flatDir * (power * throttle) * mass);
        actualTurn = horizontal;

        if(rev<0.01f)
        {
            actualTurn = -actualTurn;
        }

        turnVec = (((up * turnSpeed) * actualTurn) * mass) * 80;

        actualGrip = Mathf.Lerp(100, carGrip, mySpeed * 0.02f);

       // imp = myRight * (-slideSpeed * mass * actualTurn);
    }

    void slowVelocity()
    {
        rb.AddForce(-flatVelocity * 0.8f);
    }

    void SoundEngine()
    {
        audio.pitch = 0.30f * mySpeed * 0.025f;

            if(mySpeed>30)
            {
                audio.pitch = 0.25f * mySpeed * 0.015f;
            }
        else
            if (mySpeed > 40)
            {
                audio.pitch = 0.20f * mySpeed * 0.013f;
            }
        else
            if (mySpeed > 49)
            {
                audio.pitch = 0.15f * mySpeed * 0.011f;
            }

        if (audio.pitch > 2.0f)
            audio.pitch = 2.0f;
    }

    void MotorProcces()
    {
        if (mySpeed < maxSpeed)
            rb.AddForce(engineForce * Time.deltaTime);

        if (mySpeed > maxSpeedToTurn)
            rb.AddTorque(turnVec * velocity.magnitude * Time.deltaTime);
        else
            if (mySpeed < maxSpeedToTurn)
                return;

        rb.AddForce(imp * Time.deltaTime);
    }
}
