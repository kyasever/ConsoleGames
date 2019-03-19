using Destroy;

namespace DestroyExample
{
    public static class MyFactroy
    {
        public static GameObject CreatePlayer(Vector2Int vector2Int)
        {
            GameObject gameObject = new GameObject("Player", "Player");
            gameObject.Position = vector2Int;
            gameObject.AddComponent<Mesh>();
            return gameObject;
        }
    }
}