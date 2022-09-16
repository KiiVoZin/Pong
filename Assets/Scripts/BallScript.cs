using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    [SerializeField] float speed, acceleration;
    private Vector2 direction;
    private List<Vector> walls;
    private List<Vector> players;
    private GameObject gameManager;
    private Vector2 move;

    // Start is called before the first frame update
    void Start()
    {
        walls = FindAllWalls();
        players = FindAllPlayers();
        ThrowBall();
        gameManager = GameObject.Find("GameManager");
    }

    // Update is called once per frame
    void Update()
    {
        //players = FindAllPlayers();
        //List<Vector> edges = walls.Concat(players).ToList();
        //foreach (var edge in edges) Debug.Log("" + edge.p2 + edge.p1);
        //move = direction * speed * Time.deltaTime;
        //if (!Collide(edges)) transform.Translate(move);

        //if (transform.position.x <= -10)
        //{
        //    gameManager.GetComponent<GameManagerScript>().p1scored();
        //    ThrowBall();
        //}
        //else if (transform.position.x >= 10)
        //{
        //    gameManager.GetComponent<GameManagerScript>().p2scored();
        //    ThrowBall();
        //}
    }

    public void Move()
    {
        players = FindAllPlayers();
        List<Vector> edges = walls.Concat(players).ToList();
        foreach (var edge in edges) Debug.Log("" + edge.p2 + edge.p1);
        move = direction * speed * Time.deltaTime;
        if (!Collide(edges)) transform.Translate(move);

        if (transform.position.x <= -10)
        {
            gameManager.GetComponent<GameManagerScript>().p1scored();
            ThrowBall();
        }
        else if (transform.position.x >= 10)
        {
            gameManager.GetComponent<GameManagerScript>().p2scored();
            ThrowBall();
        }
    }

    private bool Collide(List<Vector> edges)
    {
        Vector2 ballPosNow = transform.position;
        Vector2 ballPosNext = ballPosNow + move;
        Vector2 closestIntersect = ballPosNext;
        bool flag = false;
        Vector2 edgeVector = new Vector2(0, 0);
        foreach (var edge in edges)
        {
            Vector2 edgeP1 = edge.p1;
            Vector2 edgeP2 = edge.p2;
            float denominator = (edgeP2.y - edgeP1.y) * (ballPosNext.x - ballPosNow.x) - (edgeP2.x - edgeP1.x) * (ballPosNext.y - ballPosNow.y);

            if (denominator == 0) continue;
            float u_a = ((edgeP2.x - edgeP1.x) * (ballPosNow.y - edgeP1.y) - (edgeP2.y - edgeP1.y) * (ballPosNow.x - edgeP1.x)) / denominator;
            float u_b = ((ballPosNext.x - ballPosNow.x) * (ballPosNow.y - edgeP1.y) - (ballPosNext.y - ballPosNow.y) * (ballPosNow.x - edgeP1.x)) / denominator;

            if (!(u_a >= 0 && u_a <= 1 && u_b >= 0 && u_b <= 1)) continue;
            float x = ballPosNow.x + u_a * (ballPosNext.x - ballPosNow.x);
            float y = ballPosNow.y + u_a * (ballPosNext.y - ballPosNow.y);
            Vector2 intersection = new Vector2(x, y);
            float distanceTemp = Vector2.Distance(intersection, ballPosNow);
            float closestTemp = Vector2.Distance(ballPosNow, closestIntersect);
            if (distanceTemp < Vector2.Distance(closestIntersect, ballPosNow))
            {
                flag = true;
                edgeVector = edgeP2 - edgeP1;
                closestIntersect = intersection;
            }

        }
        if (!flag) return false;
        Vector2 normal = new Vector2(edgeVector.y, -edgeVector.x).normalized;
        this.transform.position = closestIntersect - normal * this.transform.localScale.x / 2;
        direction = (direction - 2 * (Vector2.Dot(direction, normal) * normal)).normalized;
        Debug.DrawLine(this.transform.position, closestIntersect, Color.green, 3f);
        MyDebug.DrawCircle(closestIntersect, 0.1f, Color.blue, 3f, 16);
        return true;
    }

    private void ThrowBall()
    {
        transform.position = Vector2.zero;
        int temp = Random.Range(0, 2);
        float angle = Random.Range(45, 60);
        if (temp == 0)
        {
            direction = new Vector2(-1, angle / 360).normalized;
        }
        else
        {
            direction = new Vector2(1, angle / 360).normalized;
        }

        direction = new Vector2(1, 10).normalized;
    }

    private List<Vector> FindAllWalls()
    {
        List<Vector> vectors = new List<Vector>();
        GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
        foreach (var wall in walls)
        {
            WallScript s = wall.GetComponent<WallScript>();
            List<Vector> v = s.GetWalls();
            foreach (Vector v2 in v)
            {
                vectors.Add(v2);
            }
        }
        return vectors;
    }

    private List<Vector> FindAllPlayers()
    {
        List<Vector> vectors = new List<Vector>();
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var player in players)
        {
            WallScript s = player.GetComponent<WallScript>();
            List<Vector> v = s.GetWalls();
            foreach (Vector v2 in v)
            {
                vectors.Add(v2);
            }
        }
        return vectors;
    }
}
