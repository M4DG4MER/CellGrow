using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    public CustomSlider MapSlider;
    public CustomSlider CellCountSlider;
    public CustomSlider FoodCountSlider;
    public CustomSlider DiffCountSlider;
    public CustomSlider AudioSlider;

    public Transform Map;
    public Spawner CellSpawner;
    public Spawner FoodSpawner;

    public GameObject ProgressBarMenu;
    public GameObject MainMenu;
    public Image ProgressBar;
    public TMPro.TextMeshProUGUI Text;

    public CameraControl cameraControl;


    public float FunnySentenceTime = 2f;

    public List<string> FunnySentences;


    private void Awake()
    {
        AudioSlider.slider.onValueChanged.AddListener(f => AudioListener.volume = f  / 10f);
        StartCoroutine(InitAsync());
    }

    public void StartNewGame()
    {
        Continue();
        StartCoroutine(StartAsync());
    }

    public void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
    }



    public void Continue()
    {
        Cursor.lockState = CursorLockMode.Locked;
        MainMenu.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }


    public IEnumerator StartAsync()
    {
        MainMenu.SetActive(false);

        var ienu = NewGame();
        while (ienu.MoveNext())
            yield return ienu.Current;

        cameraControl.FindNewCell(true);
        cameraControl.StatsGUI.gameObject.SetActive(true);
    }


    public IEnumerator InitAsync()
    {
        cameraControl.StatsGUI.gameObject.SetActive(false);
        cameraControl.AutoPlayer = false;
        MainMenu.SetActive(false);

        var ienumerator = NewGame();

        while (ienumerator.MoveNext())
            yield return ienumerator.Current;

        MainMenu.SetActive(true);
        MapSlider.slider.value = 2;
        CellCountSlider.slider.value = 1;
        FoodCountSlider.slider.value = 1;
        DiffCountSlider.slider.value = 1;
        AudioSlider.slider.value = 6;
    }

    public IEnumerator NewGame()
    {
        NextProgress();
        ProgressBarMenu.SetActive(true);
        ProgressBar.fillAmount = 0;
        switch (DiffCountSlider.slider.value)
        {
            case 0: cameraControl.StatsGUI.WinLevel = 75f; break;
            case 1: cameraControl.StatsGUI.WinLevel = 150f; break;
            case 2: cameraControl.StatsGUI.WinLevel = 300f; break;
            default: cameraControl.StatsGUI.WinLevel = 75f; break;
        }
        cameraControl.StatsGUI.WinDetection = true;

        float mapSize = MapSlider.slider.value;
        float cellC = CellCountSlider.slider.value;
        float foodC = FoodCountSlider.slider.value;

        yield return new WaitForSeconds(FunnySentenceTime);
        NextProgress();

        Vector3 max = Vector3.one * 21.33333f;
        Vector3 min = Vector3.one * 10f;
        float mapLerp = mapSize / 4f;
        Vector3 targetSize = Vector3.Lerp(min, max, mapLerp);


        yield return new WaitForSeconds(FunnySentenceTime);
        NextProgress();

        Map.localScale = targetSize;

        int cellMin = 25;
        int cellMax = 100;
        float cellLerp = cellC / 4f;
        int targetCell = (int)Mathf.Lerp(cellMin, cellMax, cellLerp);

        yield return new WaitForSeconds(FunnySentenceTime);
        NextProgress();

        int foodMin = 75;
        int foodMax = 200;
        float foodLerp = foodC / 4f;
        int targetFood = (int)Mathf.Lerp(foodMin, foodMax, foodLerp);


        yield return new WaitForSeconds(FunnySentenceTime);
        NextProgress();

        float minArea = 100f;
        float maxArea = 200f;
        float lerpArea = Mathf.Lerp(minArea, maxArea, mapLerp);

        CellSpawner.count = targetCell;
        FoodSpawner.count = targetFood;
        CellSpawner.Area = FoodSpawner.Area = Vector2.one * lerpArea;

        yield return new WaitForSeconds(FunnySentenceTime);
        NextProgress();

        CellSpawner.ResetAll();

        yield return new WaitForSeconds(FunnySentenceTime);
        NextProgress();

        FoodSpawner.ResetAll();
        
        NextProgress(1);

        yield return new WaitForSeconds(FunnySentenceTime);
        ProgressBarMenu.SetActive(false);
        //Debug.Log($"{mapLerp} - {cellLerp} - {foodLerp}");
    }

    public void NextProgress(float f = 0.1f)
    {
        ProgressBar.fillAmount += f;
        Text.text = FunnySentences[UnityEngine.Random.Range(0, FunnySentences.Count)];
    }


    public void Exit()
    {
        Application.Quit();
    }
}
