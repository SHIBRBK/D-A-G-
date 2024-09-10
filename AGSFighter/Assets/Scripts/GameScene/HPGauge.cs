using UnityEngine;
using UnityEngine.UI;

public class HPGauge : MonoBehaviour
{
    [SerializeField] private Image _gauge;
    [SerializeField] private Image _graceGauge;
    public float _currentHP;
    [SerializeField] public float _MAX_HP;
    private float _waitingTime = 0.0f;
    [SerializeField] private float _waitingSetTime = 0.0f;

    void Start()
    {
        _currentHP = _MAX_HP;
        Update();
    }

    private void Update()
    {
        if (_waitingTime <= 1.5f)
        {
            _waitingTime -= Time.deltaTime;
        }
        if (_waitingTime < 0.0f)
        {
            _graceGauge.fillAmount = _currentHP / _MAX_HP;
            _waitingTime = 0.0f;
        }
    }

    public void BeInjured(int attack)
    {
        float damage = attack;
        _waitingTime = _waitingSetTime;
        HPUpdate(damage);
    }

    void HPUpdate(float damage)
    {
        _currentHP -= damage;
        if (_currentHP < 0) _currentHP = 0; // HP‚ª0ˆÈ‰º‚É‚È‚ç‚È‚¢‚æ‚¤‚É‚·‚é
        _gauge.fillAmount = _currentHP / _MAX_HP;

        if (_gauge.fillAmount < 0.25f)
        {
            _gauge.color = Color.yellow;
        }
    }

    public void ResetHP()
    {
        _gauge.color = Color.black;
        _currentHP = _MAX_HP;
        _gauge.fillAmount = 1.0f;
        _graceGauge.fillAmount = 1.0f;
    }
}
