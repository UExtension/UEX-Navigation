using Cysharp.Threading.Tasks;
using UExtension.Navigation.Route;
using UExtension.Navigation.Route.Tab;
using UnityEngine.UIElements;

namespace UExtension.Navigation.Editor.UI.Atoms
{
    public class RouteAtom : VisualElement
    {
        private IRoute _route;

        public IRoute Route
        {
            get => _route;
            set
            {
                _route = value;

                if (Route.IsActive && !Route.HasNext()) AddToClassList("route--selected");

                if (Route.IsActive) AddToClassList("route--active");

                if (!Route.HasNext()) AddToClassList("route--tip");

                Label.text = Route.Name;
            }
        }

        public VisualElement Background { get; }
        public Label Label { get; }

        public RouteAtom()
        {
            AddToClassList("route");

            Background = new VisualElement();
            Background.AddToClassList("route__background");

            Label = new Label();
            Label.AddToClassList("route__label");

            Add(Background);
            Add(Label);

            SetupManipulator();
        }

        public RouteAtom(IRoute route) : this() { Route = route; }

        private void SetupManipulator() =>
            this.AddManipulator(new ContextualMenuManipulator(evt =>
            {
                if (Route.IsActive)
                {
                    evt.menu.AppendAction("Pop", HandlePop, DropdownMenuAction.AlwaysEnabled);
                    evt.menu.AppendAction("Navigate", HandleNavigate, DropdownMenuAction.AlwaysEnabled);
                }

                if (Route.Previous is TabRoute) evt.menu.AppendAction("Set active tab", HandleSetTab, DropdownMenuAction.AlwaysEnabled);

                evt.menu.AppendAction("Root", HandleRoot, DropdownMenuAction.AlwaysEnabled);
            }));

        private void HandlePop(DropdownMenuAction _) => NavigationService.combinedRouter.Pop(Route).Load().Forget();

        private void HandleNavigate(DropdownMenuAction _) => NavigationService.combinedRouter.Navigate(Route).Load().Forget();

        private void HandleSetTab(DropdownMenuAction _)
        {
            if (Route.Previous is TabRoute tabRoute) NavigationService.combinedRouter.SetTab(tabRoute, Route).Load().Forget();
        }

        private void HandleRoot(DropdownMenuAction _) => NavigationService.combinedRouter.Root(Route).Load().Forget();
    }
}