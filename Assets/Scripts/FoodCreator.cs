using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodCreator : IReseteable
{
    public GameObject Prefab;

    public float MaxFoodCount;
    public float FoodCount;
    public float Rate;
    private float delta;

    public Vector2 Area;


    List<GameObject> foods = new List<GameObject>();

    private void Awake()
    {
        CreateFood();
    }

    private void FixedUpdate()
    {
        delta += Time.deltaTime;

        if (delta >= Rate)
        {
            delta = 0;
            EnableFood();
        }
    }

    public void CreateFood()
    {
        for (int i = 0; i < MaxFoodCount; i++)
        {
            var f = Instantiate(Prefab, this.transform);
            foods.Add(f.gameObject);

            ResetPos(f.transform, this.transform.position);
        }
    }

    public void EnableFood()
    {
        for (int i = 0, a = 0; i < MaxFoodCount && a < FoodCount; i++)
        {
            var f = foods[i];
            if (!f.activeSelf)
            {
                ResetPos(f.transform, this.transform.position);
                f.SetActive(true);
                a++;
            }
        }
    }

    void ResetPos(Transform t, Vector3 p)
    {
        t.position = p + new Vector3(
               Random.Range(-Area.x, Area.x),
               0,
               Random.Range(-Area.y, Area.y));
    }


    public override void ResetThis(Vector3 initial)
    {
        ResetPos(transform, initial);
        EnableFood();
    }
}
