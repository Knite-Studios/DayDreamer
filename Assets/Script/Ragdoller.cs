using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoller : MonoBehaviour
{
    public GameObject[] PlayerParts;
    public ConfigurableJoint[] JointParts;
    Vector3 COM;
    public float TouchForce, TimeStep, LegsHeight, FallFactor;
    float Step_R_Time, Step_L_Time;
    public bool StepR, StepL, Falling, Fall;
    bool flag, Flag_Leg_R, Flag_Leg_L;
    Quaternion StartLegR1, StartLegR2, StartLegL1, StartLegL2;
    JointDrive Spring0, Spring150, Spring300, Spring320;
    public bool WalkF { get; private set; }
    public bool WalkB { get; private set; }
    private void Awake()
    {
        // Initialization code
        InitializeJoints();
        InitializeJointDrives();
    }
    private void Update()
    {
        CheckWalking();
        Balance();
        if (WalkF)
        {
            StepLogicForWalking(true, false);
        }
        if (WalkB)
        {
            StepLogicForWalking(false, true);
        }
    }
    private void FixedUpdate()
    {      
        LegsMoving();
    }
    void InitializeJoints()
    {
        Physics.IgnoreCollision(PlayerParts[2].GetComponent<Collider>(), PlayerParts[4].GetComponent<Collider>(), true);
        Physics.IgnoreCollision(PlayerParts[3].GetComponent<Collider>(), PlayerParts[7].GetComponent<Collider>(), true);
        StartLegR1 = PlayerParts[4].GetComponent<ConfigurableJoint>().targetRotation;
        StartLegR2 = PlayerParts[5].GetComponent<ConfigurableJoint>().targetRotation;
        StartLegL1 = PlayerParts[7].GetComponent<ConfigurableJoint>().targetRotation;
        StartLegL2 = PlayerParts[8].GetComponent<ConfigurableJoint>().targetRotation;
    }
    void InitializeJointDrives()
    {
        Spring0 = new JointDrive
        {
            positionSpring = 0,
            positionDamper = 0,
            maximumForce = Mathf.Infinity
        };

        Spring150 = new JointDrive
        {
            positionSpring = 150,
            positionDamper = 0,
            maximumForce = Mathf.Infinity
        };

        Spring300 = new JointDrive
        {
            positionSpring = 300,
            positionDamper = 100,
            maximumForce = Mathf.Infinity
        };

        Spring320 = new JointDrive
        {
            positionSpring = 320,
            positionDamper = 0,
            maximumForce = Mathf.Infinity
        };
    }
    void UpdatePlayerPosition()
    {
        PlayerParts[12].transform.position = Vector3.Lerp(PlayerParts[12].transform.position, PlayerParts[2].transform.position, 2 * Time.unscaledDeltaTime);
        PlayerParts[10].transform.position = COM;
        PlayerParts[11].transform.LookAt(PlayerParts[10].transform.position);
    }
    void CheckWalking()
    {
        if (!WalkF && !WalkB)
        {
            ResetWalkingState();
        }
    }
    void ResetWalkingState()
    {
        StepR = false;
        StepL = false;
        Step_R_Time = 0;
        Step_L_Time = 0;
        Flag_Leg_R = false;
        Flag_Leg_L = false;
        JointParts[0].targetRotation = Quaternion.Lerp(JointParts[0].targetRotation, new Quaternion(-0.1f, JointParts[0].targetRotation.y, JointParts[0].targetRotation.z, JointParts[0].targetRotation.w), 6 * Time.fixedDeltaTime);
    }
    public void SetWalkForward(bool walkForward)
    {
        WalkF = walkForward;
    }
    public void SetWalkBackward(bool walkBackward)
    {
        WalkB = walkBackward;
    }
    void Balance()
    {
        // Falling logic
        if ((PlayerParts[10].transform.position.z > PlayerParts[6].transform.position.z + FallFactor && PlayerParts[10].transform.position.z > PlayerParts[9].transform.position.z + FallFactor) ||
            (PlayerParts[10].transform.position.z < PlayerParts[6].transform.position.z - (FallFactor + 0.2f) && PlayerParts[10].transform.position.z < PlayerParts[9].transform.position.z - (FallFactor + 0.2f)))
        {
            Falling = true;
        }
        else
        {
            Falling = false;
        }

        // Adjust joint drives based on falling state
        if (Falling)
        {
            JointParts[1].angularXDrive = Spring0;
            JointParts[1].angularYZDrive = Spring0;
            LegsHeight = 5;
        }
        else
        {
            JointParts[1].angularXDrive = Spring300;
            JointParts[1].angularYZDrive = Spring300;
            LegsHeight = 1;
            AdjustLegsForStability();
        }

        // Check if the character has fallen
        if (PlayerParts[0].transform.position.y - 0.1f <= PlayerParts[1].transform.position.y)
        {
            Fall = true;
        }
        else
        {
            Fall = false;
        }

        // Initiate standing up process if fallen
        if (Fall)
        {
            JointParts[1].angularXDrive = Spring0;
            JointParts[1].angularYZDrive = Spring0;
            StandUp();
        }
    }
    void AdjustLegsForStability()
    {
        JointParts[2].targetRotation = Quaternion.Lerp(JointParts[2].targetRotation, new Quaternion(0, JointParts[2].targetRotation.y, JointParts[2].targetRotation.z, JointParts[2].targetRotation.w), 6 * Time.fixedDeltaTime);
        JointParts[3].targetRotation = Quaternion.Lerp(JointParts[3].targetRotation, new Quaternion(0, JointParts[3].targetRotation.y, JointParts[3].targetRotation.z, JointParts[3].targetRotation.w), 6 * Time.fixedDeltaTime);
        JointParts[2].angularXDrive = Spring0;
        JointParts[2].angularYZDrive = Spring150;
        JointParts[3].angularXDrive = Spring0;
        JointParts[3].angularYZDrive = Spring150;
    }
    public void MoveBackward()
    {
        PlayerParts[0].GetComponent<Rigidbody>().AddForce(Vector3.forward * TouchForce, ForceMode.Impulse);
    }

    public void MoveForward()
    {
        PlayerParts[0].GetComponent<Rigidbody>().AddForce(Vector3.back * TouchForce, ForceMode.Impulse);
    }

    public void ReloadLevel()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    public void ToggleTimeScale()
    {
        Time.timeScale = Time.timeScale == 1 ? 0.4f : 1;
    }
    void LegsMoving()
    {
        // Logic for moving legs while walking forward or backward
        StepLogicForWalking(WalkF, WalkB);

        // Resetting leg rotations when not stepping
        if (!StepR)
        {
            ResetLegRotation(JointParts[4], StartLegR1, 8f);
            ResetLegRotation(JointParts[5], StartLegR2, 17f);
        }
        if (!StepL)
        {
            ResetLegRotation(JointParts[7], StartLegL1, 8);
            ResetLegRotation(JointParts[8], StartLegL2, 17);
        }
    }
    void StepLogicForWalking(bool walkingForward, bool walkingBackward)
    {
        if (walkingForward)
        {
            // Logic for stepping while walking forward
            if (PlayerParts[6].transform.position.z < PlayerParts[9].transform.position.z && !StepL && !Flag_Leg_R)
            {
                StepR = true;
                Flag_Leg_R = true;
                Flag_Leg_L = true;
            }
            if (PlayerParts[6].transform.position.z > PlayerParts[9].transform.position.z && !StepR && !Flag_Leg_L)
            {
                StepL = true;
                Flag_Leg_L = true;
                Flag_Leg_R = true;
            }
        }

        if (walkingBackward)
        {
            // Logic for stepping while walking backward
            if (PlayerParts[6].transform.position.z > PlayerParts[9].transform.position.z && !StepL && !Flag_Leg_R)
            {
                StepR = true;
                Flag_Leg_R = true;
                Flag_Leg_L = true;
            }
            if (PlayerParts[6].transform.position.z < PlayerParts[9].transform.position.z && !StepR && !Flag_Leg_L)
            {
                StepL = true;
                Flag_Leg_L = true;
                Flag_Leg_R = true;
            }
        }
        UpdateStepTiming();
    }
    void UpdateStepTiming()
    {
        // Update timing and reset flags for right step
        if (StepR)
        {
            Step_R_Time += Time.fixedDeltaTime;
            if (Step_R_Time > TimeStep)
            {
                Step_R_Time = 0;
                StepR = false;
                Flag_Leg_R = false;
                if (WalkF || WalkB)
                {
                    StepL = true;
                }
            }
        }

        // Update timing and reset flags for left step
        if (StepL)
        {
            Step_L_Time += Time.fixedDeltaTime;
            if (Step_L_Time > TimeStep)
            {
                Step_L_Time = 0;
                StepL = false;
                Flag_Leg_L = false;
                if (WalkF || WalkB)
                {
                    StepR = true;
                }
            }
        }
    }
    void ResetLegRotation(ConfigurableJoint joint, Quaternion startRotation, float lerpRate)
    {
        joint.targetRotation = Quaternion.Lerp(joint.targetRotation, startRotation, lerpRate * Time.fixedDeltaTime);
    }
    void StandUp()
    {
        if (WalkF)
        {
            JointParts[2].angularXDrive = Spring320;
            JointParts[2].angularYZDrive = Spring320;
            JointParts[3].angularXDrive = Spring320;
            JointParts[3].angularYZDrive = Spring320;
            JointParts[0].targetRotation = Quaternion.Lerp(JointParts[0].targetRotation, new Quaternion(-0.1f, JointParts[0].targetRotation.y, 
                JointParts[0].targetRotation.z, JointParts[0].targetRotation.w), 6 * Time.fixedDeltaTime);

            if (JointParts[2].targetRotation.x < 1.7f)
            {
                JointParts[2].targetRotation = new Quaternion(JointParts[2].targetRotation.x + 0.07f, JointParts[2].targetRotation.y, 
                    JointParts[2].targetRotation.z, JointParts[2].targetRotation.w);
            }

            if (JointParts[3].targetRotation.x < 1.7f)
            {
                JointParts[3].targetRotation = new Quaternion(JointParts[3].targetRotation.x + 0.07f, JointParts[3].targetRotation.y, 
                    JointParts[3].targetRotation.z, JointParts[3].targetRotation.w);
            }
        }

        if (WalkB)
        {
            JointParts[2].angularXDrive = Spring320;
            JointParts[2].angularYZDrive = Spring320;
            JointParts[3].angularXDrive = Spring320;
            JointParts[3].angularYZDrive = Spring320;

            if (JointParts[2].targetRotation.x > -1.7f)
            {
                JointParts[2].targetRotation = new Quaternion(JointParts[2].targetRotation.x - 0.09f, JointParts[2].targetRotation.y, 
                    JointParts[2].targetRotation.z, JointParts[2].targetRotation.w);
            }

            if (JointParts[3].targetRotation.x > -1.7f)
            {
                JointParts[3].targetRotation = new Quaternion(JointParts[3].targetRotation.x - 0.09f, JointParts[3].targetRotation.y, 
                    JointParts[3].targetRotation.z, JointParts[3].targetRotation.w);
            }
        }
    }
    void Calculate_COM()
    {
        COM = (JointParts[0].GetComponent<Rigidbody>().mass * JointParts[0].transform.position + 
            JointParts[1].GetComponent<Rigidbody>().mass * JointParts[1].transform.position +
            JointParts[2].GetComponent<Rigidbody>().mass * JointParts[2].transform.position +
            JointParts[3].GetComponent<Rigidbody>().mass * JointParts[3].transform.position +
            JointParts[4].GetComponent<Rigidbody>().mass * JointParts[4].transform.position +
            JointParts[5].GetComponent<Rigidbody>().mass * JointParts[5].transform.position +
            JointParts[6].GetComponent<Rigidbody>().mass * JointParts[6].transform.position +
            JointParts[7].GetComponent<Rigidbody>().mass * JointParts[7].transform.position +
            JointParts[8].GetComponent<Rigidbody>().mass * JointParts[8].transform.position +
            JointParts[9].GetComponent<Rigidbody>().mass * JointParts[9].transform.position) /
            (JointParts[0].GetComponent<Rigidbody>().mass + JointParts[1].GetComponent<Rigidbody>().mass +
            JointParts[2].GetComponent<Rigidbody>().mass + JointParts[3].GetComponent<Rigidbody>().mass +
            JointParts[4].GetComponent<Rigidbody>().mass + JointParts[5].GetComponent<Rigidbody>().mass +
            JointParts[6].GetComponent<Rigidbody>().mass + JointParts[7].GetComponent<Rigidbody>().mass +
            JointParts[8].GetComponent<Rigidbody>().mass + JointParts[9].GetComponent<Rigidbody>().mass);
    }
}
