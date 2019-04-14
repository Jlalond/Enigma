using System;
using System.Runtime.CompilerServices;
using Assets.Enigma.Enigma.Core.Networking.Serialization.SerializationModel;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Assets.Enigma.Enigma.Core.Networking
{
    public sealed class NetworkEntity : MonoBehaviour
    {
        public Guid Guid { get; set; }
        private readonly ConnectionHandler _connectHandlerInstance = ConnectionHandler.ConnectionHandlerInstance;
        private UpdateDictionary _updateDictionary;


        #region Networking Methods and Awake
        private void Awake()
        {
            _updateDictionary = new UpdateDictionary(this);
            AddThisComponentAsListener();
            SendSync(this);
        }

        private void AddThisComponentAsListener()
        {
            while (_connectHandlerInstance.TryAddListener(this) != true) ;
        }

        public void SendAsync(object obj)
        {
            var message = BuildNetworkMessage(obj);
            _connectHandlerInstance.SendUdpUpdate(message);
        }

        public void SendSync(object obj)
        {
            var message = BuildNetworkMessage(obj);
            _connectHandlerInstance.SendTcpUpdate(message);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private NetworkWrapper BuildNetworkMessage(object obj)
        {
            return new NetworkWrapper(this.Guid, new[] {obj});
        }

        /// <summary>
        /// Method that updates component of type if component is ready to be updated
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        public void TryGetUpdates<T>(T value) where T : NetworkedComponent
        {
            _updateDictionary.UpdateIfNewUpdate(value);
        }

        public void SafeAdd(Type type, JObject jObject)
        {
            _updateDictionary.SafeAdd(type, jObject);
        }
#endregion
    }
}
