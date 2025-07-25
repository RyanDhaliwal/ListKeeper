﻿
using global::ListKeeperWebApi.WebApi.Data;
using ListKeeper.ApiService.Models;
using ListKeeper.ApiService.Models.Extensions;
using ListKeeper.ApiService.Models.ViewModels;
using ListKeeper.ApiService.Services.ListKeeperWebApi.WebApi.Services;

namespace ListKeeperWebApi.WebApi.Services
{
    /// <summary>
    /// This is the "Service Layer" for note-related operations.
    /// Its job is to contain the core business logic. It acts as a middle-man between the API endpoints (the "presentation" layer)
    /// and the repository (the "data access" layer). This separation makes the code cleaner and easier to manage.
    /// </summary>
    public class NoteService : INoteService
    {
        // These are private fields to hold the "dependencies" that this service needs to do its job.
        private readonly INoteRepository _repo;
        private readonly ILogger<NoteService> _logger;
        private readonly IConfiguration _config; // Added to access appsettings.json

        /// <summary>
        /// This is the constructor. When an instance of `NoteService` is created,
        /// the dependency injection system (configured in Program.cs) automatically provides
        /// an instance of `INoteRepository`, `ILogger<NoteService>`, and `IConfiguration`.
        /// </summary>
        public NoteService(INoteRepository repo, ILogger<NoteService> logger, IConfiguration config)
        {
            _repo = repo;
            _logger = logger;
            _config = config; // Store the injected configuration service.
        }


        /// <summary>
        /// Creates a new note in the system.
        /// </summary>
        /// <param name="createNoteVm">A view model containing the new note's information.</param>
        public async Task<NoteViewModel?> CreateNoteAsync(NoteViewModel createNoteVm)
        {
            if (createNoteVm == null) return null;

            // Map the data from the view model to a 'Note' domain entity, which is what the database stores.
            var note = new Note
            {
                Title = createNoteVm.Title,
                DueDate = createNoteVm.DueDate,
                Color = createNoteVm.Color,
                Content = createNoteVm.Content,
                Id = createNoteVm.Id,
                IsCompleted = createNoteVm.IsCompleted
            };


            // business rules checking

            // Delegate the actual database "add" operation to the repository.
            var createdNote = await _repo.AddAsync(note);
            return createdNote?.ToViewModel();
        }


        /// <summary>
        /// Updates an existing note's information.
        /// </summary>
        public async Task<NoteViewModel?> UpdateNoteAsync(NoteViewModel noteVm)
        {
            if (noteVm == null) return null;

            // First, retrieve the existing note from the database.
            var note = await _repo.GetByIdAsync(noteVm.Id);
            if (note == null) return null; // Can't update a note that doesn't exist.

            note.Title = noteVm.Title;
            note.DueDate = noteVm.DueDate;
            note.Color = noteVm.Color;
            note.Content = noteVm.Content;
            note.Id = noteVm.Id;
            note.IsCompleted = noteVm.IsCompleted;

            // Note: We deliberately do not update the password here. Password changes
            // should be handled in a separate, dedicated "ChangePassword" method for security.

            var updatedNote = await _repo.Update(note);
            return updatedNote?.ToViewModel();
        }

        /// <summary>
        /// Retrieves all notes from the system.
        /// </summary>
        public async Task<IEnumerable<NoteViewModel>> GetAllNotesAsync()
        {
            var notes = await _repo.GetAllAsync();
            // The `?.` is a null-conditional operator. If 'notes' is null, it won't throw an error.
            // The `??` is a null-coalescing operator.
            // If the result of the Select is null, it returns an empty list instead.
            return notes?.Select(u => u.ToViewModel()) ?? Enumerable.Empty<NoteViewModel>();
        }

        /// <summary>
        /// Deletes a note by their ID. This is an overload.
        /// </summary>
        public async Task<bool> DeleteNoteAsync(int id)
        {
            return await _repo.Delete(id);
        }

        /// <summary>
        /// Deletes a note based on a view model object. This is another overload.
        /// </summary>
        public async Task<bool> DeleteNoteAsync(NoteViewModel noteVm)
        {
            if (noteVm == null) return false;
            // The `ToDomain()` extension method converts the view model back to a database entity.
            var note = noteVm.ToDomain();
            return await _repo.Delete(note);
        }

        /// <summary>
        /// Retrieves a single note by their ID.
        /// </summary>
        public async Task<NoteViewModel?> GetNoteByIdAsync(int id)
        {
            var note = await _repo.GetByIdAsync(id);
            return note?.ToViewModel();
        }


    }
}

