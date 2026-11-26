using UnityEngine;

namespace _Members.KDH.Scripts
{
    public class EnemyGenerator : MonoBehaviour
    {
        [SerializeField] private GameObject testEnemyPrefab;
        [SerializeField, Min(1)] private int spawnLength;

        public void SpawnEnemies(BSPMapGenerator mapGenerator)
        {
            SpawnPointGenerator(mapGenerator.RootNode, spawnLength);
        }

        private void SpawnPointGenerator(BSPNode node, int length)
        {
            int xMin = node.Room.xMin;
            int yMin = node.Room.yMin;
            int xMax = node.Room.xMax;
            int yMax = node.Room.yMax;
            int halfWidth = node.Room.width / 2;
            int halfHeight = node.Room.height / 2;

            if (node.LeftNode == null && node.RightNode == null)
            {
                for (int x = xMin + halfWidth / 2; x < xMax - halfWidth / 2; x += halfWidth / (int)Mathf.Sqrt(length))
                {
                    for (int y = yMin + halfHeight / 2; y < yMax - halfHeight / 2; y += halfHeight / (int)Mathf.Sqrt(length))
                    {
                        Instantiate(testEnemyPrefab, new Vector2(x, y), Quaternion.identity);
                    }
                }
            }
            else
            {
                SpawnPointGenerator(node.LeftNode, length);
                SpawnPointGenerator(node.RightNode, length);
            }
        }
    }
}
