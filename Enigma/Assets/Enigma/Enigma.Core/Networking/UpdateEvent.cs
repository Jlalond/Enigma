using Assets.Enigma.Enigma.Core.Networking.Serialization;
using Newtonsoft.Json.Linq;

namespace Assets.Enigma.Enigma.Core.Networking
{
    internal class UpdateEvent
    {
        private JObject _mostRecentMessage;
        public bool HasBeenUpdatedSinceLastGet { get; private set; }

        public UpdateEvent()
        {
            HasBeenUpdatedSinceLastGet = false;
        }

        public void AddNewestMessage(JObject jObject)
        {
            HasBeenUpdatedSinceLastGet = true;
            _mostRecentMessage = jObject;
        }

        public void Update(NetworkedComponent component)
        {
            HasBeenUpdatedSinceLastGet = false;
            ComponentUpdater.UpdateObjectOfType(component, _mostRecentMessage);
        }
    }
}
