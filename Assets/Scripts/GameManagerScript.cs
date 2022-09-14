using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManagerScript : MonoBehaviour
{
    private int p1score;
    private int p2score;
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
    }
    public void p1scored()
    {
        p1score++;
        p1text.text = "" + p1score;
    }

    public void p2scored()
    {
        p2score++;
        p2text.text = "" + p2score;
    }
}
