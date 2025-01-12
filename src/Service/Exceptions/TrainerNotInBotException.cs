namespace Service.Exceptions
{
    public class TrainerNotInBotException : Exception
    {
        public TrainerNotInBotException(string message) : base(message) { }
    }
}
