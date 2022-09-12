using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    [SerializeField] float playerSpeed;
    Vector2 direction;
    [SerializeField] float ballSpeed;
    [SerializeField] Camera cam;
    [SerializeField] GameObject p1;
    [SerializeField] GameObject p2;
    [SerializeField] GameObject leftWall;
    [SerializeField] GameObject rightWall;
    [SerializeField] GameObject upWall;
    [SerializeField] GameObject downWall;
    // Start is called before the first frame update
    void Start()
    {
        float angle = Random.Range(30, 60);
        int temp = Random.Range(0, 2);
        if(temp == 0){
            direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
        }else{
            direction = new Vector2(-Mathf.Cos(angle), -Mathf.Sin(angle)).normalized;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.W) && p1.transform.position.x + p1.transform.localScale.x <= upWall.transform.position.y - upWall.transform.localScale.y/2){
            p1.transform.Translate(new Vector2(playerSpeed, 0));
        }



        transform.Translate(direction * ballSpeed * Time.deltaTime);
        Bounce();
    }

    public float[] GetBoundaries(){
        //Top, Down, Left, Right
        float[] boundaries = new float[4];
        boundaries[0] = this.transform.position.y + (this.transform.localScale.y/2);
        boundaries[1] = this.transform.position.y - (this.transform.localScale.y/2);
        boundaries[2] = this.transform.position.x - (this.transform.localScale.x/2);
        boundaries[3] = this.transform.position.x + (this.transform.localScale.x/2);
        return boundaries;
    }

    public void Bounce(){
        if(transform.position.y + transform.localScale.y/2 >= upWall.transform.position.y - upWall.transform.localScale.y/2 && transform.position.x + transform.localScale.x/2 >= upWall.transform.position.x - upWall.transform.localScale.x/2 && transform.position.x + transform.localScale.x/2 <= upWall.transform.position.x + upWall.transform.localScale.x/2){
            direction = new Vector2(direction.x, -direction.y);
        }else if(transform.position.y - transform.localScale.y/2 <= downWall.transform.position.y + downWall.transform.localScale.y/2 && transform.position.x + transform.localScale.x/2 >= downWall.transform.position.x - downWall.transform.localScale.x/2 && transform.position.x + transform.localScale.x/2 <= downWall.transform.position.x + downWall.transform.localScale.x/2){
            direction = new Vector2(direction.x, -direction.y);
        }
    }
}
