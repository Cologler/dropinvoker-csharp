using System.Diagnostics;
using System.Windows;

using DropInvoker.Models.Configurations;

using RLauncher.Exceptions;

namespace DropInvoker.Models
{
    class Scene
    {
        private const int SlotCount = 9;

        public Scene(SceneJson sceneInfo)
        {
            var slots = sceneInfo.Slots ?? [];

            foreach (var slot in slots.Concat(Enumerable.Repeat<string?>(null, SlotCount)).Take(SlotCount))
            {
                try
                {
                    this.Slots.Add(slot is null ? CommandViewModel.Empty : new CommandViewModel(slot));
                }
                catch (InvalidRLauncherConfigurationFileException exc)
                {
                    this.Slots.Add(CommandViewModel.Empty);
                    var title = $"Failed to parse {exc.FileType}";
                    var message = $"Unable parse from {exc.FileFullPath} to {exc.FileType}\nSource:\n{exc.FileContent}";
                    MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            Debug.Assert(this.Slots.Count == SlotCount);
        }

        public List<CommandViewModel> Slots { get; } = [];
    }
}
