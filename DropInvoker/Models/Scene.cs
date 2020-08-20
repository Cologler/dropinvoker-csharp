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
                    this.Slots.Add(new LauncherViewModel(slot));
                }
                else
                {
                    this.Slots.Add(LauncherViewModel.Empty);
                }
            }
        }

        public List<LauncherViewModel> Slots { get; } = new List<LauncherViewModel>();
    }
}
