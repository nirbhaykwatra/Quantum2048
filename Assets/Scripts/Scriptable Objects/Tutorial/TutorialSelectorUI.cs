using UnityEngine;
using UnityEngine.UI;

public class TutorialSelectorUI : MonoBehaviour
{
    private RectTransform _rectTransform;
    private Image _image;
    private Animator _animator;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _image = GetComponent<Image>();
        _animator = GetComponent<Animator>();
    }

    public void ChangePosition(Vector2 position)
    {
        _rectTransform.anchoredPosition = position;
    }

    public void ChangeScale(Vector2 scale)
    {
        _rectTransform.sizeDelta = scale;
    }

    public void SetActive(bool active)
    {
        _image.enabled = active;
    }

    public void PlayAnimation(Animation animation)
    {
        _animator.Play(animation.name);
    }
}
