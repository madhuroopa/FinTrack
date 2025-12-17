using api.Models;
using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.AzureOpenAI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel.Connectors.OpenAI;

[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    private readonly string _azuresearchServiceEndpoint;
    private readonly string _azuresearchIndexName;
    private readonly string _azurequeryKey;
    private readonly string _openAIEndpoint;
    private readonly string _openAIDeployment;
    private readonly string _openAIKey;

    public ChatController(IConfiguration config)
    {
        // Load configuration values from User Secrets
        _azuresearchServiceEndpoint = config["azuresearchServiceEndpoint"];
        _azuresearchIndexName = config["azuresearchIndexName"];
        _azurequeryKey = config["azurequeryKey"];
        _openAIEndpoint = config["Azure:OpenAI:Endpoint"];
        _openAIDeployment = config["Azure:OpenAI:ModelName"];
        _openAIKey = config["Azure:OpenAI:ApiKey"];
    }

    private async Task<List<Article>> GetSearchResultsAsync(string query)
    {
        // Create the credential and search client
        AzureKeyCredential credential = new AzureKeyCredential(_azurequeryKey);
        var client = new SearchClient(new Uri(_azuresearchServiceEndpoint), _azuresearchIndexName, credential);

        // Define semantic search options
        var options = new SearchOptions
        {
            QueryType = SearchQueryType.Semantic,
            SemanticSearch = new SemanticSearchOptions
            {
                SemanticConfigurationName = "semantic",  // Replace with your semantic config name
                QueryCaption = new(QueryCaptionType.Extractive),
                QueryAnswer = new(QueryAnswerType.Extractive),
                SemanticQuery = query
            },
            Size = 5  // Limit the number of results to 5
        };

        // Perform the search query asynchronously
        SearchResults<Article> response = await client.SearchAsync<Article>(query, options);

        // Return the search results as List<Article>
        return response.GetResults().Select(r => r.Document).ToList();
    }

    [HttpPost("query")]
    public async Task<IActionResult> Post([FromBody] ChatRequest request)
    {
        if (request == null || string.IsNullOrEmpty(request.Query))
            return BadRequest("Query cannot be empty.");

        // Fetch relevant articles from Azure AI Search
        List<Article> searchResults = await GetSearchResultsAsync(request.Query);

        if (!searchResults.Any())
            return NotFound("No articles found for the given query.");

        // Prepare search results for the prompt
        string searchResultsText = string.Join("\n\n", searchResults.Select(a =>
            $"**Title**: {a.title}\n**Description**: {a.description}\n**Content**: {a.content}\n**URL**: {a.url}"));

        // Create a prompt for the OpenAI model
        var prompt = $"Analyze the following stock news and provide a summary along with potential short-term and long-term market implications. Here's the stock news:\n{searchResultsText}\nHere is the user query:\n{request.Query}.";
        Console.WriteLine(prompt);
        // Create the Azure OpenAI Chat Completion Service
        var service = new AzureOpenAIChatCompletionService(_openAIDeployment, _openAIEndpoint, _openAIKey);

        // Start the conversation and set the context
        var chatHistory = new ChatHistory(prompt);
        var aiResponse = await service.GetChatMessageContentAsync(chatHistory, new OpenAIPromptExecutionSettings { MaxTokens = 400 });

        // Log the AI response for debugging purposes
        if (aiResponse == null)
            return StatusCode(500, "Failed to get a response from the AI model.");

        // Return the AI response to the UI
        return Ok(new { response = aiResponse.Content });
    }
}
