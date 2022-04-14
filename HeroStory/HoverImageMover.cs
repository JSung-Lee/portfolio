using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HoverImageMover : MonoBehaviour
{
    bool isInit = false;
    public List<Image> moveImages;
    public Vector3 moveDistance;
    List<Vector3> origins = new List<Vector3>();
    public Image fillImage;
    public float fillImageGap = 0f;

    public bool autoRestart = false;
    public float time;
    bool isHoverd = false;
    bool isHoverExit = false;
    public float progress = 0;
    public float scaleRangeStart = 0.9f;
    public float scaleRangeEnd = 0.95f;
    public float resetTime = 5f;
    float timer = 0f;
    void Start()
    {
        isInit = true;
        foreach(var image in moveImages)
        {
            origins.Add(image.transform.position);
        }
    }

    private void Update()
    {
        progress = Mathf.Clamp(progress + (Time.deltaTime / time * (isHoverd ? 1f : -1f)), 0f, 1f);
        for(int i =0;i<moveImages.Count;i++)
        {
            var image = moveImages[i];
            var destination = origins[i] + moveDistance;
            image.transform.position = Vector3.Lerp(origins[i], destination, progress);

            float scaler = Mathf.Clamp((scaleRangeEnd - progress) / (scaleRangeEnd - scaleRangeStart), 0f, 1f);
            image.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, scaler);
        }
        fillImage.fillAmount = progress + fillImageGap;
        if (progress >= 1) timer += Time.deltaTime;
        if (!isHoverExit && autoRestart && timer >= resetTime)
        {
            isHoverd = !isHoverExit;
            timer = progress = 0;
        }
    }

    public void StartMoveImage()
    {
        isHoverExit = false;
        if (isHoverd) return;
        isHoverd = true;
        timer = 0f;
    }

    public void EndMoveImage()
    {
        isHoverExit = true;
    }
    private void OnDisable()
    {
        EndMoveImage();
        progress = 0;
        Update();
    }

    private void OnEnable()
    {
        if (!isInit) Start();
        StartMoveImage();
    }
}
