using UnityEngine;

[System.Serializable]
public struct GeneralMonsterDataStruct
{
    public float knockBackPower;
    public float hp;
    //public GeneralMonsterState currentState;
    
    public float moveSpeed;
    public float patrolDistance;
    public Vector2 patrolPos;
    public Vector2 moveDirection;
    public float obstacleRaycastDistance;

    public LayerMask targetLayer;
    public Transform targetTransform;
    public float recognizeRadius;
    public float attackSpeed;
    public float attackDamage;
}


