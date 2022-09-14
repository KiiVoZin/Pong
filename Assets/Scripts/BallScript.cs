using System.Collections;
using System.Collections.Generic;
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
        players = FindAllPlayers();
        move = direction * speed * Time.deltaTime;
        //Check if moving that collides with a player
        CollisionPlayers(move, players);
        //Check if moving that collides with a wall
        //CollisionWalls(move, walls);
        transform.Translate(move);
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

    private void CollisionPlayers(Vector2 move ,List<Vector> players)
    {
        Vector2 ballPosNow = transform.position;
        Vector2 ballPosNext = ballPosNow + new Vector2(move.x, move.y);
        Vector2 closestIntersect = move;
        float distance = Vector2.Distance(ballPosNow, ballPosNext);

        foreach (var player in players)
        {
            Vector2 playerP1 = player.p1;
            Vector2 playerP2 = player.p2;
            float denominator = (playerP2.y - playerP1.y) * (ballPosNext.x - ballPosNow.x) - (playerP2.x - playerP1.x) * (ballPosNext.y - ballPosNow.y);

            if (denominator != 0)
            {
                float u_a = ((playerP2.x - playerP1.x) * (ballPosNow.y - playerP1.y) - (playerP2.y - playerP1.y) * (ballPosNow.x - playerP1.x)) / denominator;
                float u_b = ((ballPosNext.x - ballPosNow.x) * (ballPosNow.y - playerP1.y) - (ballPosNext.y - ballPosNow.y) * (ballPosNow.x - playerP1.x)) / denominator;

                if (u_a >= 0 && u_a <= 1 && u_b >= 0 && u_b <= 1)
                {
                    float x = ballPosNow.x + u_a * (ballPosNext.x - ballPosNow.x);
                    float y = ballPosNow.y + u_a * (ballPosNext.y - ballPosNow.y);

                    Vector2 intersection = new Vector2(x, y);
                    float distanceTemp = Vector2.Distance(intersection, ballPosNow);
                    if(distanceTemp < distance)
                    {
                        closestIntersect = 99*intersection/100;
                        distance = distanceTemp;
                    }
                    direction = new Vector2(-direction.x, direction.y);
                    Debug.Log("AAA");
                }
            }
        }
        this.move = closestIntersect - ballPosNow;
    }

    private void CollisionWalls(Vector2 move, List<Vector> walls)
    {
        Vector2 ballPosNow = transform.position;
        Vector2 ballPosNext = ballPosNow + new Vector2(move.x, move.y);
        float distance = Vector2.Distance(ballPosNow, ballPosNext);
        Vector2 closestIntersect = move;

        foreach (var wall in walls)
        {
            Vector2 wallP1 = wall.p1;
            Vector2 wallP2 = wall.p2;
            float denominator = (wallP2.y - wallP1.y) * (ballPosNext.x - ballPosNow.x) - (wallP2.x - wallP1.x) * (ballPosNext.y - ballPosNow.y);

            if (denominator != 0)
            {
                float u_a = ((wallP2.x - wallP1.x) * (ballPosNow.y - wallP1.y) - (wallP2.y - wallP1.y) * (ballPosNow.x - wallP1.x)) / denominator;
                float u_b = ((ballPosNext.x - ballPosNow.x) * (ballPosNow.y - wallP1.y) - (ballPosNext.y - ballPosNow.y) * (ballPosNow.x - wallP1.x)) / denominator;

                if (u_a >= 0 && u_a <= 1 && u_b >= 0 && u_b <= 1)
                {
                    float x = ballPosNow.x + u_a * (ballPosNext.x - ballPosNow.x);
                    float y = ballPosNow.y + u_a * (ballPosNext.y - ballPosNow.y);

                    Vector2 intersection = new Vector2(x, y);
                    float distanceTemp = Vector2.Distance(intersection, ballPosNow);
                    if (distanceTemp < distance)
                    {
                        closestIntersect = 95 * intersection / 100;
                    }
                    direction = new Vector2(direction.x, -direction.y);
                    Debug.Log("BBB");
                }
            }
        }
        this.move = closestIntersect - ballPosNow;
    }

    //private void IntersectTest()
    //{
    //    Vector2 player1 = new Vector2(-5, 0);
    //    Vector2 player2 = new Vector2(5, 0);
    //    Vector2 player2 = new Vector2(5, 0);
    //}

    private void ThrowBall()
    {
        transform.position = Vector2.zero;
        int temp = Random.Range(0, 2);
        float angle = Random.Range(30, 45);
        if (temp == 0)
        {
            direction = new Vector2(-1, angle/360).normalized;
        }
        else
        {
            direction = new Vector2(1, angle/360).normalized;
        }
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
