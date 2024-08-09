using UnityEngine;

[CreateAssetMenu(fileName = "GeneralMonsterData", menuName = "MonsterData/GeneralMonsterData")]
public class GeneralMonsterData : ScriptableObject
{
    public GeneralMonsterDataStruct data;
    
    [Header("General Setting")] 
    public float knockBackPower;
    public float hp;
    //public GeneralMonsterState currentState = GeneralMonsterState.Idle;
    //public FSMState currentState;
    
    [Header("Patrol(Idle) Setting")]
    public float moveSpeed;
    public float patrolDistance;
    public Vector2 moveDirection = Vector2.right;
    public float obstacleRaycastDistance;
    
    [Header("Attack Setting")]
    public LayerMask targetLayer;
    public float recognizeRadius;
    public float attackSpeed;
    public float attackDamage;

    //Sync SO class variable to struct data (for deep copy)
    public void SyncData()
    {
        data.knockBackPower = knockBackPower;
        data.hp = hp;
        //data.currentState = currentState;

        data.moveSpeed = moveSpeed;
        data.patrolDistance = patrolDistance;
        data.patrolPos = Vector2.zero;
        data.moveDirection = moveDirection;
        data.obstacleRaycastDistance = obstacleRaycastDistance;

        data.targetLayer = targetLayer;
        data.targetTransform = null;
        data.recognizeRadius = recognizeRadius;
        data.attackSpeed = attackSpeed;
        data.attackDamage = attackDamage;
    }
}
