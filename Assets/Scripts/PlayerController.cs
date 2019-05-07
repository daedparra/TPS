using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class PlayerController : AUnit
{

	#region GlobalVariables

		[Header("Movement")]
		[SerializeField]
		private float _MoveSpeed = 5f;

		[SerializeField]
		private KeyCode _SprintKey = KeyCode.LeftShift;

		[SerializeField]
		private float _SprintMult = 5f;

		[Header("Jumping")]
		[SerializeField]
		private float _JumpSpeed = 12f;

		[SerializeField]
		private KeyCode _JumpKey = KeyCode.Space;

		[Header("Camera")]
		[SerializeField]
		private Transform _CamPivot;


		private Transform _CamTransform;
		private float _XInput;
		private float _ZInput;
		private bool _JumpPressed;
		private float _SpeedMult = 1f;

	#endregion GlobalVariables


	#region UnityFunctions 

		protected override void UnitAwake()
		{
			_CamTransform = _CamPivot.GetComponentInChildren<Camera>().transform;
		}

		private void Update()
		{
            if(IsAlive == false)
            {
                return;
            }

			ReadMoveInputs();
			SetAnimValues();
			UpdateRotations();
            ShootLasers();
			CameraZoom();
		}

		private void FixedUpdate()
		{
            if(IsAlive == false)
            {
                return;
            }

			ApplyMovePhysics();
		}

	#endregion UnityFunctions


	#region ClassFunctions 

		private void UpdateRotations()
		{
			float mouseX = Input.GetAxis("Mouse X");
			float mouseY = Input.GetAxis("Mouse Y");
			transform.Rotate(0f, mouseX, 0f);
			_CamPivot.Rotate(-mouseY, 0, 0);
		}

		private void ReadMoveInputs()
		{
			_XInput = Input.GetAxis("Horizontal");
			_ZInput = Input.GetAxis("Vertical");
			_SpeedMult = Input.GetKey(_SprintKey) ? _SprintMult : 1f;

			if(Input.GetKeyDown(_JumpKey))
			{
				_JumpPressed = true;
				_Anim.SetTrigger("Jump");
			}
		}

		private void ApplyMovePhysics()
		{
			var newVel = new Vector3(_XInput, 0f, _ZInput) * _MoveSpeed * _SpeedMult;
			newVel = transform.TransformVector(newVel);
			newVel.y =  _JumpPressed ? _JumpSpeed : _RB.velocity.y;
			_RB.velocity = newVel;

			_JumpPressed = false;
		}

		private void CameraZoom()
		{
			var newZoom = _CamTransform.localPosition;
			newZoom.z += Input.mouseScrollDelta.y;
			newZoom.z = Mathf.Clamp(newZoom.z, -24, 0);
			_CamTransform.localPosition = newZoom;
		}

		private void SetAnimValues()
		{
			_Anim.SetFloat("SideMovement", _XInput * _SpeedMult);
			_Anim.SetFloat("ForwardMovement", _ZInput * _SpeedMult);
			_Anim.SetFloat("SpeedMult", _SpeedMult);
		}

        private void ShootLasers()
        {
            if(Input.GetMouseButtonDown(0)) /// If we press LMB
            {
                Ray ray = new Ray(_CamTransform.position, _CamTransform.forward);
                RaycastHit hit;
                if(Physics.Raycast(ray, out hit))
                {
                    if(CanSee(hit.point, hit.transform))
                    {
                        ShootLasersFromEyes(hit.point, hit.transform);
                    }
                }
            }
        }

    #endregion ClassFunctions

}
