using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class IKManager : MonoBehaviour
{
    public Transform leftHandTarget;
    public Transform rightHandTarget;
    public Transform leftFootTarget;
    public Transform rightFootTarget;

    private Animator animator;
    private float rightHandWeight;

    private bool grabbing;

    private Coroutine grab=null;
    private Coroutine ungrab=null;

    public Transform centerOfMassTarget;

    

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rightHandWeight = 0;
        grabbing = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!grabbing)
            {
                if (ungrab != null)
                {
                    StopCoroutine(ungrab);
                }
                grab = StartCoroutine(AnimateGrab());
                grabbing = true;
            }
            else
            {
                if(grab!=null)
                {
                    StopCoroutine(grab);
                }
                ungrab = StartCoroutine(AnimateUngrab());
                grabbing = false;
            }
        }
    }

    IEnumerator AnimateGrab()
    {
        while(rightHandWeight<1)
        {
            yield return null;
            rightHandWeight += 0.001f;
        }
    }

    IEnumerator AnimateUngrab()
    {
        while(rightHandWeight>0)
        {
            yield return null;
            rightHandWeight -= 0.001f;
        }
    }

    private void OnAnimatorIK(int layerIndex)
    {

        animator.bodyPosition = centerOfMassTarget.position;

        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, rightHandWeight);
        animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandTarget.position);

        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
        animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandTarget.position);

    }

}
