using UnityEngine;
using UnityEngine.UI;

public class MobileLayout : MonoBehaviour
{
    [SerializeField] private CanvasScaler canvasScaler;
    [SerializeField] private RectTransform scoreRect;
    [SerializeField] private RectTransform missionFailedBannerRect;
    [SerializeField] private RectTransform launchButtonRect;
    [SerializeField] private RectTransform touchHintRect;
    [SerializeField] private Text scoreText;
    [SerializeField] private float scoreTopOffset = 100f;
    [SerializeField] private float bannerTopOffset = 225f;
    [SerializeField] private float launchButtonBottomOffset = 275f;
    [SerializeField] private float touchHintBottomOffset = 460f;

    private Rect lastSafeArea;
    private Vector2 scoreBaseSize;
    private Vector3 scoreBaseScale;
    private Vector2 bannerBaseSize;
    private Vector2 buttonBaseSize;
    private Vector2 hintBaseSize;
    private int scoreBaseFontSize;

    private void Awake()
    {
        CacheBaseValues();
        ApplyCanvasScaleDefaults();
        ApplyLayout();
    }

    private void Update()
    {
        if (Screen.safeArea != lastSafeArea) {
            ApplyLayout();
        }
    }

    private void ApplyCanvasScaleDefaults()
    {
        if (canvasScaler == null) {
            return;
        }

        canvasScaler.referenceResolution = new Vector2(1080f, 1920f);
        canvasScaler.matchWidthOrHeight = 1f;
    }

    private void CacheBaseValues()
    {
        if (scoreRect != null) {
            scoreBaseSize = scoreRect.sizeDelta;
            scoreBaseScale = scoreRect.localScale;
        }

        if (missionFailedBannerRect != null) {
            bannerBaseSize = missionFailedBannerRect.sizeDelta;
        }

        if (launchButtonRect != null) {
            buttonBaseSize = launchButtonRect.sizeDelta;
        }

        if (touchHintRect != null) {
            hintBaseSize = touchHintRect.sizeDelta;
        }

        if (scoreText != null) {
            scoreBaseFontSize = scoreText.fontSize;
        }
    }

    private void ApplyLayout()
    {
        lastSafeArea = Screen.safeArea;

        float canvasUnitsPerPixel = GetCanvasUnitsPerPixel();
        float topInset = (Screen.height - lastSafeArea.yMax) * canvasUnitsPerPixel;
        float bottomInset = lastSafeArea.yMin * canvasUnitsPerPixel;
        float safeWidth = lastSafeArea.width * canvasUnitsPerPixel;
        float widthScale = Mathf.Clamp(safeWidth / 1080f, 0.85f, 1.15f);
        float scoreScale = Mathf.Clamp(widthScale, 0.9f, 1.2f);

        if (scoreRect != null) {
            scoreRect.sizeDelta = scoreBaseSize * widthScale;
            scoreRect.localScale = scoreBaseScale * scoreScale;
            scoreRect.anchoredPosition = new Vector2(0f, -(scoreTopOffset + topInset));
        }

        if (missionFailedBannerRect != null) {
            missionFailedBannerRect.sizeDelta = bannerBaseSize * widthScale;
            missionFailedBannerRect.anchoredPosition = new Vector2(0f, -(bannerTopOffset + topInset));
        }

        if (launchButtonRect != null) {
            launchButtonRect.sizeDelta = buttonBaseSize * widthScale;
            launchButtonRect.anchoredPosition = new Vector2(0f, launchButtonBottomOffset + bottomInset);
        }

        if (touchHintRect != null) {
            touchHintRect.sizeDelta = hintBaseSize * widthScale;
            touchHintRect.anchoredPosition = new Vector2(0f, touchHintBottomOffset + bottomInset);
        }

        if (scoreText != null && scoreBaseFontSize > 0) {
            scoreText.fontSize = Mathf.RoundToInt(scoreBaseFontSize * scoreScale);
        }
    }

    private float GetCanvasUnitsPerPixel()
    {
        RectTransform rectTransform = transform as RectTransform;

        if (rectTransform == null || Screen.height <= 0) {
            return 1f;
        }

        return rectTransform.rect.height / Screen.height;
    }
}
