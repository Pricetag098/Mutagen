using DG.Tweening;

using UnityEngine;

using UnityEngine.InputSystem;


public class PauseMenu : MonoBehaviour
{
    [SerializeField] InputActionProperty pauseAction;
    public SettingsMenu settingsMenu;

    public RectTransform menu, circle;
    public CanvasGroup canvasGroup, bgCanvasGroup;
    public RingSpinner ringSpinner;
    Vector2 circlePoint;

    public float speed;

    bool open;

    private void Start()
    {
		PlayerSettingsHandler.instance.ReloadTargets();
		pauseAction.action.performed += Pause;
        circlePoint = circle.anchoredPosition;
        open = true;
        Close();
        DOTween.Kill(this, true);
    }

    private void Awake()
    {
        
    }

    private void OnEnable()
    {
        pauseAction.action.Enable();
        
    }

    private void OnDisable()
    {
        pauseAction.action.Disable();
    }

	private void OnDestroy()
	{
		pauseAction.action.performed -= Pause;
	}

	void Pause(InputAction.CallbackContext context)
    {
        
        
        //close
        if (open)
        {
            
            Close();
        }
        //open
        else
        {
            
            
            Open();
            
        }
    }
    public void Open()
    {
        if (open)
            return;
        open=true;
        DOTween.Kill(this, true);
        PlayerSettingsHandler.instance.LoadGame();
        Time.timeScale = 0;
        ringSpinner.rotationSpeed = 5;
        menu.localScale = Vector3.zero;
        menu.anchoredPosition = circlePoint;
        circle.anchoredPosition = circlePoint + new Vector2(-1000, 0);
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        Sequence s = DOTween.Sequence(this);
        s.SetUpdate(true);
        s.Append(circle.DOAnchorPos(circlePoint,speed)).SetEase(Ease.InSine);
        s.Append(menu.DOAnchorPos(Vector2.zero, speed)).SetEase(Ease.InSine);
        s.Join(DOTween.To(() => ringSpinner.rotationSpeed, x => ringSpinner.rotationSpeed = x, 1, speed));
        s.Join(menu.DOScale(Vector3.one, speed)).SetEase(Ease.InSine);
        s.Join(bgCanvasGroup.DOFade(1, speed));
    }

    public void Close()
    {
        if (!open)
            return;
        open = false;
        DOTween.Kill(this, true);
        Time.timeScale = 1;
        PlayerSettingsHandler.instance.SaveGame();
        menu.localScale = Vector3.one;
        menu.anchoredPosition = Vector2.zero;
        circle.anchoredPosition = circlePoint;
        
        Sequence s = DOTween.Sequence(this);
        s.SetUpdate(true);
        s.Append(menu.DOAnchorPos(circlePoint, speed)).SetEase(Ease.OutSine);
        s.Join(menu.DOScale(Vector3.zero, speed)).SetEase(Ease.OutSine);
        s.Join(bgCanvasGroup.DOFade(0, speed));
        s.Join(DOTween.To(() => ringSpinner.rotationSpeed, x => ringSpinner.rotationSpeed = x, 5, speed));
        s.Append(circle.DOAnchorPos(circlePoint + new Vector2(-1000, 0), speed)).SetEase(Ease.OutSine);

        s.AppendCallback(() => {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        });
    }
}
