using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public Cell Shooter;

    public float damage;
    public DamageType Type;

    Transform tr;

    private void Awake()
    {
        tr = transform;
    }


    private void OnParticleCollision(GameObject other)
    {
        var otherCell = other.gameObject.GetComponent<CellLife>();

        if (otherCell != null && otherCell.cell != Shooter)
        {
            otherCell.HandleDamage(damage, Type);
        }
    }

    internal void Set(Vector3 position, Quaternion rotation, Cell cell, float damage)
    {
        gameObject.SetActive(true);
        tr.position = position;
        tr.rotation = rotation;
        Shooter = cell;
        damage = this.damage;
    }
}
