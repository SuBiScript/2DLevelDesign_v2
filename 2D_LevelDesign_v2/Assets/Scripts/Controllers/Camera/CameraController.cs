using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [HideInInspector] public Transform target;
    [HideInInspector] public bool bossReady;

    private Vector2 velocity;
    [Range(0, 1)]
    public float smoothTime;
    private float posX, posY;
    public SpriteRenderer boss;

    //CameraZoomController
    private bool startingZoom;
    private float orthoSize;
    private float orthoSizeOriginal;
    private float smoothSpeed = 0.03f;
    private bool zoomIn;
    
    //CameraShake
    private float shakeAmount = 0;

    void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        orthoSizeOriginal = Camera.main.orthographicSize;
        orthoSize = boss.bounds.size.x + 0.5f * Screen.height / Screen.width * 0.5f; //zoom in max
        zoomIn = true;
    }

    void Update()
    {

        float posX = Mathf.Round(Mathf.SmoothDamp(transform.position.x, target.position.x, ref velocity.x, smoothTime) * 100) / 100; // limitación de decimales con la multiplicación y división por 100
        float posY = Mathf.Round(Mathf.SmoothDamp(transform.position.y, target.position.y, ref velocity.y, smoothTime) * 100) / 100;

        if (!bossReady)
            transform.position = new Vector3(posX, posY, transform.position.z);

        if (startingZoom)
        {
            ZoomInOut();
        }
    }

    public void Shake(float amount, float leght)
    {
        shakeAmount = amount;
        InvokeRepeating("BeginShake", 0, 0.01f);
        Invoke("StopShake", leght);
    }

    void BeginShake()
    {
        if (shakeAmount > 0)
        {
            Vector3 camPos = transform.position;

            float offsetX = Random.value * shakeAmount * 2 - shakeAmount;
            float offsetY = Random.value * shakeAmount * 2 - shakeAmount;

            camPos.x += offsetX;
            camPos.y += offsetY;

            transform.position = camPos;
        }
    }

    void StopShake()
    {
        CancelInvoke("BeginShake");
    }

    public void Zoom(Transform target) // DoorController call
    {
        bossReady = true;
        startingZoom = true;
        this.target = target;
    }

    void ZoomInOut()
    {

        Vector3 desiredposition = new Vector3(target.position.x, target.position.y, -10);
        Vector3 smoothposition = Vector3.Lerp(transform.position, desiredposition, smoothSpeed);
        base.transform.position = smoothposition;

        if (zoomIn)
        {
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, orthoSize, 0.01f);
            StartCoroutine(ZommInZoomOut());
        }
        else
        {
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, orthoSizeOriginal, 0.03f);
            StartCoroutine(ChangeCameraTarget());
        }
    }

    IEnumerator ZommInZoomOut()
    {
        yield return new WaitForSeconds(4f);
        zoomIn = false;
    }

    IEnumerator ChangeCameraTarget()
    {
        yield return new WaitForSeconds(1f);
        if (GameManager.gameMaster.PlayerController.gameObject.activeSelf)
            target = GameObject.FindGameObjectWithTag("Player").transform;
        bossReady = false;
    }
}
