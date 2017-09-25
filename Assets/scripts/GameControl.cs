using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour {

    public int maxSize;
    public int currentSize;
    public int xBound;
    public int yBound;
    public int score;
    public GameObject foodPrefab;
    public GameObject currentFood;
    public GameObject snakePrefab;
    public Snake head;
    public Snake tail;
    public int NESW;
    public Vector2 nextPos;

    // Use this for initialization
    void OnEnable()
    {
        Snake.hit += hit;
    }
    void Start()
    {
        InvokeRepeating("TimerInvoke", 0, .5f);
        FoodFunction();
    }
    void OnDisable()
    {
        Snake.hit += hit;
    }

    // Update is called once per frame
    void Update()
    {
        //TimerInvoke();
        comChangeD();
    }

    void TimerInvoke()
    {
        Movement();
        if(currentSize >= maxSize)
        {
            TailFunction();
        }
        else
        {
            currentSize++;
        }
    }

    void Movement()
    {
        GameObject temp;
        nextPos = head.transform.position;
        switch (NESW)
        {
            case 0:
                nextPos = new Vector2(nextPos.x, nextPos.y + 1);
                break;
            case 1:
                nextPos = new Vector2(nextPos.x + 1, nextPos.y);
                break;
            case 2:
                nextPos = new Vector2(nextPos.x, nextPos.y - 1);
                break;
            case 3:
                nextPos = new Vector2(nextPos.x - 1, nextPos.y);
                break;

        }
        temp = (GameObject)Instantiate(snakePrefab, nextPos, transform.rotation);
        head.Setnext(temp.GetComponent<Snake>());
        head = temp.GetComponent<Snake>();

        return; 
    }

    void comChangeD()
    {
        if(NESW != 2 && Input.GetKeyDown(KeyCode.W))
        {
            NESW = 0;
        }
        if (NESW != 3 && Input.GetKeyDown(KeyCode.D))
        {
            NESW = 1;
        }
        if (NESW != 0 && Input.GetKeyDown(KeyCode.S))
        {
            NESW = 2;
        }
        if (NESW != 1 && Input.GetKeyDown(KeyCode.A))
        {
            NESW = 3;
        }

    }

    void TailFunction()
    {
        Snake tempSnake = tail;
        tail = tail.GetNext();
        tempSnake.RemoveTail();
    }

    void FoodFunction()
    {
        int xPos = Random.Range(-xBound, xBound);
        int yPos = Random.Range(-yBound, yBound);

        currentFood = (GameObject)Instantiate(foodPrefab, new Vector2(xPos, yPos), transform.rotation);
        StartCoroutine(CheckRender(currentFood));
    }

    IEnumerator CheckRender(GameObject IN)
    {
        yield return new WaitForEndOfFrame();
        if(IN.GetComponent<Renderer>().isVisible == false)
        {
            if(IN.tag == "Food")
            {
                Destroy(IN);
                FoodFunction();
            }
        }
    }

    void hit(string WhatWasSent)
    {
        if(WhatWasSent == "Food")
        {
            FoodFunction();
            maxSize++;
            score++;
        }
    }
}
