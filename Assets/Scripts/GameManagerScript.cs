using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManagerScript : MonoBehaviour
{
    private int p1score;
    private int p2score;
    [SerializeField] GameObject player1;
    [SerializeField] GameObject player2;
    [SerializeField] GameObject ball;
    [SerializeField] TextMeshProUGUI p1text;
    [SerializeField] TextMeshProUGUI p2text;

    // Start is called before the first frame update
    void Start()
    {
        p1score = 0;
        p2score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        player1.GetComponent<PlayerScript>().Move();
        player2.GetComponent<PlayerScript>().Move();
        ball.GetComponent<BallScript>().Move(); 
    }
    public void p1scored()
    {
        p1score++;
        p1text.text = "" + p1score;

        player1.transform.position = new Vector3(player1.transform.position.x, 0, player1.transform.position.z);
        player2.transform.position = new Vector3(player2.transform.position.x, 0, player2.transform.position.z);
    }

    public void p2scored()
    {
        p2score++;
        p2text.text = "" + p2score;

        player1.transform.position = new Vector3(player1.transform.position.x, 0, player1.transform.position.z);
        player2.transform.position = new Vector3(player2.transform.position.x, 0, player2.transform.position.z);
    }
}
