using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using Newtonsoft.Json;

namespace BTS.BtsOne.Stateful.Data.Controllers
{
	[Route("api/statefuldata")]
	[ApiController]
	public class StatefulDataController : ControllerBase
	{
		private readonly IReliableStateManager _stateManager;

		public StatefulDataController(IReliableStateManager stateManager)
		{
			_stateManager = stateManager;
		}

		// GET api/values
		[HttpGet]
		public async Task<IActionResult> Get([FromQuery]string dictionaryName, [FromQuery]string keyRecord)
		{
			try
			{
				var result = new List<KeyValuePair<string, string>>();

				var tryGetResult = await _stateManager.TryGetAsync<IReliableDictionary<string, string>>(dictionaryName);

				if (tryGetResult.HasValue)
				{
					var dictionary = tryGetResult.Value;

					using (ITransaction tx = _stateManager.CreateTransaction())
					{
						var enumerable = await dictionary.CreateEnumerableAsync(tx);
						var enumerator = enumerable.GetAsyncEnumerator();

						while (await enumerator.MoveNextAsync(CancellationToken.None))
						{
							result.Add(enumerator.Current);
						}
					}

					if(!string.IsNullOrEmpty(keyRecord))
					{
						return Ok(result.Where(p => p.Key == keyRecord).ToList());
					}
				}
				
				return Ok(result);
			}
			catch (FabricException)
			{
				return new ContentResult { StatusCode = 503, Content = "The service was unable to process the request. Please try again." };
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

		// POST api/values
		[HttpPost]
		public async Task<IActionResult> Post([FromQuery]string dictionaryName, [FromQuery]string keyRecord, [FromBody] dynamic dataObject)
		{
			try
			{
				var dictionary = await _stateManager.GetOrAddAsync<IReliableDictionary<string, string>>(dictionaryName);

				using (ITransaction tx = _stateManager.CreateTransaction())
				{
					string datos = JsonConvert.SerializeObject(dataObject);
					//await dictionary.SetAsync(tx, keyRecord, datos);
					await dictionary.AddOrUpdateAsync(tx, keyRecord, datos, (key, value) => datos);
					await tx.CommitAsync();
				}

				return Ok();
			}
			catch (FabricNotPrimaryException)
			{
				return new ContentResult { StatusCode = 410, Content = "The primary replica has moved. Please re-resolve the service." };
			}
			catch (FabricException)
			{
				return new ContentResult { StatusCode = 503, Content = "The service was unable to process the request. Please try again." };
			}
		}

		// DELETE api/statefuldata?dictionaryName=xxx&keyRecord=xxx
		[HttpDelete]
		public async Task<IActionResult> Delete([FromQuery]string dictionaryName, [FromQuery]string keyRecord)
		{
			var votesDictionary = await _stateManager.GetOrAddAsync<IReliableDictionary<string, string>>(dictionaryName);

			using (ITransaction tx = _stateManager.CreateTransaction())
			{
				if (await votesDictionary.ContainsKeyAsync(tx, keyRecord))
				{
					await votesDictionary.TryRemoveAsync(tx, keyRecord);
					await tx.CommitAsync();
					return new OkResult();
				}
				else
				{
					return new NotFoundResult();
				}
			}
		}
	}
}
