using DropInvoker.Models.Configurations;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace DropInvoker.Models
{
    class Scene
    {
        private const int SlotCount = 9;

        public Scene(SceneJson sceneInfo)
        {
            for (var i = 0; i < SlotCount; i++)
            {
                if (sceneInfo.Slots != null &&
                    sceneInfo.Slots!.TryGetValue(i.ToString(), out var slot) && slot != null)
                {
                    this.Slots.Add(new Invoker(slot));
                }
                else
                {
                    this.Slots.Add(Invoker.Empty);
                }
            }
        }

        public List<Invoker> Slots { get; } = new List<Invoker>();
    }
}
