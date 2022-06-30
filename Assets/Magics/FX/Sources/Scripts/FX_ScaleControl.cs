using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagicalFX
{
	public class FX_ScaleControl : MonoBehaviour
    {
        [SerializeField] Vector3 maxScalePercent;
        [field: SerializeField] public float Duration { get; private set; }

        public bool CloseSpell { get; set; }

        enum ControlState
        {
            NONE, OPENING, ENDING
        }

        ControlState state;
        Vector3 initScale;

        bool startSpell;

        void Start()
        {
            initScale = transform.localScale;

            //transform.DOScale(Vector3.Scale(initScale, maxScalePercent), Duration);

            maxScalePercent = Vector3.Scale(initScale, maxScalePercent);
            transform.localScale = Vector3.zero;
            startSpell = true;
        }

        void Update()
        {
            if (!(Time.deltaTime > 0f)) return;

            UpdateOpening();
        }

        void UpdateOpening()
        {
            if (!startSpell) return;

            transform.localScale = Vector3.Lerp(transform.localScale, maxScalePercent, Duration * Time.deltaTime);

            if (transform.localScale == maxScalePercent)
            {
                startSpell = false;
            }
        }

        // Lerpの最終フレームに達したらfalse
        public bool ClosingPerFrame()
        {
            if (startSpell)
            {
                startSpell = false;
            }

            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, Duration * Time.deltaTime);
            return transform.localScale != Vector3.zero;
        }
    }
}