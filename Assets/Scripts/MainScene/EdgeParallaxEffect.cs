using UnityEngine;

public class SmoothEdgeParallaxEffect : MonoBehaviour
{
    public float parallaxFactor = 0.05f; // 배경이 움직이는 정도
    public float edgeThreshold = 50f; // 가장자리 감지 범위 (픽셀)
    public float maxOffset = 10f; // 배경이 움직일 수 있는 최대 범위 (픽셀)
    public float smoothTime = 0.1f; // 부드러운 이동을 위한 시간 (초)

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private Vector3 velocity = Vector3.zero; // SmoothDamp의 속도를 저장하기 위한 변수

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;

        // 마우스 이동의 속도에 따라 배경의 이동 정도 조정
        float mouseDeltaX = (mousePosition.x - Screen.width / 2) / (Screen.width / 2);
        float mouseDeltaY = (mousePosition.y - Screen.height / 2) / (Screen.height / 2);

        float moveX = Mathf.Clamp(mouseDeltaX, -1, 1);
        float moveY = Mathf.Clamp(mouseDeltaY, -1, 1);

        targetPosition = startPosition + new Vector3(-moveX, -moveY, 0) * parallaxFactor * maxOffset;

        // SmoothDamp를 사용하여 부드러운 이동 구현
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}