using AutoMapper;
using CoreCodeCamp.Data.Entities;
using Sessionize = CoreCodeCamp.Data.Sessionize;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CoreCodeCamp.Services
{
  public class SessionizeDataCache : IDisposable
  {
    private readonly IMemoryCache _cache;
    private readonly IMapper _mapper;
    private HttpClient _client;

    public SessionizeDataCache(IMemoryCache cache, IMapper mapper)
    {
      _cache = cache;
      _mapper = mapper;
    }

    HttpClient Client
    {
      get
      {
        if (_client == null)
        {
          _client = new HttpClient();

        }

        return _client;
      }
    }


    async Task<Sessionize.SessionizeResult> GetSessionizeResults(string embedId)
    {
      Sessionize.SessionizeResult results;
      if (!_cache.TryGetValue(embedId, out results))
      {
        var url = $"https://sessionize.com/api/v2/{embedId}/view/All";
        var json = await Client.GetStringAsync(url);
        results = Sessionize.SessionizeResult.FromJson(json);

        _cache.Set(embedId, results, DateTimeOffset.Now.AddMinutes(5));
      }

      return results;
    }

    public async Task<Speaker[]> GetSpeakersAsync(string embedId)
    {
      var results = await GetSessionizeResults(embedId);
      if (results != null)
      {
        return results.ConvertToSpeakers(_mapper);
      }

      throw new InvalidOperationException("Failed to find speakers");
    }

    public void Dispose()
    {
      if (_client != null) _client.Dispose();
    }
  }
}
