using AutoMapper;
using CoreCodeCamp.Data.Entities;
using CoreCodeCamp.Data.Sessionize;
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


    async Task<SessionizeResults> GetSessionizeResults(string embedId)
    {
      SessionizeResults results;
      if (!_cache.TryGetValue(embedId, out results))
      {
        var test = "l9c4tvg5";
        var url = $"https://sessionize.com/api/v2/{test}/view/All";
        var json = await Client.GetStringAsync(url);
        results = JsonConvert.DeserializeObject<SessionizeResults>(json);

        _cache.Set(embedId, results, DateTimeOffset.Now.AddMinutes(5));
      }

      return results;
    }

    public async Task<Speaker[]> GetSpeakersAsync(string embedId)
    {
      var results = await GetSessionizeResults(embedId);
      if (results != null)
      {
        return _mapper.Map<Speaker[]>(results.Speakers);
      }

      throw new InvalidOperationException("Failed to find speakers");
    }

    public void Dispose()
    {
      if (_client != null) _client.Dispose();
    }
  }
}
