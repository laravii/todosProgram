using Bogus;

namespace Todo.Tests.Builders
{
    public abstract class BaseBuilder<T> where T : class
    {
        protected Faker<T> Faker = new("pt_BR");
        public abstract T Build();
    }
}
