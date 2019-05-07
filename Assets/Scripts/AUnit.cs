using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(TintMaterial))]
public abstract class AUnit : MonoBehaviour
{
    public int TeamNumber = 0;

    [SerializeField]
    protected float _AttackDamage = 8f;

    [SerializeField]
    protected float _Health = 100f;

    [SerializeField]
    protected float _Emission = 15f;

    [SerializeField]
    protected Laser _LaserPrefab;

    protected Rigidbody _RB;
    protected Animator _Anim;
    protected Eye[] _Eyes;
    public bool IsAlive { get; protected set; }

    protected abstract void UnitAwake();
    protected void Awake()
    {
        IsAlive = true;
        _RB = GetComponent<Rigidbody>();
        _Anim = GetComponent<Animator>();
        _Eyes = GetComponentsInChildren<Eye>();

        SetTeam(TeamNumber);

        UnitAwake();
    }

    public void SetTeam(int team)
    {
        TeamNumber = team;
        Color teamColor = GameManager.Instance.TeamColors[TeamNumber];
        GetComponent<TintMaterial>().ApplyTintToMaterials(teamColor, _Emission);
    }

    protected bool CanSee(Vector3 hitPos, Transform who)
    {
        for (int i = 0; i < _Eyes.Length; i++)
        {
            Vector3 startPos = _Eyes[i].transform.position;
            Vector3 dir = hitPos - startPos;
            Ray ray = new Ray(startPos, dir);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit) && hit.transform == who)
            {
                return true;
            }
        }
        return false;
    }

    protected void ShootLasersFromEyes(Vector3 hitPos, Transform other)
    {
        foreach (var eye in _Eyes)
        {
            Instantiate(_LaserPrefab).Shoot(eye.transform.position, hitPos);
        }

        var otherUnit = other.GetComponent<AUnit>();
        if(otherUnit != null && otherUnit.TeamNumber != TeamNumber)
        {
            otherUnit.OnHit(_AttackDamage);
        }
    }

    public void OnHit(float damage)
    {
        _Health -= damage;
        if(_Health <= 0f)
        {
            _Health = 0f;
            Die();
        }
    }

    protected virtual void Die()
    {
        IsAlive = false;
        _Anim.SetBool("IsAlive", false);
    }

}
