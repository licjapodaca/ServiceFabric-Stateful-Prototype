using BTS.Communication.Tools;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
	[Route("api/values")]
	[ApiController]
	public class ValuesController : ControllerBase
	{
		private readonly FabricClient _fabricClient;
		private readonly StatelessServiceContext _ctx;
		private readonly HttpStatefulClient _httpStatefulClient;

		public ValuesController(FabricClient fabricClient, StatelessServiceContext ctx)
		{
			_fabricClient = fabricClient;
			_ctx = ctx;
			_httpStatefulClient = new HttpStatefulClient(_ctx, "micro-statefuldata");
		}

		// GET api/values
		[HttpGet]
		public async Task<IActionResult> Get([FromQuery]string dictionaryName, [FromQuery]string keyRecord)
		{
			try
			{
				var result = await _httpStatefulClient.GetStringAsync("api/statefuldata", dictionaryName, keyRecord);

				var resultado = new List<KeyValuePair<string, object>>();

				foreach (var record in JsonConvert.DeserializeObject<List<KeyValuePair<string, string>>>(result))
				{
					resultado.Add(new KeyValuePair<string, object>(record.Key, JsonConvert.DeserializeObject(record.Value)));
				}

				return Ok(resultado);
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		// POST api/values
		[HttpPost]
		public async Task<IActionResult> Post([FromQuery]string dictionaryName, [FromQuery]string keyRecord, [FromBody] dynamic dataObject)
		{
			try
			{
				var response = await _httpStatefulClient.PostAsync("api/statefuldata", dictionaryName, keyRecord, dataObject);

				return new ContentResult()
				{
					StatusCode = (int) response.StatusCode,
					Content = await response.Content.ReadAsStringAsync()
				};
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		// DELETE api/values
		[HttpDelete]
		public async Task<IActionResult> Delete([FromQuery]string dictionaryName, [FromQuery]string keyRecord)
		{
			try
			{
				var response = await _httpStatefulClient.DeleteAsync("api/statefuldata", dictionaryName, keyRecord);

				return new ContentResult()
				{
					StatusCode = (int) response.StatusCode,
					Content = await response.Content.ReadAsStringAsync()
				};
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
