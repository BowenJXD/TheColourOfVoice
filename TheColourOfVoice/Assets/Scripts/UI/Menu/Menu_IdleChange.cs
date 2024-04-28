using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_IdleChange : StateMachineBehaviour
{
    public int staticCount;
    public float minWaitTime = 0f;
    public float maxWaitTime = 2f;

    private float m_Interval;
    private readonly int m_HashRandomIdle = Animator.StringToHash("Idle");
    private int m_NextState;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_Interval = Random.Range(minWaitTime, maxWaitTime);
        m_NextState = Random.Range(0, staticCount);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Normalized Time: " + stateInfo.normalizedTime + ", Interval: " + m_Interval);

        if (stateInfo.normalizedTime>m_Interval && !animator.IsInTransition(0))
        {
            m_NextState = Random.Range(0, staticCount);  // 在需要更新状态前重新计算
            Debug.Log("Triggering state change to: " + m_NextState);
            animator.SetInteger(m_HashRandomIdle, m_NextState);

            m_Interval = stateInfo.normalizedTime + Random.Range(minWaitTime, maxWaitTime);
        }

    }
    

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetInteger(m_HashRandomIdle, -1);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
