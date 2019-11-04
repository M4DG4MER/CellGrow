using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public List<IReseteable> Spawn;

    public int count;
    public Vector2 Area;
    public float High = 1.24f;
    private Transform mtransform;
    private List<IReseteable> spawned = new List<IReseteable>();

    private float DeltaRespawn = 0f;
    public float TimeToRespawn = 30f;
    public bool RespawnActivated = false;


    private void Awake()
    {
        mtransform = transform;
        
    }

    private void FixedUpdate()
    {
        if (RespawnActivated && DeltaRespawn >= TimeToRespawn)
        {
            DeltaRespawn = 0;
            ResetCell(spawned.First(r => !r.gameObject.activeSelf));
        }
        DeltaRespawn += Time.deltaTime;
    }


    public void ResetAll()
    {
        foreach (var sp in spawned)
            DestroyImmediate(sp.gameObject);
        spawned.Clear();

        for (int i = 0; i < count; i++)
        {
            IReseteable prefab = Spawn[Random.Range(0, Spawn.Count)];

            IReseteable instanced = Instantiate(prefab, transform);

            spawned.Add(instanced);

            ResetCell(instanced);
        }
    }

    private void ResetCell(IReseteable c)
    {
        c.ResetThis(RandomPos());
    }


    Vector3 RandomPos()
    {
        return new Vector3(Random.Range(-Area.x, Area.x), High, Random.Range(-Area.y, Area.y));
    }

}
