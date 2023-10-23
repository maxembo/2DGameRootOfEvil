namespace Scripts.Core.Common
{
    public abstract class Command
    {
        public abstract void Execute();

        public virtual void ExecuteByValue(float value) { }
    }
}