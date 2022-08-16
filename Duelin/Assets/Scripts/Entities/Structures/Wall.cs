using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Wall : Entity
{
    private Tilemap walls;

    void Start()
    {
        walls = GetComponent<Tilemap>();
    }

    public override void OnDamage(BaseGoblin attackingEntity, Vector3 targetPos)
    {   
        OnDeath(targetPos: targetPos);
    }

    public override void OnDeath(BaseGoblin attackingEntity = null, Vector3? targetPos = null)
    {
        if (targetPos == null) return;
        Vector3Int wallPos = walls.WorldToCell((Vector3) targetPos);
        walls.SetTile(new Vector3Int(wallPos.x, wallPos.y, 2), null);
    }
}