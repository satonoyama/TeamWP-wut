using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.Monetization;

namespace MagicalFX
{
	public class Wizard : MonoBehaviour
	{
        public GameObject[] SpellsLearned;
		public int Index;

		// 取得するMagicInfo
		float shootDelay;
		float otherEffDelay;
		EffectCategorize effCats;

		GameObject otherEffect;
		bool otherEffVisible;

		FX_ScaleControl scaleControl;

		GameObject playerCamera;
        Vector3 positionLook;
		float timeTemp;
		
		public WizardControls Controls { get; private set; }

        void Awake()
        {
			// カーソルロック
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;

			Controls = new WizardControls();
			Controls.Magic.Burst.performed += OnFire;
			//Controls.Magic.Fire.started += Fire;

			playerCamera = transform.GetChild(3).gameObject;
        }

        void Start()
		{
			timeTemp = Time.time;
			LoadSpellInfo();
		}

        void Update()
        {
            if (otherEffVisible && Time.time > timeTemp + otherEffDelay)
            {
				otherEffVisible = false;
            }
			UpdateOtherEffect();
        }
		
		void LoadSpellInfo()
        {
			var info = SpellsLearned[Index].GetComponent<MagicInfo>();
			shootDelay	  = info.Delay;
			otherEffDelay = info.OtherEffectDelay;

			effCats = SpellsLearned[Index].GetComponent<EffectCategorize>();
        }

        void Aim ()
		{
			//Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			//RaycastHit hit;
			//if (Physics.Raycast (ray, out hit, 100))
			//	positionLook = hit.point;
		
			//Quaternion look = Quaternion.LookRotation ((positionLook - this.transform.position).normalized);
			//look.eulerAngles = new Vector3 (0, look.eulerAngles.y, 0);
			//this.transform.rotation = Quaternion.Lerp (this.transform.rotation, look, 0.5f);
		}
		
		void OnFire(InputAction.CallbackContext context)
        {
			if (Time.time >= timeTemp + shootDelay) {
				Shoot(SpellsLearned[Index]);
				timeTemp = Time.time;
			}
        }

		void Shoot (GameObject spell)
		{
            /*---->
             * 弾 */
            var projectile = Instantiate (effCats.MainObject,
                         this.transform.position + (Vector3.up * 1.5f) + (this.transform.forward * 0.3f) + (this.transform.right * 0.5f),
                         playerCamera.transform.rotation);
			// Colliderはプレイヤーを無視するように
			projectile.layer = 10; // 10: ProjectilePlayer

			/*---->
			 * 発射中エフェクト */
            // ex. 魔法陣
			if (otherEffect != null) return;
            otherEffect = Instantiate(effCats.MuzzleFlash,
                    CalcFirePos(),
                    CalcFireRot());
			scaleControl = otherEffect.GetComponent<FX_ScaleControl>();
            otherEffVisible = true;
        }

        void Place (GameObject skill)
		{
			Instantiate (skill, positionLook, skill.transform.rotation);
		}

		void PlaceDirection (GameObject skill)
		{
			GameObject sk = (GameObject)GameObject.Instantiate (skill, this.transform.position + this.transform.forward, skill.transform.rotation);
			FX_Position fx = sk.GetComponent<FX_Position>();
			if (fx.Mode == SpawnMode.OnDirection)
				fx.transform.forward = this.transform.forward;
		}

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
            }
        }
		
		// TODO: マジックナンバーを修正
		Vector3 CalcFirePos()
        {
			return this.transform.position + 
				(Vector3.up * 1.5f) + (this.transform.forward * 0.3f) + (this.transform.right * 0.5f);
        }
		Quaternion CalcFireRot()
        {
			return playerCamera.gameObject.transform.rotation;
        }

		private void OnEnable() => Controls.Enable();
		private void OnDisable() => Controls.Disable();
	}
}