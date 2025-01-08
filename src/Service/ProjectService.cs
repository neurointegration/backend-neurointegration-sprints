using Data.Repositories;
using Data;
using Service.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<ProjectResponse?> GetProjectByIdAsync(Guid projectId)
        {
            var project = await _projectRepository.GetProjectByIdAsync(projectId);
            if (project == null)
                return null;

            return MapToProjectResponse(project);
        }

        public async Task<IList<ProjectResponse>> GetProjectsBySprintIdAsync(Guid sprintId)
        {
            var projects = await _projectRepository.GetProjectsBySprintIdAsync(sprintId);

            return projects.Select(MapToProjectResponse).ToList();
        }

        public async Task<ProjectResponse> CreateProjectAsync(CreateProjectRequest request)
        {
            var project = new Project
            {
                Id = Guid.NewGuid(),
                SprintId = request.SprintId,
                Title = request.Title,
                SectionName = request.SectionName,
                TotalTime = request.TotalTime != null ? SerializePlanningTimeDto(request.TotalTime) : null,
                PlanningTimes = ConvertDictionaryToJson(request.PlanningTimes),
                FactTimes = ConvertDictionaryToJson(request.FactTimes)
            };

            var createdProject = await _projectRepository.CreateProjectAsync(project);
            return MapToProjectResponse(createdProject);
        }

        public async Task<ProjectResponse> UpdateProjectAsync(UpdateProjectRequest request)
        {
            var existingProject = await _projectRepository.GetProjectByIdAsync(request.Id);
            if (existingProject == null)
                throw new KeyNotFoundException("Project not found");

            existingProject.Title = request.Title ?? existingProject.Title;
            existingProject.SectionName = request.SectionName ?? existingProject.SectionName;
            existingProject.TotalTime = request.TotalTime != null ? SerializePlanningTimeDto(request.TotalTime) : existingProject.TotalTime;
            existingProject.PlanningTimes = ConvertDictionaryToJson(request.PlanningTimes) ?? existingProject.PlanningTimes;
            existingProject.FactTimes = ConvertDictionaryToJson(request.FactTimes) ?? existingProject.FactTimes;

            var updatedProject = await _projectRepository.UpdateProjectAsync(existingProject);

            return MapToProjectResponse(updatedProject);
        }

        private ProjectResponse MapToProjectResponse(Project project)
        {
            return new ProjectResponse
            {
                Id = project.Id,
                Title = project.Title,
                SectionName = project.SectionName,
                TotalTime = string.IsNullOrEmpty(project.TotalTime) ? null : DeserializePlanningTimeDto(project.TotalTime),
                PlanningTimes = ConvertJsonToDictionary<PlanningTimeDto>(project.PlanningTimes),
                FactTimes = ConvertJsonToDictionary<FactTimeDto>(project.FactTimes)
            };
        }

        private Dictionary<int, T>? ConvertJsonToDictionary<T>(string? json)
        {
            if (string.IsNullOrEmpty(json))
                return null;

            return System.Text.Json.JsonSerializer.Deserialize<Dictionary<int, T>>(json);
        }

        private string? ConvertDictionaryToJson<T>(Dictionary<int, T>? dictionary)
        {
            if (dictionary == null)
                return null;

            return System.Text.Json.JsonSerializer.Serialize(dictionary);
        }

        private string SerializePlanningTimeDto(PlanningTimeDto dto)
        {
            return System.Text.Json.JsonSerializer.Serialize(dto);
        }

        private PlanningTimeDto DeserializePlanningTimeDto(string json)
        {
            return System.Text.Json.JsonSerializer.Deserialize<PlanningTimeDto>(json)!;
        }
    }
}
