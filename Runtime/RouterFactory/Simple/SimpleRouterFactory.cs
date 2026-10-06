using UExtension.Navigation.Router;
using UExtension.Navigation.Router.Simple;

namespace UExtension.Navigation.RouterFactory.Simple
{
    public class SimpleRouterFactory : AbstractRouterFactory<SimpleRouter>
    {
        public override string Name { get; set; } = SimpleRouter.DEFAULT_NAME;

        public override SimpleRouter Create() => CreateTyped();

        public SimpleRouter CreateTyped() => new();
    }
}