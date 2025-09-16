using System;
using System.Collections.Generic;

namespace Services.SceneManagement
{
    public class SceneResources
    {
        private readonly List<Action> _objectsToRelease = new();
        public IReadOnlyList<Action> ObjectToRelease => _objectsToRelease;

        public void AddToListDelegate(Action action) => _objectsToRelease.Add(action);

        public void ClearObject() => _objectsToRelease.Clear();
    }
}