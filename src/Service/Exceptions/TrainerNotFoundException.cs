namespace Service.Exceptions
{
    public class TrainerNotFoundException : Exception
    {
        public TrainerNotFoundException(string message) : base(message) { }
    }
}
