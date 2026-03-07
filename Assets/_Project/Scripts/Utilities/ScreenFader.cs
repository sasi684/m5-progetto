using System.Collections;
using UnityEngine;

public class ScreenFader : MonoBehaviour
{
    #region Singleton declaration
    private static ScreenFader _instance;
    private static bool _isApplicationQuitting = false;

    public static ScreenFader Instance 
    { 
        get 
        {
            if (_isApplicationQuitting) return null;

            CreateOrGetInstance();
            return _instance; 
        } 
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    private static void CreateOrGetInstance()
    {
        if (_instance == null)
        {
            _instance = FindAnyObjectByType<ScreenFader>(FindObjectsInactive.Include);
            if(_instance == null)
            {
                ScreenFader prefab = Resources.Load<ScreenFader>("Singletons/Screen Fader Canvas");
                _instance = Instantiate(prefab);
            }
            DontDestroyOnLoad(_instance.gameObject);
        }
    }

    private void OnApplicationQuit()
    {
        _isApplicationQuitting = true;
    }

    private void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }
    #endregion

    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private float _fadeTime = 5f;

    public void StartFadeToOpaque(CanvasGroup canvas)
    {
        gameObject.SetActive(true);
        StartCoroutine(FadeCoroutine(0f, 1f, _fadeTime));
    }

    public void StartFadeToTransparent(CanvasGroup canvas)
    {
        gameObject.SetActive(true);
        StartCoroutine(FadeCoroutine(1f, 0f, _fadeTime));
    }

    private IEnumerator FadeCoroutine(float startValue, float endValue, float duration)
    {
        _canvasGroup.alpha = startValue;
        float timer = 0f;
        while (timer < duration)
        {
            yield return null;

            timer += Time.deltaTime;
            _canvasGroup.alpha = Mathf.Lerp(startValue, endValue, timer / duration);
        }
        _canvasGroup.alpha = endValue;

        if(endValue <= Mathf.Epsilon)
        {
            gameObject.SetActive(false);
        }
    }
}
