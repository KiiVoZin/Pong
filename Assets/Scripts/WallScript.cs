using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallScript : MonoBehaviour
{
    [SerializeField] bool top, down, left, right;

    public List<Vector> GetWalls()
    {
        List<Vector> edges = new List<Vector>();
        Vector2 position = this.transform.position;
        Vector2 scale = this.transform.localScale;
        Vector2 p1;
        Vector2 p2;
        if (top) {
            p1 = new Vector2(position.x - (scale.x / 2), position.y + (scale.y / 2));
            p2 = new Vector2(position.x + (scale.x / 2), position.y + (scale.y / 2));
            edges.Add(new Vector(p1, p2));
        }
        if (down) { 
            p1 = new Vector2(position.x + (scale.x / 2), position.y - (scale.y / 2));
            p2 = new Vector2(position.x - (scale.x / 2), position.y - (scale.y / 2));
            edges.Add(new Vector(p1, p2));
        }
        if (left) { 
            p1 = new Vector2(position.x - (scale.x / 2), position.y - (scale.y / 2));
            p2 = new Vector2(position.x - (scale.x / 2), position.y + (scale.y / 2));
            edges.Add(new Vector(p1, p2));
        }
        if (right) { 
            p1 = new Vector2(position.x + (scale.x / 2), position.y + (scale.y / 2));
            p2 = new Vector2(position.x + (scale.x / 2), position.y - (scale.y / 2));
            edges.Add(new Vector(p1, p2));
        }
        return edges;
    }
}
