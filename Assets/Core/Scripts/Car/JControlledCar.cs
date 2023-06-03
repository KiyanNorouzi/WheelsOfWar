using UnityEngine;
using System.Collections;

public class JControlledCar : JCar
{

    public float shiftDownRPM = 1500.0f; // rpm script will shift gear down
    public float shiftUpRPM = 2700.0f; // rpm script will shift gear up

   

    
    private PhotonView nv;
    private Transform tr;

    /*[System.Serializable]
    public class AudioStruct
    {
        public AudioClip[] gears;
        [HideInInspector]
        public AudioSource AS;

        public void Init(GameObject go)
        {
            go.AddComponent<AudioSource>();
            AS = go.GetComponent<AudioSource>();
        }

        public void Play(AudioClip ac)
        {
            AS.clip = ac;
            AS.Play();
        }

        public void PlayCheck(AudioClip ac)
        {
            if (!AS.isPlaying)
                AS.Play();
        }

        public void PlayCheckClip(AudioClip ac)
        {
            if (AS.clip != ac)
                AS.clip = ac;

            if (!AS.isPlaying)
                AS.Play();
        }

    }*/

    //public AudioStruct sound;

    void Awake()
    {
        nv = GetComponent<PhotonView>();
        tr = transform;

        if (!nv.isMine)
            Destroy(checkForActive);

        //sound.Init(gameObject);
    }

    [HideInInspector]
    public bool brake;
    [HideInInspector]
    public float accel;
    [HideInInspector]
    public float steer;

    // handle the physics of the engine
    void FixedUpdate()
    {
        if (!nv.isMine)
            return;


        if ((checkForActive == null) || checkForActive.activeSelf)
        {
            /*
            if (Mathf.Abs(accel) > 0)
                sound.PlayCheckClip(sound.gears[0]);
            else sound.AS.Stop();
             * */
        }


        // handle automatic shifting
        if ((CurrentGear == 1) && (accel < 0.0f))
        {
            ShiftDown(); // reverse
        }
        else if ((CurrentGear == 0) && (accel > 0.0f))
        {
            ShiftUp(); // go from reverse to first gear
        }
        else if ((MotorRPM > shiftUpRPM) && (accel > 0.0f))
        {
            ShiftUp(); // shift up
        }
        else if ((MotorRPM < shiftDownRPM) && (CurrentGear > 1))
        {
            ShiftDown(); // shift down
        }
        if (CurrentGear == 0)
        {
            //accel = -accel; // in automatic mode we need to hold arrow down for reverse
        }

        HandleMotor(steer, accel);
        CalculateRevs();
    }



        
    // for Rev

    public float Revs { get; private set; }
    private float m_GearFactor;
    //[SerializeField]
    private float m_RevRangeBoundary = 1f;
    private float maxSpeed = 200;
    // simple function to add a curved bias towards 1 for a value in the 0-1 range
    private static float CurveFactor(float factor)
    {
        return 1 - (1 - factor) * (1 - factor);
    }


    // unclamped version of Lerp, to allow value to exceed the from-to range
    private static float ULerp(float from, float to, float value)
    {
        return (1.0f - value) * from + value * to;
    }


    private void CalculateGearFactor()
    {
        float f = (1 / (float)gears.Length);
        // gear factor is a normalised representation of the current speed within the current gear's range of speeds.
        // We smooth towards the 'target' gear factor, so that revs don't instantly snap up or down when changing gear.
        var targetGearFactor = Mathf.InverseLerp(f * CurrentGear, f * (CurrentGear + 1), Mathf.Abs(CurrentSpeed / maxSpeed));
        m_GearFactor = Mathf.Lerp(m_GearFactor, targetGearFactor, Time.deltaTime * 5f);
    }


    private void CalculateRevs()
    {
        // calculate engine revs (for display / sound)
        // (this is done in retrospect - revs are not used in force/power calculations)
        CalculateGearFactor();
        var gearNumFactor = CurrentGear / (float)gears.Length;
        var revsRangeMin = ULerp(0f, m_RevRangeBoundary, CurveFactor(gearNumFactor));
        var revsRangeMax = ULerp(m_RevRangeBoundary, 1f, gearNumFactor);
        Revs = ULerp(revsRangeMin, revsRangeMax, m_GearFactor);
    }

    public float rotateK;
    public void RotateWheels(float distance)
    {
        wheelBL.Rotate(Vector3.right, distance * rotateK);
        wheelBLB.Rotate(Vector3.right, distance * rotateK);
        wheelBR.Rotate(Vector3.right, distance * rotateK);
        wheelBRB.Rotate(Vector3.right, distance * rotateK);
    }

    public void StopMovingFor(float time)
    {
        enabled = false;
        motorRPM = 0;

        Invoke("StartMoving", time);
    }

    public void StartMoving()
    {
        enabled = true;
    }
}
