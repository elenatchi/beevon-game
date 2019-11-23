using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BeevonMovement : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 0.5f;
    public float minSpeed = 0.5f;
    public float stopPoint = -0.5f;

    [Header("Shaking")]
    public UnityEvent shakeEvent = new UnityEvent();
    public Animator animator;

    [Header("Hurting")]
    public SkinnedMeshRenderer beevonBody;
    public Material beevonMat;
    public Material beevonMatHurt;
    public ParticleSystem pollenEmmiter;

    float accelerometerUpdateInterval = 1.0f / 60.0f;
    float lowPassKernelWidthInSeconds = 1.0f;
    float shakeDetectionThreshold = 2.0f;
    float lowPassFilterFactor;
    Vector3 lowPassValue;

    float shakeStartTime;
    float shakeEndTime;
    Rigidbody rigid;

    // Start is called before the first frame update
    void Start()
    {
        //Setup shaking
        lowPassFilterFactor = accelerometerUpdateInterval / lowPassKernelWidthInSeconds;
        shakeDetectionThreshold *= shakeDetectionThreshold;
        lowPassValue = Input.acceleration;

        Hive hive = GameObject.FindGameObjectWithTag("Hive").GetComponent<Hive>();
        shakeEvent.AddListener(hive.EmptyPollen);

        rigid = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        //Get input
        Vector3 acceleration = Input.acceleration;

        //Check for shake
        lowPassValue = Vector3.Lerp(lowPassValue, acceleration, lowPassFilterFactor);
        Vector3 deltaAcceleration = acceleration - lowPassValue;

        if (deltaAcceleration.sqrMagnitude >= shakeDetectionThreshold)
        {
            //Send shake Event
            shakeEvent.Invoke();
        }
        else
        {
            // If not shaking, move beevon
            MoveBeevon(acceleration);
        }
    }

    void MoveBeevon(Vector3 acceleration)
    {
        Vector3 dir = Vector3.zero;

        // we assume that device is held parallel to the ground
        // and Home button is in the right hand

        // remap device acceleration axis to game coordinates:
        //  1) XY plane of the device is mapped onto XZ plane
        //  2) rotated 90 degrees around Y axis
        dir.x = acceleration.y;
        dir.z = -acceleration.x;

        if (dir.x < stopPoint)
        {
            dir.x = 0;
            dir.z = 0;
        }
        else
        {
            dir.x = Mathf.Clamp(dir.x, minSpeed, 1);
        }

        // clamp acceleration vector to unit sphere
        if (dir.sqrMagnitude > 1)
            dir.Normalize();

        // Make it move 10 meters per second instead of 10 meters per frame...
        dir *= Time.deltaTime;

        // Move object
        transform.Translate(dir * speed);
    }

    public void AddShakeListener(GameObject flower)
    {
        FlowerScript flower_script = flower.GetComponent<FlowerScript>();
        shakeEvent.AddListener(flower_script.FillUpPollen);
    }

    public void Collect()
    {
        animator.SetBool("IsCollecting", true);
    }
    public void StopCollect()
    {
        animator.SetBool("IsCollecting", false);
    }

    public void HurtBeevon()
    {
        beevonBody.material = beevonMatHurt;
        animator.SetTrigger("Hurt");
        pollenEmmiter.Play();
    }

    public void UnhurtBeevon()
    {
        beevonBody.material = beevonMat;
    }
}
