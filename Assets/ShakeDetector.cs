using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ShakeDetector : MonoBehaviour
{
    [SerializeField] private float _shakeDetectionThreshold;
    [SerializeField] private float _minShakeInterval;

    [SerializeField] private Image debugImage;

    private float _sqrtShakeDetectionThreshold;
    private float _timeSinceLastShake;

    private void Start()
    {
        _sqrtShakeDetectionThreshold = Mathf.Pow(_shakeDetectionThreshold, 2);
    }

    private void Update()
    {
        _sqrtShakeDetectionThreshold = Mathf.Pow(_shakeDetectionThreshold, 2);

        if (Input.acceleration.sqrMagnitude >= _sqrtShakeDetectionThreshold && Time.unscaledTime >= _timeSinceLastShake + _minShakeInterval)
        {
            //Shake
            Debug.Log("Shake");
            debugImage.color = Color.red;
        }
        else
            debugImage.color = Color.green;
    }
}
