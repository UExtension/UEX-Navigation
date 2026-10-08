using UExtension.Navigation.Router;
using UExtension.Navigation.Router.Combined;

namespace UExtension.Navigation.RouterFactory.Combined
{
    public class CombinedRouterFactory : AbstractRouterFactory<CombinedRouter>
    {
        public override string Name { get; set; } = CombinedRouter.DEFAULT_NAME;

        public override CombinedRouter Create() => CreateTyped();

        public CombinedRouter CreateTyped() => new();
    }
}