using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Serialized Fields
    [Header("Camera and Animation")]
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private Animator animatorPlayer;

    [Header("Ragdoll Configuration")]
    [SerializeField]
    private GameObject ragdollPlayer;
    [SerializeField]
    private ConfigurableJoint[] ragdollParts;

    [Header("Hand Controllers")]
    [SerializeField]
    public HandController HandControllerRight, HandControllerLeft;

    [Header("Controls")]
    [SerializeField]
    public bool useControls = true;
    [SerializeField]
    public string ragdollLayer = "Player", forwardBackAxis = "Vertical", 
    leftRightAxis = "Horizontal", jump = "Jump", 
    reachLeft = "Fire1", reachRight = "Fire2";

    [Header("Movement Settings")]
    [SerializeField]
    private float moveSpeed = 6f, turnSpeed = 6f, 
    jumpForce = 6f, HeightCheck = 1.1f;

    [Header("Layer Masks")]
    [SerializeField]
    private LayerMask ignoreGroundCheckOn;
    #endregion

    #region Private Fields
    private float timer, cachedMoveSpeed, mouseYaxis = 0.5f;
    private bool jumping, isJumping, jumpAxisUsed, inAir, ragdollMode, cooledDown = true;
    private Vector3 Direction;
    private Vector2 InputDirection;
    private int animLayer = 0;
    
    private Rigidbody physicsBody;
    private ConfigurableJoint physicsJoint;

    private JointDrive DriveOff, DriveLow, DriveMedium, DriveHigh, DriveOnController, DriveOnRagdoll;
    #endregion

    #region Initialization
    void Awake()
    {
        InitializePhysicsComponents();
        InitializeJointDrives();
        ConfigureRagdollParts();
    }

    void InitializePhysicsComponents()
    {
        cachedMoveSpeed = moveSpeed;
        physicsBody = ragdollPlayer.GetComponent<Rigidbody>();
        physicsJoint = ragdollPlayer.GetComponent<ConfigurableJoint>();
    }

    void InitializeJointDrives()
    {
        DriveOff = CreateJointDrive(150, 500, Mathf.Infinity);
        DriveLow = CreateJointDrive(500, 600, Mathf.Infinity);
        DriveMedium = CreateJointDrive(2000, 600, Mathf.Infinity);
        DriveHigh = CreateJointDrive(12000, 1000, Mathf.Infinity);
        DriveOnController = CreateJointDrive(15000, 2000, Mathf.Infinity);
        DriveOnRagdoll = CreateJointDrive(20000, 600, Mathf.Infinity);
    }

    void ConfigureRagdollParts()
    {
        foreach (ConfigurableJoint part in ragdollParts)
        {
            part.slerpDrive = DriveOnRagdoll;  
        }
    }

    private JointDrive CreateJointDrive(float spring, float damper, float maxForce)
    {
        JointDrive drive = new JointDrive
        {
            positionSpring = spring,
            positionDamper = damper,
            maximumForce = maxForce
        };
        return drive;
    }
    #endregion
    void Update()
    {
        PlayerGrounded();
        
        if(useControls)
        {
            if(!ragdollMode)
            {
                PlayerInput();
                PlayerSpeed();
                PlayerRotation();
                PlayerJump();
            }
            
            if(!inAir)
            {
                PlayerMovement();
            }
            
            else if(inAir)
            {
                PlayerMovementAir();
            }
            
        }
        
        if(ragdollPlayer.activeSelf)
        {
            PlayerAnimations();
        }
    }
    //---Fixed Updates
    void FixedUpdate()
    {
        if(useControls)
        {
            PlayerJumping();
        }
    }
    void PlayerGrounded()
    {
        Ray ray = new Ray (ragdollPlayer.transform.position, -Vector3.up);
		RaycastHit hit;
		
		//Balance when ground is detected
        if (Physics.Raycast(ray, out hit, HeightCheck, ~ignoreGroundCheckOn) && inAir && !ragdollMode)
        {
            inAir = false;
            physicsJoint.slerpDrive = DriveOnController;
            
            ragdollParts[1].slerpDrive = DriveOnRagdoll;
            ragdollParts[2].slerpDrive = DriveOnRagdoll;
            ragdollParts[3].slerpDrive = DriveOnRagdoll;
            ragdollParts[4].slerpDrive = DriveOnRagdoll;
            ragdollParts[5].slerpDrive = DriveOnRagdoll;
            ragdollParts[6].slerpDrive = DriveOnRagdoll;
		}
		
		//Fall when ground is not detected
		else if(!Physics.Raycast(ray, out hit, HeightCheck, ~ignoreGroundCheckOn) && !inAir && !ragdollMode && physicsBody.velocity.y > -10)
		{
            inAir = true;
            physicsJoint.slerpDrive = DriveLow;
            
            ragdollParts[1].slerpDrive = DriveOff;
            ragdollParts[2].slerpDrive = DriveOff;
            ragdollParts[3].slerpDrive = DriveOff;
            ragdollParts[4].slerpDrive = DriveOff;
            ragdollParts[5].slerpDrive = DriveOff;
            ragdollParts[6].slerpDrive = DriveOff;
		}
        
        //Switch to ragdoll mode when falling fast
        else if(!Physics.Raycast(ray, out hit, HeightCheck, ~ignoreGroundCheckOn) && inAir && !ragdollMode && physicsBody.velocity.y < -10)
		{
            ragdollMode = true;
            cooledDown = false;
            
            physicsJoint.slerpDrive = DriveOff;
            
            ActivateRagdoll();
		}
    }  
    //---Player Get Up
    public void PlayerGetUp()
    {
        //Slowly transition from ragdoll to active ragdoll
        if(ragdollMode && !cooledDown)
        {
            cooledDown = true;
            StartCoroutine(waitCoroutine());
            
            IEnumerator waitCoroutine()
            {
                yield return new WaitForSeconds(2);
                
                foreach (ConfigurableJoint part in ragdollParts)
                {
                   part.slerpDrive = DriveMedium;  
                }
                
                ragdollParts[7].slerpDrive = DriveOff;
                ragdollParts[8].slerpDrive = DriveOff;
                ragdollParts[9].slerpDrive = DriveOff;
                ragdollParts[10].slerpDrive = DriveOff;
                ragdollParts[11].slerpDrive = DriveOff;
                ragdollParts[12].slerpDrive = DriveOff;
                ragdollParts[13].slerpDrive = DriveOff;
                ragdollParts[14].slerpDrive = DriveOff;
                
                yield return new WaitForSeconds(0.5f);
                
                foreach (ConfigurableJoint part in ragdollParts)
                {
                   part.slerpDrive = DriveHigh;  
                }
                
                ragdollParts[7].slerpDrive = DriveOff;
                ragdollParts[8].slerpDrive = DriveOff;
                ragdollParts[9].slerpDrive = DriveOff;
                ragdollParts[10].slerpDrive = DriveOff;
                ragdollParts[11].slerpDrive = DriveOff;
                ragdollParts[12].slerpDrive = DriveOff;
                ragdollParts[13].slerpDrive = DriveOff;
                ragdollParts[14].slerpDrive = DriveOff;
                
                yield return new WaitForSeconds(0.5f);
                
                physicsJoint.slerpDrive = DriveLow;
                
                yield return new WaitForSeconds(0.5f);
                
                physicsJoint.slerpDrive = DriveMedium;
                
                foreach (ConfigurableJoint part in ragdollParts)
                {
                   part.slerpDrive = DriveMedium;  
                }
                
                yield return new WaitForSeconds(0.5f);
                
                physicsJoint.slerpDrive = DriveHigh;
                
                foreach (ConfigurableJoint part in ragdollParts)
                {
                   part.slerpDrive = DriveHigh;  
                }
                
                yield return new WaitForSeconds(0.5f);
                
                ragdollMode = false;
                DeactivateRagdoll();
            }
        }
    }

    //---Player Input
    void PlayerInput()
    {
        if(Input.GetAxisRaw(forwardBackAxis) > 0)
        {
            InputDirection = new Vector2(InputDirection.x, 1);
            
            if(animatorPlayer.GetFloat("BlendVertical") < 1)
            {
                var vert = animatorPlayer.GetFloat("BlendVertical");
                animatorPlayer.SetFloat("BlendVertical", vert += 0.05f);
            }
        }
        
        else if(Input.GetAxisRaw(forwardBackAxis) < 0)
        {
            InputDirection = new Vector2(InputDirection.x, -1);
            
            if(animatorPlayer.GetFloat("BlendVertical") > -1)
            {
                var vert = animatorPlayer.GetFloat("BlendVertical");
                animatorPlayer.SetFloat("BlendVertical", vert -= 0.05f);
            }
        }
        
        else if(Input.GetAxisRaw(forwardBackAxis) == 0)
        {
            InputDirection = new Vector2(InputDirection.x, 0);
            
            if(animatorPlayer.GetFloat("BlendVertical") != 0)
            {
                animatorPlayer.SetFloat("BlendVertical", 0f);
            }
        }
        
        if(Input.GetAxisRaw(leftRightAxis) > 0)
        {
            InputDirection = new Vector2(1, InputDirection.y);
            
            if(animatorPlayer.GetFloat("BlendHorizontal") < 1)
            {
                var horiz = animatorPlayer.GetFloat("BlendHorizontal");
                animatorPlayer.SetFloat("BlendHorizontal", horiz += 0.05f);
            }
        }
        
        else if(Input.GetAxisRaw(leftRightAxis) < 0)
        {
            InputDirection = new Vector2(-1, InputDirection.y);
            
            if(animatorPlayer.GetFloat("BlendHorizontal") > -1)
            {
                var horiz = animatorPlayer.GetFloat("BlendHorizontal");
                animatorPlayer.SetFloat("BlendHorizontal", horiz -= 0.05f);
            }
        }
        
        else if(Input.GetAxisRaw(leftRightAxis) == 0)
        {
            InputDirection = new Vector2(0, InputDirection.y);
            
            if(animatorPlayer.GetFloat("BlendHorizontal") != 0)
            {
                animatorPlayer.SetFloat("BlendHorizontal", 0f);
            }
        }
    }
    //---Player Movement
    void PlayerMovement()
    {
        Direction = ragdollPlayer.transform.rotation * new Vector3(Input.GetAxisRaw(leftRightAxis), 0.0f, Input.GetAxisRaw(forwardBackAxis));
        Direction.y = 0f;
        physicsBody.velocity = Vector3.Lerp(physicsBody.velocity, (Direction * moveSpeed) + new Vector3(0, physicsBody.velocity.y, 0), 0.8f);
    }
    
    //---Player Movement Air
    void PlayerMovementAir()
    {
        Direction = ragdollPlayer.transform.rotation * new Vector3(Input.GetAxisRaw(leftRightAxis), 0.0f, Input.GetAxisRaw(forwardBackAxis));
        Direction.y = 0f;
        physicsBody.AddForce(Direction * (moveSpeed * 50));
    }
    
    //---Player Speed
    void PlayerSpeed()
    {
        //walk forward speed
        if(InputDirection == new Vector2(0,1))
        {
            moveSpeed = cachedMoveSpeed;
        }
        
        //walk backward speed
        else if(InputDirection == new Vector2(0,-1))
        {
            moveSpeed = cachedMoveSpeed / 1.5f;
        }
        
        //walk diagonal speed
        else if(InputDirection != new Vector2(0,1) && InputDirection != new Vector2(0,-1))
        {
            moveSpeed = cachedMoveSpeed / 1.7f;
        }
    }
    //---Player Rotation
    void PlayerRotation()
    {
        var lookPos = mainCamera.transform.forward;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        physicsJoint.targetRotation = Quaternion.Slerp(physicsJoint.targetRotation, Quaternion.Inverse(rotation), Time.deltaTime * turnSpeed);
    }
    //---Player Jump
    void PlayerJump()
    {
        if(Input.GetAxis(jump) > 0)
        {
            if(!jumpAxisUsed)
            {
                if(!inAir)
                {
                    jumping = true;
                    physicsJoint.slerpDrive = DriveLow;
                }
                
                else if(inAir)
                {
                    if(HandControllerRight.GrabbedObject != null && HandControllerRight.GrabbedObject.isKinematic)
                    {
                        jumping = true;
                        physicsJoint.slerpDrive = DriveLow;
                    }
                    
                    else if(HandControllerLeft.GrabbedObject != null && HandControllerLeft.GrabbedObject.isKinematic)
                    {
                        jumping = true;
                        physicsJoint.slerpDrive = DriveLow;
                    }
                }
            }

            jumpAxisUsed = true;
        }
        
        else
        {
            jumpAxisUsed = false;
        }
    }
    //---Player Jumping
    void PlayerJumping()
    {
        if(jumping && !inAir)
        {
            isJumping = true;
                
            var v3 = physicsBody.transform.up * jumpForce;
            v3.x = physicsBody.velocity.x;
            v3.z = physicsBody.velocity.z;
            physicsBody.velocity = v3;
        }
        
        else if(jumping && inAir)
        {
            isJumping = true;
                
            var v3up = physicsBody.transform.up * jumpForce;
            v3up.x = physicsBody.velocity.x;
            v3up.z = physicsBody.velocity.z;
            physicsBody.velocity = v3up;
            
            var v3forward = mainCamera.transform.forward * jumpForce;
            physicsBody.AddForce(v3forward * jumpForce * 200);
        }

		if (isJumping)
		{
			timer = timer + Time.fixedDeltaTime;
				
			if (timer > 0.2f)
			{
				timer = 0.0f;
				jumping = false;
				isJumping = false;
                inAir = true;
			}
		}
    }
    //---Player Animations
    void PlayerAnimations()
    {   
        if(useControls && !ragdollMode)
        {
            //mouse Y axis recording
            if(Input.GetAxisRaw("Mouse Y") > 0)
            {
                mouseYaxis = 0.5f;
            }

            else if(Input.GetAxisRaw("Mouse Y") < 0)
            {
                mouseYaxis = 0.5f;
            }
            //Look Height
            animatorPlayer.SetFloat("BlendLook", mouseYaxis);


            //Reach Left
            if(Input.GetAxisRaw("Fire1") != 0)
            {
                if(animatorPlayer.GetLayerWeight(3) != 1)
                {
                    animatorPlayer.SetLayerWeight(3, 1f);
                    animatorPlayer.Play("ReachLeftArm", -1, 0f);
                }

                animatorPlayer.SetFloat("BlendReachLeft", mouseYaxis);
            }

            if(Input.GetAxisRaw("Fire1") == 0)
            {
                if(animatorPlayer.GetLayerWeight(3) > 0)
                {
                    animatorPlayer.SetLayerWeight(3, 0f);
                    animatorPlayer.SetFloat("BlendReachLeft", 0.5f);
                }
            }


            //Reach Right
            if(Input.GetAxisRaw("Fire2") != 0)
            {
                if(animatorPlayer.GetLayerWeight(2) != 1)
                {
                    animatorPlayer.SetLayerWeight(2, 1f);
                    animatorPlayer.Play("ReachRightArm", -1, 0f);
                }

                animatorPlayer.SetFloat("BlendReachRight", mouseYaxis);
            }

            if(Input.GetAxisRaw("Fire2") == 0)
            {
                if(animatorPlayer.GetLayerWeight(2) > 0)
                {
                    animatorPlayer.SetLayerWeight(2, 0f);
                    animatorPlayer.SetFloat("BlendReachRight", 0.5f);
                }
            }


            //Move Direction
            if(!isPlaying(animatorPlayer, "DirectionMotion") && !jumping && !inAir && !ragdollMode)
            {
                animatorPlayer.Play("DirectionMotion");
            }

            //In Air
            if(!jumping && !isPlaying(animatorPlayer, "Jump") && !isPlaying(animatorPlayer, "Air") && inAir && !ragdollMode && !ragdollPlayer.GetComponent<Rigidbody>().isKinematic)
            {
                animatorPlayer.Play("Air");
            }
            
            //Jump
            if(jumping && !isPlaying(animatorPlayer, "Jump") && !isPlaying(animatorPlayer, "Air") && !inAir && !ragdollMode)
            {
                animatorPlayer.Play("Jump");
            }
        }
        
        //Getup
        if(!jumping && !isPlaying(animatorPlayer, "Air") && !isPlaying(animatorPlayer, "Getup") && !inAir && ragdollMode && !cooledDown)
        {
            animatorPlayer.Play("Getup");
        }
    }
    //Keep track of playing animations
    bool isPlaying(Animator anim, string stateName)
    {
        if (anim.GetCurrentAnimatorStateInfo(animLayer).IsName(stateName) && anim.GetCurrentAnimatorStateInfo(animLayer).normalizedTime < 1.0f)
        {
            return true;
        }
        
        else
        {
            return false;
        }
    }
    //---KnockDown
    public void KnockDown()
    {
        physicsJoint.slerpDrive = DriveOff;
        ActivateRagdoll();
        
        ragdollMode = true;
        inAir = true;
        cooledDown = false;
    } 
    //---Activate Ragdoll
    void ActivateRagdoll()
    {
        foreach (ConfigurableJoint part in ragdollParts)
        {
           part.slerpDrive = DriveOff;  
        }
    }
    //---Deactivate Ragdoll
    void DeactivateRagdoll()
    {
        foreach (ConfigurableJoint part in ragdollParts)
        {
           part.slerpDrive = DriveOnRagdoll;  
        }
    }
}