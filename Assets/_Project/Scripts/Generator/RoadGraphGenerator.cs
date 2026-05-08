using System.Collections.Generic;
using UnityEngine;

public enum RoadType { Main, Local }

public class RoadNode
{
    public Vector2 position;
    public List<RoadNode> connections = new List<RoadNode>();
    public Vector2 lastDirection;
    public RoadType type;
    public int turnCount = 0;
}

public class RoadGraphGenerator : MonoBehaviour
{
    public int maxNodes = 150;
    public float stepSize = 10f;
    [Range(0, 1)] public float mainRoadBranchChance = 0.1f;
    [Range(0, 1)] public float localRoadBranchChance = 0.4f;

    public List<RoadNode> nodes = new List<RoadNode>();
    private Queue<RoadNode> activeNodes = new Queue<RoadNode>();

    void Start() => Generate();

    [ContextMenu("Generate City")]
    void Generate()
    {
        nodes.Clear();
        activeNodes.Clear();

        // Спавним корень (Центральный проспект)
        RoadNode root = new RoadNode
        {
            position = Vector2.zero,
            type = RoadType.Main,
            lastDirection = Vector2.up
        };
        nodes.Add(root);
        activeNodes.Enqueue(root);

        int safety = 0;
        while (activeNodes.Count > 0 && nodes.Count < maxNodes && safety < 2000)
        {
            safety++;
            RoadNode current = activeNodes.Dequeue();

            // Шанс ветвления зависит от типа дороги
            float chance = (current.type == RoadType.Main) ? mainRoadBranchChance : localRoadBranchChance;
            int branches = (Random.value < chance) ? 2 : 1;

            for (int i = 0; i < branches; i++)
            {
                GenerateBranch(current);
            }
        }
    }

    void GenerateBranch(RoadNode parent)
    {
        Vector2 dir = GetSmartDirection(parent);
        Vector2 targetPos = Snap(parent.position + dir * stepSize);

        RoadNode existing = FindNodeAt(targetPos);

        if (existing != null)
        {
            Connect(parent, existing);
        }
        else
        {
            RoadNode newNode = new RoadNode
            {
                position = targetPos,
                lastDirection = dir,
                type = (parent.type == RoadType.Main && Random.value > 0.2f) ? RoadType.Main : RoadType.Local,
                turnCount = (dir != parent.lastDirection) ? parent.turnCount + 1 : 0
            };

            // Если слишком много поворотов — заставляем идти прямо
            if (newNode.turnCount > 2) newNode.lastDirection = parent.lastDirection;

            nodes.Add(newNode);
            Connect(parent, newNode);
            activeNodes.Enqueue(newNode);
        }
    }

    Vector2 GetSmartDirection(RoadNode node)
    {
        Vector2[] dirs = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
        Vector2 selected = dirs[Random.Range(0, dirs.Length)];

        // Запрет разворота на 180 градусов
        if (selected == -node.lastDirection)
            selected = node.lastDirection;

        return selected;
    }

    Vector2 Snap(Vector2 pos)
    {
        return new Vector2(
            Mathf.Round(pos.x / stepSize) * stepSize,
            Mathf.Round(pos.y / stepSize) * stepSize
        );
    }

    RoadNode FindNodeAt(Vector2 pos) => nodes.Find(n => Vector2.Distance(n.position, pos) < 0.1f);

    void Connect(RoadNode a, RoadNode b)
    {
        if (a == b) return;
        if (!a.connections.Contains(b)) a.connections.Add(b);
        if (!b.connections.Contains(a)) b.connections.Add(a);
    }

    private void OnDrawGizmos()
    {
        foreach (var node in nodes)
        {
            Gizmos.color = (node.type == RoadType.Main) ? Color.red : Color.gray;
            Vector3 p1 = new Vector3(node.position.x, 0, node.position.y);
            Gizmos.DrawSphere(p1, 0.3f);

            foreach (var conn in node.connections)
            {
                Vector3 p2 = new Vector3(conn.position.x, 0, conn.position.y);
                Gizmos.DrawLine(p1, p2);
            }
        }
    }
}