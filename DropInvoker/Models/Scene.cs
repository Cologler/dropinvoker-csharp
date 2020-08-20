using DropInvoker.Models.Configurations;

using System;
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
            var slots = sceneInfo.Slots ?? Array.Empty<string>();
            foreach (var slot in slots.Take(9))
            {
                this.Slots.Add(slot is null ? LauncherViewModel.Empty : new LauncherViewModel(slot));
            }
        }

        public List<LauncherViewModel> Slots { get; } = new List<LauncherViewModel>();
    }
}
