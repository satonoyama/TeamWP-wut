using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEditor;

namespace MagicalFX
{
	public class SpellCaster : MonoBehaviour
    {
		GameObject spell;
        MagicInfo info;
        GameObject staff;
		EffectCategorize effCats;

		GameObject otherEffect;
		bool otherEffVisible;
		FX_ScaleControl scaleControl;

		GameObject playerCamera;
		float timeTemp;

        private PlayerStatus status;
        bool CheckSP => (status.Sp - info.ManaCost) >= info.ManaCost;

        public void Init(GameObject _spell, PlayerStatus _status, GameObject _staff)
        {
            spell = _spell;
            status = _status;
            staff = _staff;
        }

        void Awake()
        {
            playerCamera = transform.GetChild(3).gameObject;
        }

        void Start()
        {
            timeTemp = Time.time;

			LoadSpellInfo();
        }

        void Update()
        {
            if (otherEffVisible && Time.time > timeTemp + info.OtherEffectDelay)
            {
				otherEffVisible = false;
            }
			UpdateOtherEffect();
        }

        //private IEnumerator Blink()
        //{
        //    Vector3 movementVector = Quaternion.LookRotation(transform.forward, transform.up).eulerAngles;
        //    transform.position = Vector3.MoveTowards(transform.position, movementVector * 3.0f, 1f * Time.deltaTime);
        //    Debug.Log("execute");
        //    yield return null;
        //}
        IEnumerator Blink(Transform transform, float time, Vector3? position, Quaternion? rotation)
        {
            // 現在のposition, rotation
            var currentPosition = transform.position;
            var currentRotation = transform.rotation;

            // 目標のposition, rotation
            var targetPosition = position ?? currentPosition;
            var targetRotation = rotation ?? currentRotation;

            var sumTime = 0f;
            while (true)
            {
                // Coroutine開始フレームから何秒経過したか
                sumTime += Time.deltaTime;
                // 指定された時間に対して経過した時間の割合
                var ratio = sumTime / time;

                transform.SetPositionAndRotation(
                    Vector3.Lerp(currentPosition, targetPosition, ratio),
                    Quaternion.Lerp(currentRotation, targetRotation, ratio)
                );

                if (ratio > 1.0f)
                {
                    // 目標の値に到達したらこのCoroutineを終了する
                    // ~.Lerpは割合を示す引数は0 ~ 1の間にClampされるので1より大きくても問題なし
                    break;
                }

                yield return null;
            }
        }

        void LoadSpellInfo()
        {
            info = spell.GetComponent<MagicInfo>();
            effCats = spell.GetComponent<EffectCategorize>();
        }
        //void Aim()
        //{
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit hit;
        //    if (Physics.Raycast(ray, out hit, 100))
        //        positionLook = hit.point;

        //    Quaternion look = Quaternion.LookRotation((positionLook - this.transform.position).normalized);
        //    look.eulerAngles = new Vector3(0, look.eulerAngles.y, 0);
        //    this.transform.rotation = Quaternion.Lerp(this.transform.rotation, look, 0.5f);
        //}

		public void PrepareFire()
        {
            if (Time.time >= timeTemp + info.Delay) // shoot delay
            {
                if (!CheckSP) return;
                status.AddSP(-info.ManaCost); // マナ消費

                Shoot();
                timeTemp = Time.time;
            }
        }

		void Shoot ()
		{
            /*---->
             * 弾 */
            var projectile = Instantiate (effCats.MainObject,
                         CalcFirePos(),
                         playerCamera.transform.rotation);
			// Colliderはプレイヤーを無視するように
			projectile.layer = 10; // 10: ProjectilePlayer

            //Vector3 movementVector = Quaternion.LookRotation(transform.forward, transform.up).eulerAngles;
            //StartCoroutine(Blink(transform, 1, transform.forward * 1.0f, null));
            //
            // これこそコルーチンで動かすべきだな
            //GetComponent<CharacterController>().Move(transform.forward * Time.deltaTime * .0f);


            /*---->
			 * 発射中エフェクト */
            // ex. 魔法陣
            if (otherEffect != null) return;
            otherEffect = Instantiate(effCats.MuzzleFlash,
                    CalcFirePos(),
                    CalcFireRot());
			scaleControl = otherEffect.GetComponent<FX_ScaleControl>();
            otherEffVisible = true;

            transform.GetChild(3).GetChild(0).gameObject.GetComponent<WeaponStateController>().
                _SwitchHoldState(true);
        }

        //void Place(GameObject skill)
        //{
        //    Instantiate(skill, positionLook, skill.transform.rotation);
        //}

        //void PlaceDirection(GameObject skill)
        //{
        //    GameObject sk = (GameObject)GameObject.Instantiate(skill, this.transform.position + this.transform.forward, skill.transform.rotation);
        //    FX_Position fx = sk.GetComponent<FX_Position>();
        //    if (fx.Mode == SpawnMode.OnDirection)
        //        fx.transform.forward = this.transform.forward;
        //}

        void UpdateOtherEffect()
        {
			if (otherEffect == null) return;

			otherEffect.transform.position = CalcFirePos();
			otherEffect.transform.rotation = transform.rotation;

			if (!otherEffVisible)
			{
                if (!scaleControl.ClosingPerFrame())
                {
					Destroy(otherEffect);
                    
                }
                transform.GetChild(3).GetChild(0).gameObject.GetComponent<WeaponStateController>().
                _SwitchHoldState(false);
                // タイミング的にSwitchHoldStateはここに設置してもよい
            }
        }
		
		// TODO: マジックナンバーを修正
		Vector3 CalcFirePos()
        {
			return staff.transform.position + (staff.transform.up * 0.9f);
        }
		Quaternion CalcFireRot()
        {
			return staff.transform.rotation;
        }
    }


    public class Wizard : MonoBehaviour
	{
		// よかったらEditor拡張で配列の名前をセットしてくれ
		enum ShootKeys {AttackMove1 = 0, AttackMove2, StatusMove, SpecialMove}
        [SerializeField] GameObject[] spellsLearned;
        SpellCaster[] caster;

        [SerializeField] private PlayerStatus status;

        [SerializeField] private GameObject staff;

        public WizardControls Controls { get; private set; }

        void Awake()
        {
			// カーソルロック
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;

			Controls = new WizardControls();
			Controls.Magic.AttackMove1.performed += OnFire_Atk1;
			Controls.Magic.AttackMove2.performed += OnFire_Atk2;
			Controls.Magic.StatusMove.performed += OnFire_Stat;
			Controls.Magic.SpecialMove.performed += OnFire_Spec;

            caster = new SpellCaster[spellsLearned.Length];
            int idx = 0;
            foreach (GameObject spell in spellsLearned)
            {
                caster[idx] = gameObject.AddComponent<SpellCaster>();
                caster[idx].Init(spell, status, staff);
                ++idx;
            }
        }

        void OnFire_Atk1(InputAction.CallbackContext context) { caster[(int)ShootKeys.AttackMove1].PrepareFire(); }
		void OnFire_Atk2(InputAction.CallbackContext context) { caster[(int)ShootKeys.AttackMove2].PrepareFire(); }
		void OnFire_Stat(InputAction.CallbackContext context) { caster[(int)ShootKeys.StatusMove].PrepareFire(); }
		void OnFire_Spec(InputAction.CallbackContext context) { caster[(int)ShootKeys.SpecialMove].PrepareFire(); }

        private void OnEnable() => Controls.Enable();
		private void OnDisable() => Controls.Disable();
	}
}