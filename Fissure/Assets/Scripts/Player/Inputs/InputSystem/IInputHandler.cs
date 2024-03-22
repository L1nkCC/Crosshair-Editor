namespace Input
{
    public interface IInputHandler
    {
        public abstract bool InputsEnabled { get; }
        public abstract void DisableInputs();
        public abstract void EnableInputs();

    }
}


