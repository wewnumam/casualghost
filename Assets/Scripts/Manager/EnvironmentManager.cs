using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Cinemachine;

public class EnvironmentManager : MonoBehaviour {
    public static EnvironmentManager Instance { get; private set; }

    [SerializeField] private Light2D globalLight;
    [SerializeField] private Transform environmentParent;
    private float currentDirectionalLightIntensity;
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    private Coroutine zoomRoutine;

    void Awake () {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    public void ResetEnvironment(Level level) {
        ClearEnvironment(environmentParent.gameObject);
        GameObject environment = Instantiate(level.environment, environmentParent);
        Destroy(environment, GameManager.Instance.playTimeInSeconds);
        currentDirectionalLightIntensity = level.directionalLightIntensity;
        globalLight.intensity = level.directionalLightIntensity;
        globalLight.color = level.directionalLightColor;
        if (zoomRoutine != null) StopCoroutine(zoomRoutine);
        zoomRoutine = StartCoroutine(CameraZoom(level.cameraOrthoSize));
    }

    IEnumerator CameraZoom(float targetSize) {
        yield return new WaitForSeconds(2f);
        const float MAX_ORTHOGRAPHIC_SIZE = 50;
        float elapsedTime = 0f;

        while (elapsedTime < 1f) {
            cinemachineVirtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(targetSize, MAX_ORTHOGRAPHIC_SIZE, elapsedTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(2f);
        while (elapsedTime > 0) {
            cinemachineVirtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(targetSize, MAX_ORTHOGRAPHIC_SIZE, elapsedTime);
            elapsedTime -= Time.deltaTime;
            yield return null;
        }
    }

    public void SetGlobalLight() {
        if (GameManager.Instance.IsGameStateGameplay()) {
            globalLight.intensity = currentDirectionalLightIntensity;
        } else {
            // Preventing UI from getting too dark
            globalLight.intensity = 0.8f;
        }
    }

    public void ClearEnvironment(GameObject parentObject) {
        int childCount = parentObject.transform.childCount;
        for (int i = childCount - 1; i >= 0; i--) {
            Destroy(parentObject.transform.GetChild(i).gameObject);
        }
    }
}
