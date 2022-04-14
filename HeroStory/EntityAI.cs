using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAI : MonoBehaviour
{
    public Entity owner;

    [Header("Patrol")]
    public bool usePatrol = false;
    public Vector3 spawnPosition;
    public float patrolRadius;
    public float patrolWaitTime;
    [HideInInspector] public float waitTimer = 0f;
    
    [Header("Kitting")]
    public bool useKitting = false;
    public float minRadius = 0f;
    public float maxRadius = 0f;
    public float angleRange = 360f;
    public float kittingWaitTime;
    public bool useTargetDistance = false;
    public bool cancelFinishTerm = false;
    public bool isKittingNow = false;
    [HideInInspector] public float kittingWaitTimer = 0f;

    [Header("Skill Cost")]
    public int[] skillCosts;
    List<(int, int)> skills = new List<(int, int)>();

    [Header("Searching")]
    public bool useSearching = false;
    public float searchingRadius = 0f;

    [Header("Agent")]
    public bool useSlowRotate = false;
    public float slowRotateSpeed = 2f;
    [HideInInspector] public bool finishedRotate = false;

    // Start is called before the first frame update
    void Start()
    {
        if (!owner) owner = GetComponentInParent<Entity>();

        spawnPosition = transform.position;
        waitTimer = 0f;

        for (int i = 0; i < owner.skillTemplates.Length; i++) {
            if (skillCosts[i] > 0)
            {
                //Debug.Log("Add " + owner.skillTemplates[i].name);
                skills.Add((skillCosts[i], i));
            }
        }
        skills.Sort((skill1, skill2) => skill2.Item1.CompareTo(skill1.Item1));
    }

    // Update is called once per frame
    void Update()
    {
        if (owner.state != State.Move)
        {
            waitTimer = Mathf.Max(0, waitTimer - Time.deltaTime);
            kittingWaitTimer = Mathf.Max(0, kittingWaitTimer - Time.deltaTime);
        }
    }

    public Vector3 GetNextPatrolPosition()
    {
        Vector3 vector = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
        vector.Normalize();
        return spawnPosition + (vector * Random.Range(0f, patrolRadius));
    }

    public Vector3 GetNextKittingPosition(Vector3 targetPos)
    {
        Vector3 targetVec = (targetPos - owner.transform.position).normalized;
        float exeptAngle = (360f - angleRange) * 0.5f;
        Quaternion rotVec = Quaternion.Euler(0f, exeptAngle, 0f);
        Vector3 rotTargetVec = rotVec * targetVec;

        float angle = Random.Range(0, angleRange);
        rotVec = Quaternion.Euler(0f, angle, 0f);
        rotTargetVec = rotVec * rotTargetVec;

        float moveRadius = Random.Range(minRadius, maxRadius);

        return owner.transform.position + (rotTargetVec * moveRadius);
    }

    public int GetNextSkillSelect()
    {
        for (int i = skills.Count - 1; i >= 0; i--)
        {
            var skill = owner.skills[skills[i].Item2];
            if (owner.CastCheckSelf(skill) && owner.CastCheckTarget(skill))
                return skills[i].Item2;
        }
        return -1;
    }

    public Entity GetEntitySuitableTarget()
    {
        var entities = owner.GetEntitiesInRange(searchingRadius, owner.team);
        foreach (var entity in entities)
            return entity;
        return null;
    }
}
