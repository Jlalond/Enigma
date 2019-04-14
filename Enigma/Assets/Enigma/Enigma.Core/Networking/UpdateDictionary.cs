using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Enigma.Enigma.Core.Networking.Serialization;
using Newtonsoft.Json.Linq;

namespace Assets.Enigma.Enigma.Core.Networking
{
    internal class UpdateDictionary
    {
        private readonly Dictionary<Type, UpdateEvent> _networkedComponentsInGameObject;
        private readonly NetworkEntity _parent;
        private readonly object _lock = new object();

        public UpdateDictionary(NetworkEntity parent)
        {
            _parent = parent;
            _networkedComponentsInGameObject = parent.GetComponents(typeof(NetworkedComponent))
                                                     .ToDictionary(component => component.GetType(),
                                                                   value => new UpdateEvent());
        }

        internal void SafeAdd(Type type, JObject jObject)
        {
            // we have a local lock here
            lock (_lock)
            {
                if (!_networkedComponentsInGameObject.ContainsKey(type))
                {
                    var newComponent = _parent.gameObject.AddComponent(type);
                    _networkedComponentsInGameObject.Add(type, new UpdateEvent());
                    ComponentUpdater.UpdateObjectOfType((NetworkedComponent) newComponent, jObject);
                }
                else
                {
                    _networkedComponentsInGameObject[type].AddNewestMessage(jObject);
                }
            }
        }

        internal void UpdateIfNewUpdate<T>(T value) where T : NetworkedComponent
        {
            if (_networkedComponentsInGameObject.ContainsKey(typeof(T)) &&
                _networkedComponentsInGameObject[typeof(T)].HasBeenUpdatedSinceLastGet)
            {
                _networkedComponentsInGameObject[typeof(T)].Update(value);
            }
        }
    }
}
