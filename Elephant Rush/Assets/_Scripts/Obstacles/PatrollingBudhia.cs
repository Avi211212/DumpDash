using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using static UnityEngine.Rendering.HableCurve;


public class PatrollingBudhia : Obstacle
{
    static int s_SpeedRatioHash = Animator.StringToHash("SpeedRatio");
    static int s_DeadHash = Animator.StringToHash("Dead");

    [Tooltip("Minimum time to cross all lanes.")]
    public float minTime = 2f;
    [Tooltip("Maximum time to cross all lanes.")]
    public float maxTime = 5f;
    [Tooltip("Leave empty if no animation")]
    public Animator animator;
    public float rotationSpeed = 90f; // Adjust as needed

    public AudioClip[] patrollingSound;

    protected TrackSegment m_Segement;

    [SerializeField] private Transform budhiaTransform;

    protected Vector3 m_OriginalPosition = Vector3.zero;
    protected Quaternion m_OriginalRotation = Quaternion.identity;
    protected Vector3 lastPosVector = Vector3.zero;
    protected Vector3 currentPosVector = Vector3.zero;
    protected float m_MaxSpeed;
    protected float m_CurrentPos;

    protected AudioSource m_Audio;
    private bool m_isMoving = false;

    protected const float k_LaneOffsetToFullWidth = 2f;

    public override IEnumerator Spawn(TrackSegment segment, float t)
    {
        Vector3 position;
        Quaternion rotation;
        segment.GetPointAt(t, out position, out rotation);
        AsyncOperationHandle op = Addressables.InstantiateAsync(gameObject.name, position, rotation);
        yield return op;
        if (op.Result == null || !(op.Result is GameObject))
        {
            Debug.LogWarning(string.Format("Unable to load obstacle {0}.", gameObject.name));
            yield break;
        }
        GameObject obj = op.Result as GameObject;

        obj.transform.SetParent(segment.objectRoot, true);

        PatrollingBudhia po = obj.GetComponent<PatrollingBudhia>();
        po.m_Segement = segment;

        //TODO : remove that hack related to #issue7
        Vector3 oldPos = obj.transform.position;
        obj.transform.position += Vector3.back;
        obj.transform.position = oldPos;

        po.Setup();
    }

    public override void Setup()
    {
        m_Audio = GetComponent<AudioSource>();
        if (m_Audio != null && patrollingSound != null && patrollingSound.Length > 0)
        {
            m_Audio.loop = true;
            m_Audio.clip = patrollingSound[Random.Range(0, patrollingSound.Length)];
            m_Audio.Play();
        }

        m_OriginalPosition = transform.localPosition + transform.right * m_Segement.manager.laneOffset;
        transform.localPosition = m_OriginalPosition;

        lastPosVector = transform.position;
        currentPosVector = transform.position;

        float actualTime = Random.Range(minTime, maxTime);

        //time 2, because the animation is a back & forth, so we need the speed needed to do 4 lanes offset in the given time
        m_MaxSpeed = (m_Segement.manager.laneOffset * k_LaneOffsetToFullWidth * 2) / actualTime;

        if (animator != null)
        {
            AnimationClip clip = animator.GetCurrentAnimatorClipInfo(0)[0].clip;
            animator.SetFloat(s_SpeedRatioHash, clip.length / actualTime);
        }

        m_isMoving = true;
    }

    public override void Impacted()
    {
        m_isMoving = false;
        base.Impacted();

        if (animator != null)
        {
            animator.SetTrigger(s_DeadHash);
        }
    }

    void Update()
    {
        if (!m_isMoving)
            return;

        m_CurrentPos += Time.deltaTime * m_MaxSpeed;
        lastPosVector = transform.localPosition;
        transform.localPosition = m_OriginalPosition - transform.right * Mathf.PingPong(m_CurrentPos, m_Segement.manager.laneOffset * k_LaneOffsetToFullWidth);
        currentPosVector = transform.localPosition;

        // Rotate in the direction of movement
        float dotProd = Vector3.Dot(lastPosVector, currentPosVector);

        if (dotProd < 0) 
        {
            Debug.Log("Rotate");
            budhiaTransform.rotation = Quaternion.Euler(0, budhiaTransform.rotation.y + 90, 0);
        }

        //transform.localRotation = m_OriginalRotation  transform.right * Mathf.PingPong(m_CurrentPos, m_Segement.manager.laneOffset * k_LaneOffsetToFullWidth);

        // Move forward
        //transform.Translate(Time.deltaTime * m_MaxSpeed * Vector3.forward );

        // Check if reached the end of the path
        //if (transform.position.z > m_OriginalPosition.z + m_Segement.manager.laneOffset * 4)
        //{
            // Rotate
            //transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
       // }
    }
}

