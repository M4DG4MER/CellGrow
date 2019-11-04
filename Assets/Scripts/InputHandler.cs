using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class InputHandler : MonoBehaviour
{
    public Vector3 Target;
    private Transform mtransform;
    private float lastDistance;
    CellControl cell;
    private Animator anim;
    private List<CellShooter> cellShooters;
    private Dictionary<string, float> PressedAxis = new Dictionary<string, float>();

    public bool isAI;
    public void SetAi(bool state)
    {
        Debug.Log(state);
        anim.enabled = isAI = state;
    }

    private void OnEnable()
    {
        mtransform = transform;
        cell = GetComponent<CellControl>();
        anim = GetComponent<Animator>();
        cellShooters = GetComponents<CellShooter>().ToList();
        cell.inputHandler = this;
        cellShooters.ForEach(s => s.inputHandler = this);
        GetComponent<Animator>().enabled = isAI;
        lastDistance = float.MaxValue;
    }

    internal void SetAxis(string attackAxis, float val = 1f)
    {
        if (!PressedAxis.ContainsKey(attackAxis))
            PressedAxis.Add(attackAxis, val);
        else
            PressedAxis[attackAxis] = val;
    }
    

    public float GetAxis(string axis)
    {
        return (isAI ? AiAxis(axis) : PlayerAxis(axis));
    }

    public float AiAxis(string axis)
    {
        switch (axis)
        {
            case "Horizontal": return AiHorizontal();
            case "Vertical": return AiVertical();
            case "Mouse X": return AiMouse();
            default:
                if (PressedAxis.ContainsKey(axis))
                    return PressedAxis[axis];
                else
                    return 0f;
        }
    }

    public float PlayerAxis(string axis)
    {
        if (EventSystem.current.IsPointerOverGameObject()) return 0f;
        if (axis == "Mutate") return Input.GetKeyDown(KeyCode.M) ? 1f : 0f;

        float val = Input.GetAxis(axis);
        logger.Log($"{axis} : {val}");
        return val;
    }



    float AiMouse()
    {
        var targetRotation = Quaternion.LookRotation(Target - mtransform.position);
        var targetAngle = targetRotation.eulerAngles.y;
        var currentAngle = mtransform.eulerAngles.y;
        var upAngleSmooth = currentAngle + cell.angleSpeed;
        var downAngleSmooth = currentAngle - cell.angleSpeed;

        if (targetAngle > upAngleSmooth)
        {
            if (targetAngle - currentAngle > 180.0)
            {
                return -1f;
            }
            else if (targetAngle - currentAngle < 180.0)
            {
                return 1f;
            }

        }
        else if (targetAngle < downAngleSmooth)
        {
            if (currentAngle - targetAngle > 180.0)
            {
                return 1f;
            }
            else if (currentAngle - targetAngle < 180.0)
            {
                return -1f;
            }
        }

        return 0f;
    }
    float AiVertical()
    {
        float d = Vector3.Distance(Target, mtransform.position);

        if (d < lastDistance)
            return 1f;
        if (d > lastDistance)
            return -2f;

        lastDistance = d;
        return 0f;
    }
    float AiHorizontal()
    {
        Vector3 dir = Target - mtransform.position;
        

        return 0f;// IsLeft(mtransform.position, Target) ? -.5f : .5f;
    }

    bool IsLeft(Vector3 A, Vector3 B)
    {
        return -A.x * B.z + A.z * B.x < 0;
    }

    public bool DebugTarget = false;

    private void OnDrawGizmos()
    {
        if (DebugTarget)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(Target, 15);
        }
    }

}
