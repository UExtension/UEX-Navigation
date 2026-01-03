using Cysharp.Threading.Tasks;
using UExtension.Navigation;
using UExtension.Navigation.Route;
using UExtension.Navigation.Route.Tab;
using UnityEngine.UIElements;

namespace UI.Atoms
{
    public class RouteUI : VisualElement
    {
        public VisualElement Background { get; }
        public Label Label { get; }
        public IRoute Route { get; }

        public RouteUI(IRoute route)
        {
            Route = route;

            AddToClassList("route");

            if (route.IsActive && !route.HasNext())
            {
                AddToClassList("route--selected");
            }

            if (route.IsActive)
            {
                AddToClassList("route--active");
            }

            if (!route.HasNext())
            {
                AddToClassList("route--tip");
            }

            Background = new VisualElement();
            Background.AddToClassList("route__background");

            Label = new Label(route.Name);
            Label.AddToClassList("route__label");

            Add(Background);
            Add(Label);

            SetupManipulator();
        }

        private void SetupManipulator()
        {
            this.AddManipulator(new ContextualMenuManipulator(evt =>
            {
                if (Route.IsActive)
                {
                    evt.menu.AppendAction("Pop", HandlePop, DropdownMenuAction.AlwaysEnabled);
                    evt.menu.AppendAction("Navigate", HandleNavigate, DropdownMenuAction.AlwaysEnabled);
                }

                if (Route.Previous is TabRoute)
                {
                    evt.menu.AppendAction("Set active tab", HandleSetTab, DropdownMenuAction.AlwaysEnabled);
                }

                evt.menu.AppendAction("Root", HandleRoot, DropdownMenuAction.AlwaysEnabled);
            }));
        }

        private void HandlePop(DropdownMenuAction _)
        {
            NavigationService.Pop(Route).Forget();
        }

        private void HandleNavigate(DropdownMenuAction _)
        {
            NavigationService.Navigate(Route).Forget();
        }

        private void HandleSetTab(DropdownMenuAction _)
        {
            if (Route.Previous is TabRoute tabRoute)
                NavigationService.SetTab(tabRoute, Route).Forget();
        }

        private void HandleRoot(DropdownMenuAction _)
        {
            NavigationService.Root(Route).Forget();
        }
    }
}