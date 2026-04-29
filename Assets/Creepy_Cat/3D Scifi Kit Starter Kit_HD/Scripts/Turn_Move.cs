using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn_Move : MonoBehaviour
{

    public bool initialState;
    public GameObject particle;

    public float TurnX;
    public float TurnY;
    public float TurnZ;

    public float MoveX;
    public float MoveY;
    public float MoveZ;

    public bool World;

    private bool state;

    // Use this for initialization
    void Start()
    {
        state = initialState;
        particle.SetActive(state);
    }

    public void TurnOn()
    {
        state = true;
        particle.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!state)
            return;

        if (World == true)
        {
            transform.Rotate(TurnX * Time.deltaTime, TurnY * Time.deltaTime, TurnZ * Time.deltaTime, Space.World);
            transform.Translate(MoveX * Time.deltaTime, MoveY * Time.deltaTime, MoveZ * Time.deltaTime, Space.World);
        }
        else
        {
            transform.Rotate(TurnX * Time.deltaTime, TurnY * Time.deltaTime, TurnZ * Time.deltaTime, Space.Self);
            transform.Translate(MoveX * Time.deltaTime, MoveY * Time.deltaTime, MoveZ * Time.deltaTime, Space.Self);
        }
    }
}
