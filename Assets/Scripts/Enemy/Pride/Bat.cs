using UnityEngine;

public class Bat : GeneralMonsterTest
{
    [SerializeField] private GameObject sonicWavePrefab;
    
    protected override void StateInit()
    {
        base.StateInit();
        
    }
    protected override bool TransitionCheck()
    {
        return base.TransitionCheck();
    }
    

    protected override void Attack()
    {
        
        if (generalMonsterData.targetTransform.position != null)
        {
            var targetPos = generalMonsterData.targetTransform.position;
            
            Vector2 attackDir = new Vector2(targetPos.x - transform.position.x, targetPos.y - transform.position.y).normalized;
            float angle = Mathf.Atan2(attackDir.y, attackDir.x) * Mathf.Rad2Deg; // 각도 계산
            
            GameObject bullet = Instantiate(sonicWavePrefab, transform.position + transform.forward, Quaternion.Euler(new Vector3(0, 0, angle)));
            bullet.GetComponent<Rigidbody2D>().AddForce(  generalMonsterData. attackSpeed * attackDir * 5 , ForceMode2D.Impulse);
        }
        
        Invoke("Attack", 1f);
    }
}