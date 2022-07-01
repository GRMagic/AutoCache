namespace AutoCache.Tests.Infrastructure
{
    public class Service : IService
    {
        [Cache(Seconds = 1)]
        public DateTime DoSomethingWithCache(int number) => DoSomethingSlow(number);

        public DateTime DoSomethingWithoutCache(int number) => DoSomethingSlow(number);

        public int InstanceId() => GetHashCode();

        private DateTime DoSomethingSlow(int number)
        {
            //Thread.Sleep(1000);
            return DateTime.UtcNow.AddDays(number);
        }
    }
}
