using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public float followSpeed = 2f;
    public Transform player;
    

    private void FixedUpdate()
    {
        if (player == null) return;

        Vector3 camPos = new Vector3(player.position.x + 3f, player.position.y + 2f, -10f);
        Vector2 mousePos = Input.mousePosition;
        
        // player의 시야 자유도
        if (mousePos.y > player.position.y)
        {
            camPos = new Vector3(player.position.x + 3f, player.position.y + 2.5f, -10f);
        }
        else if (mousePos.y < player.position.y)
        {
            camPos = new Vector3(player.position.x + 3f, player.position.y - 1f, -10f);
        }
        
		Vector3 camofMousePos = camPos;
        
        
        // player가 맵 밖을 보지 못하도록
        // tutorial map 크기를 반영하여 코드 작성
        if (player.position.x + 3f < 0f)
        {
            transform.position = new Vector3(0, player.position.y + 2f, -10f);
        }
        
        if (player.position.x > 316f || transform.position.x > 316f)
        {
            transform.position = new Vector3(316f, camofMousePos.y, -10f);
        }

        transform.position = Vector3.Slerp(transform.position, camPos, followSpeed * Time.deltaTime);
    
        // 볼 수 있는 최소 높이 설정
        // if (transform.position.y < 0) 
        // {
        //     transform.position = new Vector3(player.position.x + 3f, player.position.y + 1f , -10f);
        // }
            
    }
}