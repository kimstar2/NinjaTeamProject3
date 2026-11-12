namespace _Members.KJY._01.Scripts.Agent
{
    public interface IMoverPlus : IMoveable
    {
        void StopImmediately(bool stopX = true, bool stopY = true);
    }
}