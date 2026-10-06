using System;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

[RequireComponent(typeof(Rigidbody))]
public class ShipController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float maxSpeed;
    [Range(0f, 1f)] public float backwardSpeedMultiplier;
    public float acceleration;
    public float deceleration;
    public float turnSpeed;

    [Header("Tilt & Bank Settings")]
    public Rigidbody shipRigidbody;
    public Transform shipModel;
    public float minPitch;
    public float maxPitch;
    public float tiltSmooth;
    public float maxRoll;
    public float bankSmooth;

    [Header("Effects")]
    public WaterDeformer bowWawe;
    public ParticleSystem splash;
    private ParticleSystem.EmissionModule splashEmission;

    [Header("Sound")]
    public AudioSource engineAudioSource;
    public float maxSoundPitch, idleSoundPitch;

    [Header("Camera")]
    public Vector3 cameraOffset = new Vector3(0f, 0f, 0f);

    // Private Fields
    private float currentSpeed;
    private float currentPitch;
    private float currentRoll;

    void Start()
    {
        shipRigidbody.drag = 1.5f;
        shipRigidbody.angularDrag = 3f;
        shipRigidbody.freezeRotation = true; 
        splashEmission = splash.emission;
    }

    void FixedUpdate()
    {
        float moveInput = Input.GetAxis("Vertical");
        float turnInput = Input.GetAxis("Horizontal");

        if (moveInput != 0)
        {
            float targetSpeed = moveInput > 0 ? moveInput * maxSpeed : moveInput * maxSpeed * backwardSpeedMultiplier;
            currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.fixedDeltaTime);
        }
        else
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0, deceleration * Time.fixedDeltaTime);
        }

        Vector3 moveDirection = transform.forward * currentSpeed;
        shipRigidbody.velocity = new Vector3(moveDirection.x, shipRigidbody.velocity.y, moveDirection.z);

        if (Mathf.Abs(currentSpeed) > 0.5f)
        {
            float directionMultiplier = Mathf.Sign(currentSpeed);
            float turn = turnInput * turnSpeed * directionMultiplier * Time.fixedDeltaTime;
            transform.Rotate(0, turn, 0);
        }

        float speedRatio = Mathf.Abs(currentSpeed) / maxSpeed;
        float targetPitch = Mathf.Lerp(minPitch, maxPitch, speedRatio);
        currentPitch = Mathf.Lerp(currentPitch, targetPitch, tiltSmooth * Time.fixedDeltaTime);

        float targetRoll = -turnInput * maxRoll * speedRatio;
        currentRoll = Mathf.Lerp(currentRoll, targetRoll, bankSmooth * Time.fixedDeltaTime);

        Vector3 currentEuler = shipModel.localEulerAngles;
        shipModel.localEulerAngles = new Vector3(currentPitch, currentEuler.y, currentRoll);

        bowWawe.amplitude = currentSpeed > 0 ? Mathf.Min(currentSpeed / maxSpeed * 2f, 0.5f) : 0;
        bowWawe.bowWaveElevation = currentSpeed > 0 ? Mathf.Min(currentSpeed / maxSpeed * 1.5f, 0.66f) : 0;
        splashEmission.rateOverDistance = currentSpeed > 8 ? currentSpeed : (ParticleSystem.MinMaxCurve)0;
    }

    private void LateUpdate()
    {
        engineAudioSource.pitch = Mathf.Clamp(idleSoundPitch + (Math.Abs(currentSpeed) / maxSpeed) * (maxSoundPitch - idleSoundPitch), idleSoundPitch, maxSoundPitch);
    }
}