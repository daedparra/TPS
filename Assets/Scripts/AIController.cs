using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AIController : AUnit
{
    [Header("Enemy Detection")]

    [SerializeField]
    private float _EnemyDetectionRadius = 5f;

    [SerializeField]
    private LayerMask _UnitsLayerMask;
    [Header("Attack Parameters")]

    [SerializeField]
    private float _ShootCooldown = 1f;


	private NavMeshAgent _Agent;
	private IEnumerator _CurrentState = null;
	private Outpost _TargetOutpost = null;
    private AUnit _TargetEnemy = null;

	protected override void UnitAwake()
	{
		_Agent = GetComponent<NavMeshAgent>();
	}

	private void Start()
	{
		SetState(State_Idle());
	}

	private void Update()
	{
        if(IsAlive == false)
        {
            return;
        }

		_Anim.SetFloat("ForwardMovement", _Agent.velocity.magnitude);
	}

	#region StateMachine

		private void SetState(IEnumerator newState)
		{
			if(_CurrentState != null)
			{
				StopCoroutine(_CurrentState);
			}

			_CurrentState = newState;
            StartCoroutine(_CurrentState);
		}

		private IEnumerator State_Idle()
		{
			while(_TargetOutpost == null)
			{
				LookForOutpost();
				yield return null;
			}

			SetState(State_MovingToOutpost());
		}

		private IEnumerator State_MovingToOutpost()
		{
            // _Agent.isStopped = false; // _NOTE: THIS
			_Agent.SetDestination(_TargetOutpost.transform.position);
			while(_Agent.remainingDistance > _Agent.stoppingDistance)
			{
                LookForEnemy();
				yield return null;
			}

			SetState(State_CapturingOutpost());
		}

		private IEnumerator State_CapturingOutpost()
		{
            // _Agent.isStopped = true; // _NOTE: THIS
			while(_TargetOutpost != null && (_TargetOutpost.CurrentTeam != TeamNumber || _TargetOutpost.CaptureValue < 1f))
			{
                LookForEnemy();
				yield return null;
			}
			_TargetOutpost = null;
			SetState(State_Idle());
		}

        private IEnumerator State_AttackingEnemy()
        {
            _Agent.isStopped = true;
            _Agent.ResetPath();
            float shootTimer = 0f;

            while(_TargetEnemy != null && _TargetEnemy.IsAlive)
            {
                shootTimer += Time.deltaTime;

                transform.LookAt(_TargetEnemy.transform);
                transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);

                if(shootTimer >= _ShootCooldown)
                {
                    shootTimer = 0f;
                    ShootLasersFromEyes(_TargetEnemy.transform.position + Vector3.up, _TargetEnemy.transform);
                }

                yield return null;
            }

            _TargetEnemy = null;
            SetState(State_Idle());
        }

        private IEnumerator State_Dead()
        {
            yield return null;
        }
		
	#endregion StateMachine

	private void LookForOutpost()
	{
		var outpost = Outpost.OutpostList.GetRandomOutpost();
		if(outpost == null) return;
		if(outpost.CurrentTeam != TeamNumber || outpost.CaptureValue < 1f)
		{
			_TargetOutpost = outpost;
		}
		else
		{
			_TargetOutpost = null;
		}
	}

    private void LookForEnemy()
    {
        var aroundMe = Physics.OverlapSphere(transform.position, _EnemyDetectionRadius, _UnitsLayerMask);
        foreach (var item in aroundMe)
        {
            var otherUnit = item.GetComponent<AUnit>();
            if(otherUnit != null && otherUnit.TeamNumber != TeamNumber && otherUnit.IsAlive)
            {
                _TargetEnemy = otherUnit;
                SetState(State_AttackingEnemy());
                return;
            }
        }
    }

    protected override void Die()
    {
        base.Die();
        _Agent.isStopped = true;
        _Agent.ResetPath(); 
        _TargetOutpost = null;
        SetState(State_Dead());
    }

}
