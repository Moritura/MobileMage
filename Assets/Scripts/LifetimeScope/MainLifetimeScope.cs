using Mage.Draw;
using VContainer;
using VContainer.Unity;

public class MainLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<DrawService>(Lifetime.Singleton)
            .WithParameter("maxCountLine", 10)
            .WithParameter("timeBetweenLines", 1f)
            .AsImplementedInterfaces();
    }
}
