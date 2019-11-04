using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform cellCameraHolder;
    private Transform mtransform;
    private CellControl ccontrol;
    private CameraCellStats stats;

    public Stadistics StatsGUI;

    public float followSpeed = 5;

    public bool AutoSelect = true;
    public bool AutoPlayer = true;

    private void Awake()
    {
        stats = GetComponent<CameraCellStats>();
        mtransform = transform;   
    }

    private void FixedUpdate()
    {
        if (cellCameraHolder != null && cellCameraHolder.gameObject.activeInHierarchy)
        {
            mtransform.position = Vector3.Lerp(mtransform.position, cellCameraHolder.position, followSpeed * Time.deltaTime);
            mtransform.rotation = Quaternion.Lerp(mtransform.rotation, cellCameraHolder.rotation, followSpeed * Time.deltaTime);

            if (!ccontrol.inputHandler.isAI && Input.GetKey(KeyCode.Escape))
            {
                StatsGUI.startGame.MainMenu.SetActive(true);
                StatsGUI.startGame.Pause();
            }
        }
        else
        {
            if (AutoSelect) FindNewCell();
        }
    }


    public void FindNewCell(bool autoplayer = false)
    {
        ccontrol = FindObjectOfType<CellControl>();
        if (ccontrol != null)
        {
            ccontrol.cell.ResetThis(ccontrol.cell.mtransform.position);
            ccontrol.inputHandler.SetAi(!(AutoPlayer = autoplayer));
            cellCameraHolder = ccontrol.CameraHolder;
            ccontrol.cell.onMutate += new CellEvent(stats.RecalculateVision);
            StatsGUI.SetUp(ccontrol.cell);
        }     
    }

    public void DeactivateCellAi()
    {
        if (ccontrol != null)
            ccontrol.inputHandler.SetAi(!(AutoPlayer = false));
    }


}
