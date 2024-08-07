using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class FlyingEyeLaser : MonoBehaviour
{
    private enum State { active, disabled }
    private State currentState;
    
    private LineRenderer lineRenderer;

    public float laserLength = 6f;

    public Material defaultMaterial;
    public Material activeMaterial;

    public int dmg;

    public Vector2 direction;
    
    public float directionX = 0f;
    public float directionY = -1f;

    public GeneralMonsterTest flyingEye;
    
    void Start()
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        

        // 레이저의 시작점과 끝점을 설정
        lineRenderer.positionCount = 2;

        // 레이저의 너비를 설정
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

        lineRenderer.material = defaultMaterial;
    
        // 시작할 때는 레이저 안 나오게
        lineRenderer.enabled = false;
        currentState = State.disabled;
        
        // 레이저의 Z축 위치를 고정
        lineRenderer.sortingOrder = 101;
    }

    void Update()
    {
        if (lineRenderer != null)
        {
            // 레이저의 시작점과 끝점을 설정
            Vector2 startPoint = transform.position;

            // 레이저의 위치를 업데이트
            lineRenderer.SetPosition(0, startPoint);

            if (flyingEye.GeneralMonsterData.targetTransform != null)
            {
                Transform target = flyingEye.GeneralMonsterData.targetTransform;
                direction = (target.position - transform.position).normalized;
            }
            else
            {
                direction = new Vector2(0f, -1f).normalized;
            }

            RaycastHit2D hit = Physics2D.Raycast(startPoint, direction, laserLength, LayerMask.GetMask("Player","Ground","Wall","HookNParkour"));
            
            if (currentState == State.active) // 레이저가 active 상태
            {
                lineRenderer.enabled = true;
                lineRenderer.material = lineRenderer.materials[1];
                
                if (hit.collider != null) // 레이저에 플레이어가 부딪혔을 때
                {
                    lineRenderer.SetPosition(1, hit.point);

                    if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
                    {
                        hit.collider.GetComponent<PlayerController>().GetDamaged(dmg, gameObject, new Vector2(0f, 0f));
                    }
                }
                else
                {
                    lineRenderer.SetPosition(1, startPoint + direction * laserLength);
                }
            }
            else if (currentState == State.disabled)
            {
                lineRenderer.enabled = false;
            }
        }
        else
        {
            Debug.LogError("Laser not active!");
        }
    }

    public void DisabledState()
    {
        // lineRenderer.enabled = false;
        currentState = State.disabled;
    }
    

    public void ActiveState()
    {
        // lineRenderer.enabled = true;
        currentState = State.active;
    }
    
}
