using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

public enum TypeOfDirection
{
    Up,
    Down,
    Left,
    Right
}

public class UIFishing : UICanvas
{
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;
    [SerializeField] private List<FishingButton> fishingButtons = new List<FishingButton>();
    [SerializeField] private float timeToStartShowDirection = 2f;
    [SerializeField] private float timeToChangeDirection = 1f;
    private List<TypeOfDirection> typeOfDirections = new List<TypeOfDirection>();
    private CancellationTokenSource cancellationTokenSource;
    private int currentIndex = 0;
    private TypeOfDirection currentTypeOfDirection;
    private bool isButtonDownCorrectly = false;
    private Tween tweenEffect;
    private bool isFirstTime = true;

    public override void Setup()
    {
        base.Setup();
        if (isFirstTime)
        {
            isFirstTime = false;
            foreach (var item in fishingButtons)
            {
                item.buttonCustom.customButtonOnClick += () =>
                {
                    if (item.TypeOfDirection == currentTypeOfDirection)
                    {
                        isButtonDownCorrectly = true;
                    }
                    else
                    {
                        this.CloseDirectly();
                        cancellationTokenSource?.Cancel();
                        UIManager.Instance.OpenUI<UIGamePlay>().PopUpText("You achieved 'cai nit'");
                        var player = GameManager.Instance.Player;
                        player.SetActiveFishingRod(false);
                        player.actionStateMachine.ChangeState(player.actionStateMachine.noActionUpperState);
                        player.stateMachine.ChangeState(player.stateMachine.idleState);
                    }
                };
            }
        }
        GameManager.Instance.Player.SetActiveFishingRod(true);
        typeOfDirections.Clear();
        cancellationTokenSource?.Cancel();
        cancellationTokenSource = new CancellationTokenSource();
        for (int i = 0; i < 10; i++)
        {
            int random = Random.Range(0, 10000);
            random = random % 4;
            switch (random)
            {
                case 0:
                    typeOfDirections.Add(TypeOfDirection.Up);
                    break;
                case 1:
                    typeOfDirections.Add(TypeOfDirection.Down);
                    break;
                case 2:
                    typeOfDirections.Add(TypeOfDirection.Left);
                    break;
                case 3:
                    typeOfDirections.Add(TypeOfDirection.Right);
                    break;
            }
        }

        _ = StartShowDirection();
    }

    public async UniTask StartShowDirection()
    {
        await UniTask.WaitForSeconds(timeToStartShowDirection,cancellationToken: cancellationTokenSource.Token);
        ShowDirection(typeOfDirections[0]);
        currentTypeOfDirection = typeOfDirections[0];
        await UniTask.WaitForSeconds(timeToChangeDirection,cancellationToken: cancellationTokenSource.Token);
        for(int i = 1; i < typeOfDirections.Count; i++)
        {
            if(!isButtonDownCorrectly) break;
            ShowDirection(typeOfDirections[i]);
            isButtonDownCorrectly = false;
            currentTypeOfDirection = typeOfDirections[i];
            await UniTask.WaitForSeconds(timeToChangeDirection,cancellationToken: cancellationTokenSource.Token);
        }
        var player = GameManager.Instance.Player;
        player.actionStateMachine.ChangeState(player.actionStateMachine.noActionUpperState);
        player.stateMachine.ChangeState(player.stateMachine.idleState);
        GameManager.Instance.Player.SetActiveFishingRod(false);
        if(isButtonDownCorrectly)
        {
            this.CloseDirectly();
            
            UIManager.Instance.OpenUI<UIGamePlay>().PopUpText("You achieved tomato");
            GameManager.Instance.Player.AddItemToInventory(1, SaveGameManager.GetDataItem("Tomato").prefab);
            SaveGameManager.Instance.SaveData();
        }
        else
        {
            this.CloseDirectly();
            UIManager.Instance.OpenUI<UIGamePlay>().PopUpText("You achieved 'cai nit'");
        }
    }
    public void ShowDirection(TypeOfDirection type)
    {
        switch (type)
        {
            case TypeOfDirection.Down:
                PopUpText("Down");
                break;
            case TypeOfDirection.Left:
                PopUpText("Left");
                break;
            case TypeOfDirection.Right: 
                PopUpText("Right");
                break;
            case TypeOfDirection.Up: 
                PopUpText("Up");
                break;
            default:
                break;
        }
    }
    public void PopUpText(string popUpContent)
    {
        tweenEffect?.Kill();
        textMeshProUGUI.text = popUpContent;
        textMeshProUGUI.gameObject.SetActive(true);
        Tween tweenFade = textMeshProUGUI.DOFade(0, 3f);
        tweenFade.onComplete += () =>
        {
            textMeshProUGUI.gameObject.SetActive(false);
            textMeshProUGUI.color = new Color(textMeshProUGUI.color.r, textMeshProUGUI.color.g, textMeshProUGUI.color.b, 1);
        };
        tweenFade.onKill += () =>
        {
            textMeshProUGUI.gameObject.SetActive(false);
            textMeshProUGUI.color = new Color(textMeshProUGUI.color.r, textMeshProUGUI.color.g, textMeshProUGUI.color.b, 1);
        };
        float tempY = textMeshProUGUI.transform.localPosition.y;
        Tween tweenMove = textMeshProUGUI.transform.DOLocalMoveY(tempY + 100, 3f);
        tweenMove.onComplete += () =>
        {
            textMeshProUGUI.transform.localPosition = new Vector3(textMeshProUGUI.transform.localPosition.x, tempY, textMeshProUGUI.transform.localPosition.z);
        };
        tweenMove.onKill += () =>
        {
            textMeshProUGUI.transform.localPosition = new Vector3(textMeshProUGUI.transform.localPosition.x, tempY, textMeshProUGUI.transform.localPosition.z);
        };
        tweenEffect = DOTween.Sequence().Append(tweenFade).Join(tweenMove);
    }

    public void CancelFishing()
    {
        var player = GameManager.Instance.Player;
        player.SetActiveFishingRod(false);
        player.actionStateMachine.ChangeState(player.actionStateMachine.noActionUpperState);
        player.stateMachine.ChangeState(player.stateMachine.idleState);
        this.CloseDirectly();
        cancellationTokenSource?.Cancel();
        UIManager.Instance.OpenUI<UIGamePlay>().PopUpText("You achieved 'cai nit'");
    }
}
