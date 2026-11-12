using Assets.MAIN.Common.Scripts.Domain;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public class FloorPresenter : MonoBehaviour
{
    [SerializeField] private GameResultData _gameResultData;
    [SerializeField] private PlayerManager _playerManager;
    [SerializeField] private FloorView _floorView;

    private FloorModel _floorModel;
    private int _enemyDefeatedCount;

    private void Awake()
    {
        _floorModel = new FloorModel();

        ProceedNextFloor().Forget();
    }

    private async UniTask ProceedNextFloor()
    {
        SoundService.Instance.PlaySE(SEType.Dash);
        _floorModel.AddCurrentFloor();

        if (IsFinalFloor())
        {
            await _floorView.ShowFinalFloorText();
        }
        else
        {
            await _floorView.ShowFloorText(_floorModel.CurrentFloor, _floorModel.MaxFloor);
        }
    }

    private void BattlePhase()
    {
        _floorView.Battle();
    }

    private async UniTask RestPhase()
    {
        SoundService.Instance.PlaySE(SEType.Back, 50);
        await _floorView.Rest();
        await _playerManager.AddStatus(StatName.currentHP, 5);
        await ProceedNextFloor();
    }

    private async UniTask TreasurePhase()
    {
        await _floorView.Treasure();
        await ProceedNextFloor();
    }

    private async UniTask AttackPhase()
    {
        await _playerManager.AttackCommand(_floorView.GetCurrentEnemy());

        if(!IsFinalFloor()) await ProceedNextFloor();
    }

    private async UniTask EscapePhase()
    {
        await _playerManager.AddStatus(StatName.dodgeChance, 2);
        await _floorView.HideEnemy();
        await _floorView.UpdatePlayerText("一目散に逃げた！");
        await ProceedNextFloor();
    }

    public void SetResulData(int PlayerLevel)
    {
        _gameResultData.SetResultData(PlayerLevel, _floorModel.CurrentFloor, _enemyDefeatedCount);
    }

    public bool IsFinalFloor()
    {
        return _floorModel.CurrentFloor == _floorModel.MaxFloor;
    }

    public async UniTask UpdatePlayerText(string text)
    {
        await _floorView.UpdatePlayerText(text);
    }

    public async UniTask HideEnemy()
    {
        await _floorView.HideEnemy();
    }

    public void AddEnemyDefeatedCount()
    {
        _enemyDefeatedCount++;
    }

    public void ResetSword()
    {
        _floorView.ResetSwordTreasure();
    }

    public void ResetShield()
    {
        _floorView.ResetShieldTreasure();
    }

    public void OnClickButtoleButton()
    {
        SoundService.Instance.PlaySE(SEType.Button);
        BattlePhase();
    }

    public void OnClickAttackButton()
    {
        SoundService.Instance.PlaySE(SEType.Button);
        AttackPhase().Forget();
    }

    public void OnClickEscapeButton()
    {
        SoundService.Instance.PlaySE(SEType.Button);
        EscapePhase().Forget();
    }

    public void OnClickRestButton()
    {
        SoundService.Instance.PlaySE(SEType.Button);
        RestPhase().Forget();
    }

    public void OnClickTreasureButton()
    {
        SoundService.Instance.PlaySE(SEType.Button);
        TreasurePhase().Forget();
    }

    public bool ChackHayabusaSword()
    {
        return _floorView.ChackHayabusaSword();
    }

    public bool ChackIronShield()
    {
        return _floorView.ChackIronShield();
    }

    public bool ChackCursedSword()
    {
        return _floorView.ChackCursedSword();
    }
}
