using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraCellStats : MonoBehaviour
{
    public ADNstat VisionDependency;
    public ADNstat VisionDefDependency;
    PostProcessVolume postProcess;
    Vignette vignette;

    private void Awake()
    {
        postProcess = GetComponent<PostProcessVolume>();
        postProcess.profile.TryGetSettings(out vignette);
    }

    public void RecalculateVision(Cell cell, float v)
    {
        if (cell.gameObject.activeSelf)
        {
            vignette.intensity.value = calcCamValue(cell.RecalculateStats(VisionDependency));
            vignette.smoothness.value = calcCamValue(cell.RecalculateStats(VisionDefDependency));
        }
        else
        {
            cell.onMutate -= RecalculateVision;
        }
    }

    float calcCamValue(float v) => 1 - v / 100f;


}
