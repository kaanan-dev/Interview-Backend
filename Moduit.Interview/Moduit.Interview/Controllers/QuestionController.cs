using Microsoft.AspNetCore.Mvc;
using Moduit.Interview.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;

namespace Moduit.Interview.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class QuestionController : ControllerBase
	{
		public IRestClient client;
		public QuestionController(IServiceProvider svcProvider)
		{
			client = svcProvider.GetService<IRestClient>();
			client.UseNewtonsoftJson(new JsonSerializerSettings
			{
				DefaultValueHandling = DefaultValueHandling.Ignore,
				NullValueHandling = NullValueHandling.Ignore,
				ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
			});
			client.BaseUrl = new Uri("https://screening.moduit.id/backend/question");
		}
		[HttpGet]
		[Route("one")]
		public async Task<ActionResult> One()
		{
			try
			{
				var request = new RestRequest("one", DataFormat.Json);
				var response = await client.GetAsync<Entity>(request);
				return Ok(response);
			}
			catch (Exception)
			{
				throw;
			}
		}
		[HttpGet]
		[Route("two")]
		public async Task<ActionResult> Two()
		{
			try
			{
				var request = new RestRequest("two", DataFormat.Json);
				var response = await client.GetAsync<IEnumerable<Entity>>(request);
				return Ok(
					response
						.Where(entity => entity.Description.Contains("Ergonomic") || entity.Title.Contains("Ergonomic"))
						.Where(entity => entity.Tags != null && entity.Tags.Count > 0 && entity.Tags.Contains("Sports"))
						.OrderByDescending(o => o.Id)
						.Take(3)
						);
			}
			catch (Exception)
			{
				throw;
			}
		}

		[HttpGet]
		[Route("three")]
		public async Task<ActionResult> Three()
		{
			try
			{
				var request = new RestRequest("three", DataFormat.Json);
				var response = await client.GetAsync<IEnumerable<Entity>>(request);
				Console.WriteLine(JsonConvert.SerializeObject(response));
				return Ok(
					response
						.Where(entity => entity.Items != null && entity.Items.Count > 0)
						.SelectMany(entity => entity
										   .Items?.Select(newEntity => new Entity
										   {
											   Id = entity.Id,
											   Category = entity.Category,
											   Title = newEntity.Title,
											   Description = newEntity.Description,
											   Footer = newEntity.Footer,
											   CreatedAt = entity.CreatedAt
										   })
								)

						);
			}
			catch (Exception)
			{
				throw;
			}
		}

	}
}