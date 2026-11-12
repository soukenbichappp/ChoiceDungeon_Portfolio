using UnityEngine;
using UniRx;

public class EnemyPresenter : MonoBehaviour
{

    [SerializeField] private EnemyView _enemyView;
    [SerializeField] private int _enemyMaxHP;
    [SerializeField] private int _enemyAtkPower;

    private EnemyModel _enemyModel;
    public EnemyModel EnemyModel => _enemyModel;

    void Awake()
    {
        _enemyModel = new EnemyModel(_enemyMaxHP, _enemyAtkPower);
        _enemyView.SetEnemyMaxHP(_enemyModel.MaxHP.Value);
        
        _enemyModel.CurrentHP.Subscribe(currentHP => {
            _enemyView.UpdateEnemyHP(currentHP);
        }).AddTo(this);
    }

    public void TakeDamage(int damage)
    {
        _enemyModel.TakeEnemyDamage(damage);
    }
}
