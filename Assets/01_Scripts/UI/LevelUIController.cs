using UnityEngine;
using DG.Tweening;

public class LevelUIController : MonoBehaviour
{
    public GameObject levelEndUI;      // Level bitiş ekranı
    public RectTransform levelEndPanel; // UI paneli

    public void ShowLevelEndUI()
    {
        levelEndUI.SetActive(true);
        levelEndPanel.DOAnchorPos(Vector2.zero, 1f).SetEase(Ease.OutBounce); // Ortaya animasyonlu geçiş
    }

    public void HideLevelEndUI()
    {
        levelEndPanel.DOAnchorPos(new Vector2(0, -1000), 1f).SetEase(Ease.InBack).OnComplete(() =>
        {
            levelEndUI.SetActive(false);
        });
    }
}
