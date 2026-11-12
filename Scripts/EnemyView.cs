using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyView : MonoBehaviour
{
    [SerializeField] private Slider _enemyHPSlider;

    public void SetEnemyMaxHP(int maxHP)
    {
        _enemyHPSlider.maxValue = maxHP;
        _enemyHPSlider.value = maxHP;
    }

    public void UpdateEnemyHP(int currentHP)
    {
        _enemyHPSlider.value = currentHP;
        _enemyHPSlider.GetComponentInChildren<TextMeshProUGUI>().text = $"{currentHP}/{_enemyHPSlider.maxValue}";
    }
}
