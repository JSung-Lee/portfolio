using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
public enum ItemType
{
    Potion, Equipment, Size,
}

public enum ItemGrade
{
    Common, Rare, Legendary, Size
}
public enum DoorType
{
    left, right, top, bottom
}

public enum DamageType
{
    Physical, Magic, True, Size
}

public enum State
{
    None,
    Idle,
    Move,
    Rotate,
    Casting,
    Stun,
    Silence,
    Dead,
    Size,
}

public enum Team
{
    Player, Enemy, Neutral, All
};


public enum EffectMount
{
    None, LeftHand, RightHand, BothHands, Head, Center, Bottom, Weapon1, Weapon2, Weapon3, Move, 
    LeftFoot, RightFoot, Size,
}
public enum SkillConditionType
{
    Self, ConditionSkill,
    Size
}
public enum SkillCondition
{ 
    None, 
    StageStart, StageClear, RoomEnter, RoomClear, Battle, 
    Attack, ReceiveDeal, Death, KillEnemy, LevelUp, Critical, cleanDebuffs,
    Size
}
public enum SkillTarget
{
    None,
    Self, Attacker, Receiver,
    NearEnemy, AllEnemy,
    NearTeam, AllTeam,
    NearAll, All, 
    Size
}
public enum RoomName
{
    Start, Normal, Hidden, Item, Boss, Shop, Battle
}
public enum RoomType
{
    Single, Double, Triple, Quad
}
public enum Direction
{
    left, up, right, down
}
public static class Extensions
{
    public static Vector3 NearestValidDestination(this NavMeshAgent agent, Vector3 destination)
    {
        if (!agent.enabled) return agent.transform.position;

        var path = new NavMeshPath();
        if (agent.CalculatePath(destination, path))
            return path.corners[path.corners.Length - 1]; 

        NavMeshHit hit;
        if (NavMesh.SamplePosition(destination, out hit, agent.speed * 2, NavMesh.AllAreas))
            if (agent.CalculatePath(hit.position, path))
                return path.corners[path.corners.Length - 1];

        return agent.transform.position;
    }

    public static void SetDestinationWithCalculatePath(this NavMeshAgent agent, Vector3 destination)
    {
        if (!agent.enabled) return;
        var path = new NavMeshPath();
        agent.CalculatePath(destination, path);
        agent.SetPath(path);
    }
    public static void SetListener(this UnityEvent uEvent, UnityAction call)
    {
        uEvent.RemoveAllListeners();
        uEvent.AddListener(call);
    }

    public static void SetListener<T>(this UnityEvent<T> uEvent, UnityAction<T> call)
    {
        uEvent.RemoveAllListeners();
        uEvent.AddListener(call);
    }

    public static void MeshNormalAverage(this Mesh mesh)
    {
        Dictionary<Vector3, List<int>> map = new Dictionary<Vector3, List<int>>();

        for(int v = 0; v < mesh.vertexCount; ++v)
        {
            if(!map.ContainsKey(mesh.vertices[v]))
            {
                map.Add(mesh.vertices[v], new List<int>());
            }

            map[mesh.vertices[v]].Add(v);
        }

        Vector3[] normals = mesh.normals;
        Vector3 normal;

        foreach(var p in map)
        {
            normal = Vector3.zero;

            foreach(var n in p.Value)
            {
                normal += mesh.normals[n];
            }

            normal /= p.Value.Count;

            foreach (var n in p.Value)
            {
                normals[n] = normal;
            }
        }

        mesh.normals = normals;
    }
    public static T CopyComponent<T>(T original, GameObject destination) where T : Component
    {
        System.Type type = original.GetType();
        Component copy = destination.AddComponent(type);
        System.Reflection.FieldInfo[] fields = type.GetFields();
        foreach (System.Reflection.FieldInfo field in fields)
        {
            field.SetValue(copy, field.GetValue(original));
        }
        return copy as T;
    }

    public static void SetLossyScale(this Transform _transform, Vector3 scale)
    {
        var parent = _transform.parent;
        _transform.parent = null;
        _transform.localScale = scale;
        _transform.parent = parent;
    }

    public static string KeyCodeToString(this KeyCode keyCode)
    {
        if (keyCode >= KeyCode.Alpha0 && keyCode <= KeyCode.Alpha9)
            return keyCode.ToString().Replace("Alpha", "");
        return keyCode.ToString();
    }

    public static KeyCode GetInputKey()
    {
        foreach (KeyCode kcode in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKey(kcode))
                return kcode;
        }
        return KeyCode.None;
    }

    public static Vector3 RotatePosition(Vector3 from, Vector3 to, float angle)
    {
        Quaternion v3Rotation = Quaternion.Euler(0f, angle, 0f);
        Vector3 v3Direction = (to - from).normalized;
        Vector3 v3RotatedDirection = v3Rotation * v3Direction;
        float dist = Vector3.Distance(from, to);
        return from + (v3RotatedDirection * dist);
    }

    public static Vector3 RotatedPosition(this Transform from, Vector3 to, float t)
    {
        float angle = from.forward.GetAngle((to - from.position).normalized);
        Quaternion v3Rotation = Quaternion.Euler(0f, from.eulerAngles.y + (angle * t), 0f);
        Vector3 v3Direction = from.forward;
        Vector3 v3RotatedDirection = v3Rotation * v3Direction;
        float dist = Vector3.Distance(from.position, to);
        return from.position + (v3RotatedDirection * dist);
    }

    public static float GetAngle(this Vector3 from, Vector3 to)
    {
        return Quaternion.FromToRotation(Vector3.up, to - from).eulerAngles.z;
    }

    public static Vector3 FindEmptySpace(Vector3 center, float size, int ignore, int rimit = 5)
    {
        //size is radius
        //int layerMask = (1 << LayerMask.NameToLayer("Floor")) | 
        //                (1 << LayerMask.NameToLayer("Ignore Raycast"));
        //layerMask = ~layerMask;
        int layerMask = (1 << LayerMask.NameToLayer("Floor")) |
                        (1 << LayerMask.NameToLayer("Ignore Raycast"));
        int floor = 0;
        while(floor < rimit) 
        {
            float r = size * floor;
            float circle = 2 * Mathf.PI * r;
            int count = (int)(circle / (size * 2));
            if (count == 0) count = 1;
            float angle = 360f / count;
            for (int i = 0; i < count; i++) {
                float gap = angle * i;
                Vector3 pos = RotatePosition(center, center + (Vector3.forward * r), gap);
                Collider[] colliders = Physics.OverlapSphere(pos, size, ~layerMask);
                if (colliders.Length <= 0) {
                    if (ignore <= 0) return pos;
                    else ignore--;
                }
            }
            floor += 1;
        }
        return Vector3.zero;
    }

    public static void SetTeamLayerInAllChildren(Transform tr, int layer)
    {
        int monster = LayerMask.NameToLayer("Monster");
        int player = LayerMask.NameToLayer("Player");
        if (tr.gameObject.layer == player || tr.gameObject.layer == monster)
            tr.gameObject.layer = layer;
        foreach (Transform child in tr) {
                SetTeamLayerInAllChildren(child, layer);
        }
    }

    public static void SetYPosition(this Transform tr, float y)
    {
        // Set 동작이 먹히지 않음.
        // tr.position.Set(tr.position.x, y, tr.position.z);
        tr.position = new Vector3(tr.position.x, y, tr.position.z);
    }

    public static bool ToBool(this int value)
    {
        return value == 0 ? false : true;
    }
    public static int ToInt(this bool value)
    {
        return value ? 1 : 0;
    }
}
