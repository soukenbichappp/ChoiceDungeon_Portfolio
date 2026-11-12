using Cysharp.Threading.Tasks;
using LitMotion;
using LitMotion.Extensions;
using TMPro;
using UnityEngine;
using System;
using System.Threading;
using System.Collections.Generic;
using UnityEngine.UI;
using Assets.MAIN.Common.Scripts.Domain;

public class FloorView : MonoBehaviour
{
    [SerializeField] private List<GameObject> _enemys;
    [SerializeField] private List<int> _treasureNumbers;
    [SerializeField] private TextMeshProUGUI _floorText;
    [SerializeField] private TextMeshProUGUI _playerText;
    [SerializeField] private GameObject _battleButton;
    [SerializeField] private GameObject _restButton;
    [SerializeField] private GameObject _treasureButton;
    [SerializeField] private GameObject _attackButton;
    [SerializeField] private GameObject _escapeButton;
    [SerializeField] private GameObject _currentEnemyParent;
    [SerializeField] private float _phaseWaitTime;
    [SerializeField] private float _fadeWaitTime;
    [SerializeField] private float _fadeTime;
    [SerializeField] private float _textWaitTime;
    [SerializeField] private float _hideEnemyFadeTime;

    private CancellationToken _textToken;
    private GameObject _currentEnemy;
    private bool _hasHayabusaSword;
    private bool _hasIronShield;
    private bool _hasCursedSword;

    private void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async UniTask ShowFinalFloorText()
    {
        _floorText.gameObject.SetActive(true);
        _floorText.text = $"Final Floor!";

        await FloorTextFadeOut(true);
    }

    public async UniTask ShowFloorText(int currentFloor, int maxFloor)
    {
        _floorText.gameObject.SetActive(true);
        _floorText.text = $"Floor {currentFloor} / {maxFloor}";

        await FloorTextFadeOut(false);
    }
    

    public async UniTask FloorTextFadeOut(bool isFinalFloor)
    {
        CancellationToken cancellationToken = this.GetCancellationTokenOnDestroy();

        _floorText.GetComponent<TextMeshProUGUI>().color = new Color(1, 1, 1, 1);

        await UniTask.Delay(TimeSpan.FromSeconds(_fadeWaitTime), cancellationToken: cancellationToken);

        if (isFinalFloor)
        {
            await LMotion.Create(new Color(1, 1, 1, 1), new Color(1, 1, 1, 0), _fadeTime)
                .WithOnComplete(() => ShowBattlePhaseButton(true))
                .BindToColor(_floorText.GetComponent<TextMeshProUGUI>());
        }
        else
        {
           var motion = LMotion.Create(new Color(1, 1, 1, 1), new Color(1, 1, 1, 0), _fadeTime)
                .WithOnComplete(() => ShowActionButton())
                .BindToColor(_floorText.GetComponent<TextMeshProUGUI>());

            await motion.ToUniTask(cancellationToken: cancellationToken);
        }

        SoundService.Instance.StopSE();
    }

    public void ShowActionButton()
    {
        _battleButton.gameObject.SetActive(true);
        _restButton.gameObject.SetActive(true);
        _treasureButton.gameObject.SetActive(true);
    }

    public void HideActionButton()
    {
        _battleButton.gameObject.SetActive(false);
        _restButton.gameObject.SetActive(false);
        _treasureButton.gameObject.SetActive(false);
    }

    public void ShowBattlePhaseButton(bool IsFinalFloor)
    {
        if (IsFinalFloor)
        {
            _currentEnemy = Instantiate(_enemys[UnityEngine.Random.Range(0, _enemys.Count)], _currentEnemyParent.transform);

            _currentEnemy.SetActive(true);
        }

        _attackButton.gameObject.SetActive(true);
        _escapeButton.gameObject.SetActive(true);
    }

    public void HideBattlePhaseButton()
    {
        _attackButton.gameObject.SetActive(false);
        _escapeButton.gameObject.SetActive(false);
    }

    public async UniTask HideEnemy()
    {
        await LMotion.Create(new Color(1, 1, 1, 1), new Color(1, 1, 1, 0), _hideEnemyFadeTime)
            .WithOnComplete(() => _currentEnemy.SetActive(false))
            .BindToColor(_currentEnemy.gameObject.GetComponentInChildren<Image>());
        await UniTask.WaitForSeconds(_fadeTime);
    }


    public void Battle()
    {
        _currentEnemy = Instantiate(_enemys[UnityEngine.Random.Range(0, _enemys.Count)], _currentEnemyParent.transform);

        _currentEnemy.SetActive(true);

        ShowBattlePhaseButton(false);

    }

    public async UniTask Rest()
    {
        await UpdatePlayerText("HPを5回復しました");
    }

    public async UniTask Treasure()
    {
        int treasureNumber = _treasureNumbers[UnityEngine.Random.Range(0, _treasureNumbers.Count)];

        switch (treasureNumber)
        {
            case 1:
                SoundService.Instance.PlaySE(SEType.Goal_Jingle, 50);
                await UpdatePlayerText("ハヤブサの剣を手に入れた！");
                await UpdatePlayerText("次回の攻撃が与えるダメージが２倍！");
                _hasHayabusaSword = true;
                break;
            case 2:
                SoundService.Instance.PlaySE(SEType.Goal_Jingle, 50);
                await UpdatePlayerText("鉄の盾を手に入れた！");
                await UpdatePlayerText("次回受けるダメージが半減！");
                _hasIronShield = true;
                break;
            case 3:
                SoundService.Instance.PlaySE(SEType.Goal_Jingle, 50);
                await UpdatePlayerText("呪いの剣を手に入れた！");
                await UpdatePlayerText("次回の攻撃で与えるダメージが半減！");
                _hasCursedSword = true;
                break;
            case 4:
                await UpdatePlayerText("何も手に入らなかった...");
                break;

            default:
                break;
        }
    }

    public EnemyModel GetCurrentEnemy()
    {
        return _currentEnemy.GetComponent<EnemyPresenter>().EnemyModel;
    }

    public async UniTask UpdatePlayerText(string text)
    {
        _playerText.gameObject.SetActive(true);
        _playerText.text = text;
        await UniTask.WaitForSeconds(_textWaitTime);
        _playerText.gameObject.SetActive(false);
        await UniTask.WaitForSeconds(_phaseWaitTime);
    }

    public bool ChackHayabusaSword()
    {
        return _hasHayabusaSword;
    }

    public bool ChackIronShield()
    {
        return _hasIronShield;
    }

    public bool ChackCursedSword()
    {
        return _hasCursedSword;
    }

    public void ResetSwordTreasure()
    {
        _hasHayabusaSword = false;
        _hasCursedSword = false;
    }

    public void ResetShieldTreasure()
    {
        _hasIronShield = false;
    }
}
