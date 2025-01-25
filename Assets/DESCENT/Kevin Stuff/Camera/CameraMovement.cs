using System;
using System.Collections;
using System.Runtime.Serialization;
using UnityEngine;


public class CameraMovement : MonoBehaviour
{
    [Header("Fields")]
    public float maxMovementSpeed;

    public float maxDistance;

    public float minCamFOV;
    public float maxCamFOV;
    public float midAirCamFOV;

    public float minSpeedlinesEmission;
    public float maxSpeedlinesEmission;

    [Tooltip("When you are midair, this number is added to the current emissions")]
    public float midairSpeedlinesExtraEmissionCount;

    [Header("References")]
    public Transform target;
    public Transform camTransform;
    public Camera cameraValues;

    public ParticleSystem speedlines;

    public Animator camAnimator;

    private FloatTransition floatTransition = new FloatTransition();


    public float camSpeed;

    private float timeInAir;

    public CameraState CurrentState
    {
        get => m_CurrentState;
        set
        {
            switch (value)
            {
                case CameraState.Grounded:
                    OnGrounded();
                    m_UpdateHandler = GroundedBehavior;
                    break;
                case CameraState.Midair:
                    OnMidair();
                    m_UpdateHandler = MidairBehavior;
                    break;
                default:
                    break;
            }

            m_CurrentState = value;
        }
    }
    private CameraState m_CurrentState;

    private delegate void UpdateHandler();
    private UpdateHandler m_UpdateHandler;

    public enum CameraState
    {
        Grounded,
        Midair
    }

    Quaternion defaultRotation;

    // Start is called before the first frame update
    void Start()
    {
        defaultRotation = transform.rotation;
        transform.position = target.position;

        timeInAir = 0f;

        CurrentState = CameraState.Grounded;


        //floatTransition = new FloatTransition();
    }

    Vector3 vectorToTarget;
    float distanceToTarget;

    // Update is called once per frame
    private void Update()
    {

        vectorToTarget = (target.position - this.transform.position);
        distanceToTarget = vectorToTarget.magnitude;

        m_UpdateHandler?.Invoke();
    }

    #region Behavior Functions

    Vector3 moveVel;

    private void SmoothMoveWithPlayer()
    {
        // Scale the moveVel by the xFactor in the rightward direction to make x-axis adjust more
        float x = Vector3.Dot(Vector3.right, vectorToTarget);

        vectorToTarget = (target.position - this.transform.position);
        distanceToTarget = vectorToTarget.magnitude;

        Vector3 moveVel = Time.deltaTime * maxMovementSpeed * Mathf.Lerp(0f,
                1f,
                distanceToTarget * distanceToTarget / (maxDistance * maxDistance))
                * vectorToTarget.normalized;

        // Cap max distance
        if (distanceToTarget > maxDistance)  //Using sqrmag is more performant
        {
            this.transform.position = target.position - vectorToTarget.normalized * maxDistance;
        }

        transform.position += moveVel;

    }

    private void SmoothRotateWithPlayer()
    {
        Quaternion targetRotation = Quaternion.Euler(target.transform.rotation.eulerAngles.x - 20f, 0f, 0f);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 20f * Time.deltaTime);
    }

    private void OnGrounded()
    {
        // Override the FOV changing
        if (changeFOVCoroutine != null)
        {
            StopCoroutine(changeFOVCoroutine);
            changeFOVCoroutine = null;
        }

        ChangeFieldOfView(Mathf.SmoothStep(minCamFOV,
            maxCamFOV,
            camSpeed / maxMovementSpeed),
            0.4f
        );

        // Camera Shake
        if (timeInAir >= 1f)
        {
            camAnimator.Play("CameraShakeOnLand");
        }
        timeInAir = 0f;
    }

    private void GroundedBehavior()
    {
        SmoothMoveWithPlayer();
        SmoothRotateWithPlayer();

        // Dynamically change the field of view
        ChangeFieldOfView(Mathf.SmoothStep(minCamFOV,
            maxCamFOV,
            camSpeed / maxMovementSpeed),
            0.0f
        );

        ChangeSpeedlinesEmission(Mathf.Lerp(minSpeedlinesEmission, maxSpeedlinesEmission, camSpeed / maxMovementSpeed), 0.0f);
        // Rotation
    }

    private void OnMidair()
    {
        if (changeFOVCoroutine != null)
        {
            StopCoroutine(changeFOVCoroutine);
            changeFOVCoroutine = null;
        }

        ChangeFieldOfView(midAirCamFOV, 0.5f);
    }

    private void MidairBehavior()
    {
        SmoothMoveWithPlayer();
        SmoothRotateWithPlayer();

        timeInAir += Time.deltaTime;

        ChangeSpeedlinesEmission(speedlines.emission.rateOverTime.constant + midairSpeedlinesExtraEmissionCount, 0.2f);
    }

    #endregion

    #region Transition Functions

    Coroutine changeSpeedlinesCoroutine;
    private void ChangeSpeedlinesEmission(float newValue, float transitionTime)
    {
        var emission = speedlines.emission;
        var emissionRate = emission.rateOverTime; // Get the actual MinMaxCurve

        TryChangeFloatTransition(ref changeSpeedlinesCoroutine, emissionRate.constant, newValue, transitionTime,
            (float f) =>
            {
                var rateOverTime = emission.rateOverTime;
                rateOverTime.constant = f; // Set the updated value back to the MinMaxCurve
                emission.rateOverTime = rateOverTime; // Assign the updated curve back to the emission module
            },
            () => changeSpeedlinesCoroutine = null);
    }

    Coroutine changeFOVCoroutine;
    private void ChangeFieldOfView(float newFOV, float transitionTime)
    {
        TryChangeFloatTransition(ref changeFOVCoroutine, cameraValues.fieldOfView, newFOV, transitionTime,
            (float f) => cameraValues.fieldOfView = f, () => changeFOVCoroutine = null);
    }

    public bool TryChangeFloatTransition(ref Coroutine c, float oldF, float newF, float transitionTime,
        Action<float> funcToDo, Action end)
    {
        if (c == null)
        {
            if (transitionTime == 0.0f)
            {
                funcToDo(newF);
            }
            else
            {
                c = StartCoroutine(floatTransition.ChangeFloatTransition(oldF, newF, transitionTime,
                    funcToDo, end));
            }
            return true;
        }

        return false;
    }

    #endregion



    private void OnTriggerStay(Collider collision)
    {
        //transform.position += new Vector3(0f, 10f * Time.deltaTime, 0f);

        print("HI");
    }
}


public class FloatTransition
{
    public IEnumerator ChangeFloatTransition(float oldF, float newF, float transitionTime,
        Action<float> OnTransition, Action OnCompletion)
    {
        float timeChanging = 0f;

        //OnStart();

        while (true)
        {
            timeChanging += Time.deltaTime;

            OnTransition(Mathf.SmoothStep(oldF, newF, timeChanging / transitionTime));

            //cameraValues.fieldOfView = Mathf.SmoothStep(oldFOV, newFOV, timeChanging / transitionTime);

            if (timeChanging >= transitionTime)
            {
                break;
            }
            yield return new WaitForEndOfFrame();
        }

        OnCompletion();
    }

    //public IEnumerator ChangeFloatTransition(float oldF, float newF, float transitionTime,
    //    Action<float> OnTransition)
    //{
    //    float timeChanging = 0f;

    //    while (true)
    //    {
    //        timeChanging += Time.deltaTime;

    //        OnTransition(Mathf.SmoothStep(oldF, newF, timeChanging / transitionTime));

    //        //cameraValues.fieldOfView = Mathf.SmoothStep(oldFOV, newFOV, timeChanging / transitionTime);

    //        if (timeChanging >= transitionTime)
    //        {
    //            break;
    //        }
    //        yield return new WaitForEndOfFrame();
    //    }
    //}
}

