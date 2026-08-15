using System;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private GameObject crossArrow;
    [SerializeField] private GameObject circleArrow;
    [SerializeField] private GameObject crossYouText;
    [SerializeField] private GameObject circleYouText;

    private void Awake()
    {
        crossArrow.SetActive(false);
        circleArrow.SetActive(false);
        crossYouText.SetActive(false);
        circleYouText.SetActive(false);
    }

    private void Start()
    {
        GameManager.Instance.OnGameStarted += GameManager_OnGameStart;
        GameManager.Instance.OnCurrentPlayerTypeChanged += GameManager_OnCurrentPlayerTypeChanged;
    }

    private void GameManager_OnCurrentPlayerTypeChanged(object sender, EventArgs e)
    {
        UpdateCurrentArrow();
    }

    private void GameManager_OnGameStart(object sender, EventArgs e)
    {
        if(GameManager.Instance.LocalPlayerType == PlayerType.Cross)
        {
            crossYouText.SetActive(true);
        }
        else
        {
            circleYouText.SetActive(true);
        }

        UpdateCurrentArrow();
    }

    private void UpdateCurrentArrow()
    {
        if(GameManager.Instance.CurrentPlayerType == PlayerType.Cross)
        {
            crossArrow.SetActive(true);
            circleArrow.SetActive(false);
        }
        else
        {
            crossArrow.SetActive(false);
            circleArrow.SetActive(true);
        }
    }
}