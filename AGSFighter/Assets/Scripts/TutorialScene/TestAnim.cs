using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAnim : MonoBehaviour
{
    private Animator anim;
    private AnimationState state;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        state = GetComponent<AnimationState>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            state.SetAnimTrue("N_Mid");
        }
    }
}
