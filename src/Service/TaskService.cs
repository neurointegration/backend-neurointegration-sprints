﻿using Data.Repositories;
using Data;
using Service.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<TaskResponse?> GetTaskByIdAsync(Guid taskId)
        {
            var task = await _taskRepository.GetTaskByIdAsync(taskId);
            if (task == null)
                return null;

            return MapToTaskResponse(task);
        }

        public async Task<IList<TaskResponse>> GetTasksByProjectIdAsync(Guid projectId)
        {
            var tasks = await _taskRepository.GetTasksByProjectIdAsync(projectId);

            return tasks.Select(MapToTaskResponse).ToList();
        }

        public async Task<TaskResponse> CreateTaskAsync(CreateTaskRequest request)
        {
            var task = new NeuroTask
            {
                Id = Guid.NewGuid(),
                ProjectId = request.ProjectId,
                CreatedAt = DateTime.UtcNow,
                Title = request.Title,
                SectionName = request.SectionName,
                PlanningTimes = request.PlanningTimes,
                FactTimes = request.FactTimes
            };

            var createdTask = await _taskRepository.CreateTaskAsync(task);
            return MapToTaskResponse(createdTask);
        }

        public async Task<TaskResponse> UpdateTaskAsync(UpdateTaskRequest request)
        {
            var existingTask = await _taskRepository.GetTaskByIdAsync(request.Id);
            if (existingTask == null)
                throw new KeyNotFoundException("Task not found");

            existingTask.Title = request.Title ?? existingTask.Title;
            existingTask.SectionName = request.SectionName ?? existingTask.SectionName;
            existingTask.PlanningTimes = request.PlanningTimes ?? existingTask.PlanningTimes;
            existingTask.FactTimes = request.FactTimes ?? existingTask.FactTimes;

            var updatedTask = await _taskRepository.UpdateTaskAsync(existingTask);

            return MapToTaskResponse(updatedTask);
        }

        public async Task DeleteTaskAsync(Guid taskId)
        {
            await _taskRepository.DeleteTaskAsync(taskId);
        }

        private TaskResponse MapToTaskResponse(NeuroTask task)
        {
            return new TaskResponse
            {
                Id = task.Id,
                CreatedAt = task.CreatedAt,
                Title = task.Title,
                SectionName = task.SectionName,
                PlanningTimes = task.PlanningTimes,
                FactTimes = task.FactTimes
            };
        }

        private Dictionary<String, T>? ConvertJsonToDictionary<T>(string? json)
        {
            if (string.IsNullOrEmpty(json))
                return null;

            return System.Text.Json.JsonSerializer.Deserialize<Dictionary<String, T>>(json);
        }

        private string? ConvertDictionaryToJson<T>(Dictionary<String, T>? dictionary)
        {
            if (dictionary == null)
                return null;

            return System.Text.Json.JsonSerializer.Serialize(dictionary);
        }
    }
}
