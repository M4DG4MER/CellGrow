using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellControl : MonoBehaviour
{
    internal Cell cell;
    private Transform mtransform;
    internal InputHandler inputHandler;
    internal Rigidbody rg;
    public Transform CameraHolder;

    float changeDirectionDelta = 0;

    internal void ChangeDirection()
    {
        if (inputHandler.isAI && changeDirectionDelta <= 0)
        {
            changeDirectionDelta = 4f;
            inputHandler.Target = cell.mtransform.position - (inputHandler.Target - cell.mtransform.position);
        }
        else
        {
            changeDirectionDelta -= Time.deltaTime;
        }
    }

    public string ForwardAxis;
    public string LateralAxis;
    public string RotationAxis;
    public string MutateAxis;

    public float minForwardSpeed = 100;
    public float minLateralSpeed = 100;
    public float minAngleSpeed = 70;

    internal float forwardSpeed;
    public ADNstat SpeedDependency;
    internal float lateralSpeed;
    public ADNstat LateralSpeedDependency;
    internal float angleSpeed;
    public ADNstat RotationDependency;
    
    private void Awake()
    {
        cell = GetComponent<Cell>();
        rg = GetComponent<Rigidbody>();
        mtransform = transform;
        cell.onMutate += new CellEvent(RecalculateStats);
        cell.Control = this;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        float forward = inputHandler?.GetAxis(ForwardAxis) ?? 0f;
        float lateral = inputHandler?.GetAxis(LateralAxis) ?? 0f;
        float rotation = inputHandler?.GetAxis(RotationAxis) ?? 0f;


        if ((inputHandler?.GetAxis(MutateAxis) ?? 0f) > 0)
            cell.Mutate();

        rg.AddForce(((mtransform.forward * -forward * (minForwardSpeed + forwardSpeed)) + (mtransform.right * -lateral * (minLateralSpeed + lateralSpeed)))  * Time.deltaTime);
        rg.AddRelativeTorque(mtransform.up * rotation * (angleSpeed + minAngleSpeed) * mtransform.lossyScale.magnitude  * Time.deltaTime);
    }





    public void RecalculateStats(Cell cell, float value)
    {
        forwardSpeed = cell.RecalculateStats(SpeedDependency);
        lateralSpeed = cell.RecalculateStats(LateralSpeedDependency);
        angleSpeed = cell.RecalculateStats(RotationDependency);
    }


}
