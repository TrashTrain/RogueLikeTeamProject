using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStageBlocker : MonoBehaviour
{
	public GameObject bossBlock;
	public KingSlimeAI kingActive;
	
	private void OnEnable()
	{
		if (kingActive == null) return;
		kingActive.OnBossSlimeDeath += OpenDoor;
	}

	private void OnDisable()
	{
        if (kingActive == null) return;
        kingActive.OnBossSlimeDeath -= OpenDoor;
	}
	
	public bool isBlocked = false;
	
	public int waitingTime = 2;
	

    private void OnTriggerEnter2D(Collider2D other)
    {
	    if (other.gameObject.layer == 6 && !isBlocked)
	    {
		    isBlocked = true;
		    bossBlock.SetActive(true);
		    Invoke("ActivateKingSlime", waitingTime);
	    }
    }

    public void ActivateKingSlime()
    {
	    kingActive.SetReady();
    }
    
    public void OpenDoor()
    {
	    bossBlock.SetActive(false);
    }
}
