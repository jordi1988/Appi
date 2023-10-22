namespace FileDemo
{
    public class DummyService
    {
        private readonly DateTimeOffset _creationTime;

        public DummyService()
        {
            _creationTime = DateTime.Now;
        }

        public string GetDummyServiceCreationTime() 
            => $"The dummy service was created at {_creationTime}";
    }
}
