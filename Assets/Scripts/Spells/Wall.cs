using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Wall : MonoBehaviour
{
	[HideInInspector] public GameObject pointerPosition;
	[HideInInspector] public InputActionProperty handGrip;

	[HideInInspector] public Vector3 startPoint;

    [SerializeField] public float scaleSpeed = 1;
    [SerializeField] public bool debuging = false;

    [SerializeField] Transform continuesParticle;
    [SerializeField] GameObject oneTimeParticle;
    [SerializeField] Transform particlePos;

    private bool isScriptDone = false;

    public activateSpellcating asc;

    public Vector3 normal;
    public Quaternion upRotation;

    // Start is called before the first frame update
    void Start()
    {
		transform.position = startPoint;

        startPoint.y = pointerPosition.transform.position.y;

        if (debuging) print(startPoint);
	}

    // Update is called once per frame
    void Update()
    {

        if (handGrip.action.inProgress)
        {
            scaleWall();
            isScriptDone = true;
			continuesParticle.position = particlePos.position;
			continuesParticle.rotation = transform.rotation;
		}
        else if (isScriptDone)
        {
			
			asc.ResetFirstSpell();
            GameObject part = Instantiate(oneTimeParticle);
            part.transform.SetParent(transform.parent.transform);
            part.transform.position = transform.GetChild(0).position;
            ParticleSystem ps = part.GetComponent<ParticleSystem>();
            var pss = ps.shape;
            Vector3 scale = transform.localScale;
            scale.y /= 6;

			pss.scale = scale;

            var burst = ps.emission;
            burst.burstCount = (int)(20 * scale.y) * 2;

			this.enabled = false;

            ps.Play();

            
		}
		
	}

    private void scaleWall()
    {
		//float dis = Vector3.Distance(startPoint, pointerPosition.transform.position);
		float dis = pointerPosition.transform.position.y - startPoint.y;
        dis = Mathf.Clamp(dis, -4, 2);

        if (dis < 0)
        {
			transform.rotation = Quaternion.LookRotation(Vector3.Cross(transform.right, normal), normal);
		}
        else
        {
			transform.rotation = Quaternion.LookRotation(Vector3.Cross(-transform.right, normal), normal);
		}
        Vector3 scale = Vector3.one;
        scale.y *= dis * scaleSpeed;

		transform.localScale = scale;
	}

}
