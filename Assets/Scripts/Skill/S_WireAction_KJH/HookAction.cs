using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class HookAction : MonoBehaviour
{
    public WireAction wireAction;
    public DistanceJoint2D joint2D;
    //private SpringJoint2D joint2D;
    
    
    public Transform enemyTrans;
    public bool isBindToEnemy = false;
    
    public float maxHoldLength = 8f;
    public bool isLerping = false;
    public float lerpDuration = 0.4f;
    
    
    private void Awake()
    {
        joint2D = GetComponent<DistanceJoint2D>();
        //joint2D = GetComponent<SpringJoint2D>();
        
        if (joint2D == null)
        {
            Debug.LogError("joint2D is null");
        }

        joint2D.enabled = false;
    }

    private void FixedUpdate()
    {
        if (isBindToEnemy)
        {
            if (enemyTrans == null)
            {
                isBindToEnemy = false;
                wireAction.isAttached = false;
                wireAction.isWireMax = true;
                joint2D.enabled = false;
                return;
            }
            transform.position = enemyTrans.position;
            //Debug.Log("stuck!");
        }
    }

    IEnumerator SetRopeForce()
    {
        yield return new WaitForSeconds(0.1f);
        if (wireAction.isAttached)
        {
            var RopeForceDir = ((wireAction.playerPos.position.x <= transform.position.x) ? 1 :-1) * 
                               new Vector2(- wireAction.playerPos.position.y + transform.position.y, 
                                   - transform.position.x + wireAction.playerPos.position.x).normalized; 
            //Debug.Log(RopeForceDir);
            wireAction.player.SetForce( 80 * RopeForceDir);
        }
    }
    
    IEnumerator LerpDistance(float targetDistance)
    {
        isLerping = true;
        float timeElapsed = 0f;
        float initialDistance = joint2D.distance;

        while (timeElapsed < lerpDuration)
        {
            joint2D.distance = Mathf.Lerp(initialDistance, targetDistance, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        joint2D.distance = targetDistance; // 최종 거리 설정
        isLerping = false;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 7 || other.gameObject.layer == 12)
        {
            isBindToEnemy = false;
            wireAction.isAttached = true;
            wireAction.isWireMax = true;
            joint2D.enabled = true;
            
            //joint2D.distance = Mathf.Min(Vector2.Distance(transform.position, wireAction.playerPos.position), maxHoldLength);
            float currentDistance = Vector2.Distance(transform.position, wireAction.playerPos.position);
            float newDistance = Mathf.Min(currentDistance, maxHoldLength);

            if (currentDistance > newDistance && !isLerping)
            {
                StartCoroutine(LerpDistance(newDistance));
            }
            else if(currentDistance <= newDistance)
            {
                joint2D.distance = currentDistance;
            }
            
            //Debug.Log(joint2D.distance);
            StartCoroutine(SetRopeForce());
        }

        else if(other.gameObject.layer == 9 || other.gameObject.layer == 4 || other.gameObject.layer == 3)
        {
            isBindToEnemy = false;
            wireAction.isWireMax = true;   
        }
        
        // else if (other.gameObject.layer == 9)
        // {
        //     isBindToEnemy = true;
        //     enemyTrans = other.transform;
        //
        //     wireAction.isAttached = true;
        //     wireAction.isWireMax = true;
        //     joint2D.enabled = true;
        //     joint2D.distance = Vector2.Distance(  wireAction.playerPos.position, other.transform.position);
        // }
    }

    public void DisableJoint2D()
    {
        joint2D.enabled = false;
    }

    public void ShortenJoint2D(float speed)
    {
        joint2D.distance -= speed * Time.deltaTime;
    }

    private void OnEnable()
    {
        transform.position = wireAction.playerPos.position;
    }

    private void OnDisable()
    {
        // var playerRb = wireAction.playerPos.gameObject.GetComponent<Rigidbody2D>();
        // if (playerRb != null)
        // {
        //     playerRb.AddForce(Vector2.up * wireAction.lastJumpSpeed, ForceMode2D.Impulse);
        // }
            
        //transform.position = Vector2.zero;
        enemyTrans = null;
        isBindToEnemy = false;
    }
}
