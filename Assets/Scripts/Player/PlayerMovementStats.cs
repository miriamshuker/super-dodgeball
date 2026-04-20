using UnityEngine;

[CreateAssetMenu(menuName ="Player Movement")]
public class PlayerMovementStats : ScriptableObject
{
    [Header("Walk")]
    public float MaxWalkSpeed = 12.5f;
    public float GroundAcceleration = 5f;
    public float GroundDeceleration = 20f;
    public float AirAcceleration = 5f;
    public float AirDeceleration = 5f;

    
    [Header("Grounded/Collision Checks")]
    public LayerMask GroundLayer;
    public float GroundDetectionRayLength = 0.02f;
    public float HeadDetectionRayLength = 0.02f;
    [Range(0f,1f)] public float HeadWidth = 0.75f;

    
    [Header("Jump")]
    public float JumpHeight = 6.5f;
    public float JumpHeightCompensationFactor = 1.054f;
    public float TimeTillJumpApex = 0.35f;
    public float GravityOnReleaseMultiplier = 32f;
    public float MaxFallSpeed = 26f;
    public int NumberofJumpsAllowed = 2;

     
    [Header("Jump Cut")]
    public float TimeForUpwardsCancel = 0.027f;

    
    [Header("Jump Apex")]
    public float ApexThreshold = 0.97f;
    public float ApexHangTime = 0.075f;

    
    [Header("Jump Buffer")]
    public float JumpBufferTime = 0.125f;

    
    [Header("Jump Coyote Time")]
    [Range(0f, 1f)] public float JumpCoyoteTime = 0.1f;


    public float Gravity {get; private set;}
    public float InitialJumpVelocity{get; private set;}
    //helps player "close gap" to get to the actual jump height
    public float AdjustedJumpHeight{get; private set;}

    private void OnValidate()
    {
        CalculateValues();
    }

    private void OnEnable()
    {
        CalculateValues();
    }


    //calculate gravity and initial jump height
    private void CalculateValues()
    {
        AdjustedJumpHeight = JumpHeight * JumpHeightCompensationFactor;
        Gravity = -(2f * AdjustedJumpHeight / Mathf.Pow(TimeTillJumpApex, 2f));
        InitialJumpVelocity = Mathf.Abs(Gravity) * TimeTillJumpApex;
    }


}
