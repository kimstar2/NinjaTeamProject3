using UnityEngine;

namespace _Members.KDH.Scripts
{
    public class BSPNode
    {
        public RectInt Room;

        public BSPNode LeftNode; // 이게 무조권 왼쪽 노드가 아님. 그냥 구분할려고 이름을 이렇게 지은 것
        public BSPNode RightNode;
        public BSPNode ParentNode;

        public BSPNode(RectInt room)
        {
            Room = room;
        }

        public bool IsXInRect(int x)
        {
            return x >= Room.xMin && x < Room.xMax;
        }

        public bool IsYInRect(int y)
        {
            return y >= Room.yMin && y < Room.yMax;
        }

        public int GetXCenter()
        {
            return Room.x + Room.width / 2;
        }

        public int GetYCenter()
        {
            return Room.y + Room.height / 2;
        }
    }
}