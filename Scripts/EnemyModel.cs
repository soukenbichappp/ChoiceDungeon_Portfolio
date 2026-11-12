using UniRx;

public class EnemyModel
{
    private ReactiveProperty<int> _enemyMaxHP = new ReactiveProperty<int>();
    private ReactiveProperty<int> _enemyCurrentHP = new ReactiveProperty<int>();
    private ReactiveProperty<int> _enemyAtkPower = new ReactiveProperty<int>();

    public ReactiveProperty<int> MaxHP => _enemyMaxHP;
    public ReactiveProperty<int> CurrentHP => _enemyCurrentHP;
    public ReactiveProperty<int> AtkPower => _enemyAtkPower;

    public EnemyModel(int maxHP, int atkPower)
    {
        _enemyMaxHP.Value = maxHP;
        _enemyCurrentHP.Value = maxHP;
        _enemyAtkPower.Value = atkPower;
    }

    public void TakeEnemyDamage(int damage)
    {
        _enemyCurrentHP.Value -= damage;
        if (_enemyCurrentHP.Value < 0)
        {
            _enemyCurrentHP.Value = 0;
        }
    }
}
