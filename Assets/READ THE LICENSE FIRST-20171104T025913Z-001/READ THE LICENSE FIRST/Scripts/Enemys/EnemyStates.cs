using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class EnemyStates : MonoBehaviour
    {
        public float health;
        public bool canBeParried = true;
        public bool parryIsOn = true;
        public bool doParry = false;
        public bool isInvicible;
        public bool dontDoAnything;
        public bool canMove;
        public bool isDead;

        public StateManager parriedBy;

        public Animator anim;
        EnemyTarget enTarget;
        AnimatorHook a_hook;
        public Rigidbody rigid;
        public float delta;

        List<Rigidbody> ragdollRigids = new List<Rigidbody>();
        List<Collider> ragdollColliders = new List<Collider>();

        float timer;

        void Start()
        {
            health = 100;
            //rigid = GetComponent<Rigidbody>();
            anim = GetComponent<Animator>();
            enTarget = GetComponent<EnemyTarget>();
            enTarget.Init(this);

            a_hook = anim.GetComponent<AnimatorHook>();
            if (a_hook == null)
            {
                a_hook = anim.gameObject.AddComponent<AnimatorHook>();
            }
            a_hook.Init(null, this);

            InitRagdoll();
            parryIsOn = false;
        }

        void InitRagdoll()
        {
            Rigidbody[] rigs = GetComponentsInChildren<Rigidbody>();
            for (int i = 0; i < rigs.Length; i++)
            {
                if (rigs[i] == rigid)
                {
                    continue;
                }

                ragdollRigids.Add(rigs[i]);
                rigs[i].isKinematic = true;

                Collider col = rigs[i].gameObject.GetComponent<Collider>();
                col.isTrigger = true;
                ragdollColliders.Add(col);
            }
        }

        public void EnableRagdoll()
        {
            for (int i = 0; i < ragdollRigids.Count; i++)
            {
                ragdollRigids[i].isKinematic = false;
                ragdollColliders[i].isTrigger = false;
            }

            Collider controllerCollider = rigid.gameObject.GetComponent<Collider>();
            controllerCollider.enabled = false;
            rigid.isKinematic = true;

            StartCoroutine("CloseAnimator");
        }

        IEnumerable CloseAnimator()
        {
            yield return new WaitForEndOfFrame();
            anim.enabled = false;
            this.enabled = false;

            BreakAnimatorsKneesWithFuckingHammers();
        }

        public void BreakAnimatorsKneesWithFuckingHammers()
        {
            anim.runtimeAnimatorController = null;
        }

        void Update()
        {

            //Forces Death
            //By removing animation controller
            if (isDead) {
                anim.runtimeAnimatorController = null;
            }

            delta = Time.deltaTime;

            if (!isDead) { 
                canMove = anim.GetBool("canMove");
            }
            //end of forced death//

            if (dontDoAnything)
            {
                dontDoAnything = !canMove;

                return;
            }

            if (health <= 0)
            {
                if (!isDead)
                {
                    isDead = true;
                    EnableRagdoll();
                }
            }

            if (isInvicible)
            {
                isInvicible = !canMove;
            }

            if (parriedBy != null && parryIsOn == false)
            {
                //parriedBy.parryTarget = null;
                parriedBy = null;
            }

            if (canMove)
            {
                parryIsOn = false;

                anim.applyRootMotion = false;

                //debug
                timer += Time.deltaTime;
                if (timer > 3)
                {
                    DoAction();
                    timer = 0;
                }
            }
        }

        void DoAction()
        {
            anim.Play("oh_attack_1");
            anim.applyRootMotion = true;
            anim.SetBool(StaticStrings.canMove, false);
        }

        public void DoDamge(float v)
        {
            if (isInvicible)
            {
                return;
            }

            health -= v;
            isInvicible = true;
            anim.Play("damage_2");
            anim.applyRootMotion = true;
            anim.SetBool(StaticStrings.canMove, false);
        }

        public void CheckForParry(Transform target, StateManager states)
        {
            if (canBeParried == false || parryIsOn == false || isInvicible)
            {
                return;
            }

            //Debug.Log("Made it here too!");
            Vector3 dir = transform.position - target.position;
            dir.Normalize();
            float dot = Vector3.Dot(target.forward, dir);

            if (dot < 0)
            {
                return;
            }

            isInvicible = true;
            anim.Play("attack_interrupt");
            anim.applyRootMotion = true;
            anim.SetBool(StaticStrings.canMove, false);
            //states.parryTarget = this;
            parriedBy = states;
            doParry = true;
            return;
        }

        public void IsGettingParried()
        {
            health -= 500;
            dontDoAnything = true;
            anim.SetBool(StaticStrings.canMove, false);
            anim.Play("parry_recieved");
        }

        public void IsGettingBackStabbed()
        {
            health -= 500;
            dontDoAnything = true;
            anim.SetBool(StaticStrings.canMove, false);
            anim.Play("backStabbed");
        }
    }
}
