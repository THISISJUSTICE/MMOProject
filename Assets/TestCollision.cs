using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TestCollision : MonoBehaviour
{
    Tilemap tilemap_;
    public TileBase tileBase_;
    private void Awake() {
        tilemap_ = GetComponent<Tilemap>();
    }

    private void Start() {
        tilemap_.SetTile(new Vector3Int(0,0,0), tileBase_);
    }

    private void Update() {
        List<Vector3Int> blocked = new List<Vector3Int>();

        foreach(Vector3Int pos in tilemap_.cellBounds.allPositionsWithin){
            TileBase tile = tilemap_.GetTile(pos);
            if(tile != null){
                blocked.Add(pos);
            }
        }
    }

}
