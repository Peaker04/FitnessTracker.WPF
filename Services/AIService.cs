using FitnessTracker.WPF.Models;
using FitnessTracker.WPF.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessTracker.WPF.Services
{
    public class AIService : IAIService
    {
        private readonly IConfiguration _configuration;
        private readonly RestClient _client;
        private string _apiKey;
        private string _model;

        public bool IsEnabled { get; set; }

        public AIService(IConfiguration configuration)
        {
            _configuration = configuration;
            IsEnabled = _configuration.GetValue<bool>("AI:Enabled");
            _apiKey = _configuration.GetValue<string>("AI:ApiKey");
            _model = _configuration.GetValue<string>("AI:Model");

            _client = new RestClient("https://api.openai.com/v1");
        }

        public async Task<string> GenerateWorkoutPlanAsync(FitnessGoal goal, PreferredEnvironment environment, int daysPerWeek)
        {
            if (!IsEnabled) return GetFallbackWorkoutPlanJSON(goal, environment, daysPerWeek);

            try
            {
                // Yêu cầu AI trả về JSON
                var prompt = $@"Create a {daysPerWeek}-day workout plan for goal: {goal}, environment: {environment}. 
                                Return ONLY a valid JSON array of objects. Do not include markdown formatting or extra text.
                                JSON format: [{{ ""Day"": 1, ""ExerciseName"": ""Name"", ""Sets"": 3, ""Reps"": ""10-12"" }}]";

                return await CallOpenAIAsync(prompt);
            }
            catch
            {
                return GetFallbackWorkoutPlanJSON(goal, environment, daysPerWeek);
            }
        }

        public async Task<string> GenerateNutritionAdviceAsync(WorkoutSession session, User user)
        {
            if (!IsEnabled)
                return GetFallbackNutritionAdvice(session, user);

            try
            {
                var prompt = $@"User just completed a {session.TotalDurationMinutes} minute workout.
                                User goal: {user.FitnessGoal}, Weight: {user.Weight}kg, Height: {user.Height}cm.
                                Provide post-workout nutrition advice including calories and macros.";

                var response = await CallOpenAIAsync(prompt);
                return response;
            }
            catch
            {
                return GetFallbackNutritionAdvice(session, user);
            }
        }

        public async Task<IEnumerable<Exercise>> SuggestExercisesAsync(
            FitnessGoal goal,
            PreferredEnvironment environment)
        {
            // This would query database based on goal and environment
            // Placeholder implementation
            return new List<Exercise>();
        }

        private async Task<string> CallOpenAIAsync(string prompt)
        {
            var request = new RestRequest("chat/completions", Method.Post);
            request.AddHeader("Authorization", $"Bearer {_apiKey}");
            request.AddHeader("Content-Type", "application/json");

            var body = new
            {
                model = _model,
                messages = new[]
                {
                    new { role = "system", content = "You are a fitness and nutrition expert." },
                    new { role = "user", content = prompt }
                },
                max_tokens = 500
            };

            request.AddJsonBody(body);

            var response = await _client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                dynamic result = JsonConvert.DeserializeObject(response.Content);
                return result.choices[0].message.content.ToString();
            }

            throw new Exception($"AI API Error: {response.ErrorMessage}");
        }

        // Fallback methods khi AI không available
        private string GetFallbackWorkoutPlanJSON(FitnessGoal goal, PreferredEnvironment environment, int days)
        {
            var mockData = new List<object>();
            for (int i = 1; i <= days; i++)
            {
                mockData.Add(new { Day = i, ExerciseName = $"Push Up (Day {i})", Sets = 3, Reps = "10" });
                mockData.Add(new { Day = i, ExerciseName = $"Squat (Day {i})", Sets = 3, Reps = "12" });
            }
            return JsonConvert.SerializeObject(mockData);
        }

        private string GetFallbackNutritionAdvice(WorkoutSession session, User user)
        {
            var calories = (session.TotalCaloriesBurned ?? 300) * 1.5;
            return $"Post-workout nutrition:\n" +
                   $"- Calories: ~{calories} kcal\n" +
                   $"- Protein: 30-40g\n" +
                   $"- Carbs: 50-60g\n" +
                   $"- Drink plenty of water!";
        }
    }
}
