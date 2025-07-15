using ListKeeper.ApiService.Models.ViewModels;
using ListKeeperWebApi.WebApi.Models;
using ListKeeperWebApi.WebApi.Models.ViewModels;

namespace ListKeeper.ApiService.Models.Extensions
{
    public static class NoteMappingExtensions
    {
        public static NoteViewModel? ToViewModel(this Note? note)
        {
            if (note == null)
            {
                return null;
            }

            return new NoteViewModel
            {
                Id = note.Id,
                Title = note.Title,
                Content = note.Content,
                DueDate = note.DueDate,
                IsCompleted = note.IsCompleted,
                Color = note.Color

            };
        }

        public static Note? ToDomain(this NoteViewModel? viewModel)
        {
            if (viewModel == null)
            {
                return null;
            }

            return new Note
            {
                Id = viewModel.Id,
                Title = viewModel.Title,
                Content = viewModel.Content,
                DueDate = viewModel.DueDate,
                IsCompleted = viewModel.IsCompleted,
                Color = viewModel.Color

            };
        }
    }
}
