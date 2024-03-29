﻿using AutoMapper;

using FluentValidation;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;

namespace StudentEnrollment.API.Endpoints;

public static class CourseEndpoints
{
    public static void MapCourseEndpoints(this IEndpointRouteBuilder routes)
    {
        var route = routes.MapGroup("/api/courses")
                                        .EnableOpenApiWithAuthentication()
                                        .WithTags(nameof(Course));

        route.MapGet("/", async (ICourseRepository repository, IMapper mapper) =>
        {
            var courses = await repository.GetAllAsync();
            return mapper.Map<List<CourseDto>>(courses);
        })
        .AllowAnonymous()
        .WithName("GetAllCourses")
        .WithOpenApi();

        route.MapGet("/{id}", async Task<Results<Ok<CourseDto>, NotFound>> (int id, ICourseRepository repository, IMapper mapper) =>
        {
            return await repository.GetAsync(id)
                is Course model
                    ? TypedResults.Ok(mapper.Map<CourseDto>(model))
                    : TypedResults.NotFound();
        })
        .AllowAnonymous()
        .WithName("GetCourseById")
        .WithOpenApi();

        route.MapGet("/detail/{id}", async Task<Results<Ok<CourseDetailsDto>, NotFound>> (int id, ICourseRepository repository, IMapper mapper) =>
        {
            return await repository.GetStudentList(id)
                is Course model
                    ? TypedResults.Ok(mapper.Map<CourseDetailsDto>(model))
                    : TypedResults.NotFound();
        })

        .WithName("GetCourseDetail")
        .WithOpenApi();

        route.MapPut("/{id}", async Task<Results<NotFound, NoContent, BadRequest<IDictionary<string, string[]>>>> (int id, CourseDto course, ICourseRepository repository, IMapper mapper, IValidator<CourseDto> validator) =>
        {
            var modelState = await validator.ValidateAsync(course);

            if (!modelState.IsValid)
                return TypedResults.BadRequest(modelState.ToDictionary());

            var foundModel = await repository.GetAsync(id);

            if (foundModel is null)
                return TypedResults.NotFound();

            mapper.Map(course, foundModel);
            await repository.UpdateAsync(foundModel);

            return TypedResults.NoContent();
        })
        .WithName("UpdateCourse")
        .WithOpenApi();

        route.MapPost("/", async (CreateCourseDto course, ICourseRepository repository, IMapper mapper, IValidator<CreateCourseDto> validator) =>
        {
            var modelState = await validator.ValidateAsync(course);

            if (!modelState.IsValid)
                return Results.BadRequest(modelState.ToDictionary());

            var model = mapper.Map<Course>(course);
            await repository.AddAsync(model);
            return TypedResults.Created($"/api/courses/{model.Id}", model);
        })
        .WithName("CreateCourse")
        .WithOpenApi();

        route.MapDelete("/{id}", [Authorize(Roles = "Administrator")] async Task<Results<Ok<Course>, NoContent, NotFound>> (int id, ICourseRepository repository) =>
        {
            return await repository.DeleteAsync(id) ? TypedResults.NoContent() : TypedResults.NotFound();
        })
        .WithName("DeleteCourse")
        .WithOpenApi();
    }
}
