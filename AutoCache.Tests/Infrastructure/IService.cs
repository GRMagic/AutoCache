namespace AutoCache.Tests.Infrastructure
{
    public interface IService
    {
        public int InstanceId();
        public DateTime DoSomethingWithoutCache(int number);
        public DateTime DoSomethingWithCache(int number);
    }
}
