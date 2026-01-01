using System.Collections.Generic;
using UExtension.SceneLoader.ScriptableObjects;

namespace UExtension.Navigation.Route
{
    public interface IRoute
    {
        /// <summary>
        /// The unique name of the route, used as an identifier.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The scenes to be loaded with this route.
        /// </summary>
        public List<SceneContainer> Scenes { get; }

        /// <summary>
        /// The scene to set active in Unity with this route.
        /// </summary>
        public SceneContainer ActiveScene { get; }

        /// <summary>
        /// The scene to set active in Unity's Baking Set with this route. 
        /// </summary>
        public SceneContainer BakingSetActiveScene { get; }

        /// <summary>
        /// Gets or sets the preceding <see cref="IRoute"/> of this route.
        /// </summary>
        /// <remarks>
        /// This value is null if there is no preceding route. Use <see cref="HasPrevious"/> to check for existence.
        /// </remarks>
        public IRoute Previous { get; set; }

        /// <summary>
        /// Gets or sets the succeeding <see cref="IRoute"/> of this route.
        /// </summary>
        ///  /// <remarks>
        /// This value is null if there is no succeeding route. Use <see cref="HasNext"/> to check for existence.
        /// </remarks>
        public IRoute Next { get; set; }

        /// <summary>
        /// Determines whether this route is active
        /// </summary>
        /// <returns><see langword="true"/> if this route is active; otherwise, <see langword="false"/>.</returns>
        bool IsSelfActive { get; internal set; }

        /// <summary>
        /// Determines whether this route is active in the stack
        /// </summary>
        /// <returns><see langword="true"/> if this route is active in the stack; otherwise, <see langword="false"/>.</returns>
        bool IsActive { get; }
      
        /// <summary>
        /// Adds the given route as the tip of the current route stack.
        /// </summary>
        /// <param name="route">The route to be added.</param>
        /// <returns>The updated route tip after the operation.</returns>
        public IRoute Push(IRoute route);

        /// <summary>
        /// Removes the tip of the current route stack.
        /// </summary>
        /// <returns>The updated route tip after the operation.</returns>
        public IRoute Pop();

        /// <summary>
        /// Navigates backwards to the specified route, pushes it if it does not exist in history.
        /// </summary>
        /// <param name="route">The route to navigate to.</param>
        /// <returns>The updated route tip after the operation.</returns>
        public IRoute Navigate(IRoute route);

        /// <summary>
        /// Retrieves the root route from the current route stack.
        /// </summary>
        public IRoute GetRoot();

        /// <summary>
        /// Retrieves the tip of the current route stack.
        /// </summary>
        public IRoute GetTip();

        /// <summary>
        /// Searches for a specified route starting from the current route backward through any previous routes.
        /// </summary>
        /// <param name="route">The route to search for.</param>
        /// <returns>The matching route if found; otherwise, null.</returns>
        public IRoute Search(IRoute route);

        /// <summary>
        /// Determines whether a preceding route exists.
        /// </summary>
        /// <returns><see langword="true"/> if a preceding route exists; otherwise, <see langword="false"/>.</returns>
        bool HasPrevious();

        /// <summary>
        /// Determines whether a succeeding route exists.
        /// </summary>
        /// <returns><see langword="true"/> if a succeeding route exists; otherwise, <see langword="false"/>.</returns>
        bool HasNext();

        /// <summary>
        /// Returns a string representation of the current route.
        /// </summary>
        /// <returns>A string representation of the current route.</returns>
        public string ToString();
    }
}