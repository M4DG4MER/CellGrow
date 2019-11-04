using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stadistics : MonoBehaviour
{
    public bool WinDetection = false;
    public void SetWinDetection()
    {
        WinDetection = false;
    }
    public float WinLevel;

    public StartGame startGame;

    public Cell Observing;
    
    public TMPro.TextMeshProUGUI FoodGUI;
    public TMPro.TextMeshProUGUI LifeGUI;

    public GameObject Editor;
    public Button ExitEditorButton;

    public GameObject DeadMenu;
    public GameObject WinMenu;

    public List<CellEditor> Editors;

    public void SetUp(Cell nextObserving)
    {
        if (Observing != null)
        {
            Observing.onEat -= new CellEvent(ChangeFoodEaten);
            Observing.onLifeChange -= new CellEvent(LifeChange);
            Observing.onMutatePointsChange -= new CellEvent(ChangeFoodEaten);
        }

        Observing = nextObserving;
       
        Observing.onEat += new CellEvent(ChangeFoodEaten);
        Observing.onLifeChange += new CellEvent(LifeChange);
        Observing.onMutatePointsChange += new CellEvent(ChangeFoodEaten);

        ChangeFoodEaten(Observing, 0);
        LifeChange(Observing, Observing.GetComponent<CellLife>().MaxLife);

        Editors.ForEach(e => e.SetUp(Observing));
    }


    public void UpgradeCell()
    {
        Observing.CalculateProgression();
    }


    void ChangeFoodEaten(Cell cell, float value)
    {
        FoodGUI.text = $"{cell.mutatePoints.ToString("0")} / {cell.NextMutatePoints.ToString("0")} ";

        if (!cell.Control.inputHandler.isAI)
        {
            if (WinDetection && WinLevel <= cell.mutatePoints)
            {
                Debug.Log("WTF");
                if (this.gameObject.activeInHierarchy)
                {
                    Debug.Log("WTF2");
                    WinDetection = false;
                    startGame.Pause();
                    WinMenu.gameObject.SetActive(true);
                    return;
                }
            }else
            {
                if ((int)cell.mutatePoints >= (int)cell.NextMutatePoints)
                {
                    startGame.Pause();
                    Editor.SetActive(true);
                    ExitEditorButton.interactable = false;
                }
                else if ((int)cell.mutatePoints <= 0)
                {
                    ExitEditorButton.interactable = true;
                }
            }
        }
        

    }


    void LifeChange(Cell cell, float value)
    {

        LifeGUI.text = $"{cell.Life.Life.ToString("0")} / {cell.Life.MaxLife.ToString("0")} ";

        if (this.gameObject.activeInHierarchy && cell.Life.Life <= 0)
        {
            Cursor.lockState = CursorLockMode.None;
            DeadMenu.gameObject.SetActive(true);
        }
    }
    
   
}