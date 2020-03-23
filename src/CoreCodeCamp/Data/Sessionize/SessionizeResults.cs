using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CoreCodeCamp.Data.Sessionize
{

  public partial class SessionizeResult
  {
    [JsonProperty("sessions")]
    public List<Session> Sessions { get; set; }

    [JsonProperty("speakers")]
    public List<Speaker> Speakers { get; set; }

    [JsonProperty("questions")]
    public List<Question> Questions { get; set; }

    [JsonProperty("categories")]
    public List<Category> Categories { get; set; }

    [JsonProperty("rooms")]
    public List<Room> Rooms { get; set; }

    public static SessionizeResult FromJson(string json) => JsonConvert.DeserializeObject<SessionizeResult>(json, CoreCodeCamp.Data.Sessionize.Converter.Settings);
  }

  public partial class Category
  {
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("items")]
    public List<Room> Items { get; set; }

    [JsonProperty("sort")]
    public long Sort { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }
  }

  public partial class Room
  {
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("sort")]
    public long Sort { get; set; }
  }

  public partial class Question
  {
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("question")]
    public string QuestionQuestion { get; set; }

    [JsonProperty("questionType")]
    public string QuestionType { get; set; }

    [JsonProperty("sort")]
    public long Sort { get; set; }
  }

  public partial class Session
  {
    [JsonProperty("id")]
    public Id Id { get; set; }

    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("startsAt")]
    public DateTimeOffset StartsAt { get; set; }

    [JsonProperty("endsAt")]
    public DateTimeOffset EndsAt { get; set; }

    [JsonProperty("isServiceSession")]
    public bool IsServiceSession { get; set; }

    [JsonProperty("isPlenumSession")]
    public bool IsPlenumSession { get; set; }

    [JsonProperty("speakers")]
    public List<Guid> Speakers { get; set; }

    [JsonProperty("categoryItems")]
    public List<long> CategoryItems { get; set; }

    [JsonProperty("questionAnswers")]
    public List<QuestionAnswer> QuestionAnswers { get; set; }

    [JsonProperty("roomId")]
    public long RoomId { get; set; }
  }

  public partial class QuestionAnswer
  {
    [JsonProperty("questionId")]
    public long QuestionId { get; set; }

    [JsonProperty("answerValue")]
    [JsonConverter(typeof(ParseStringConverter))]
    public long AnswerValue { get; set; }
  }

  public partial class Speaker
  {
    [JsonProperty("id")]
    public Guid Id { get; set; }

    [JsonProperty("firstName")]
    public string FirstName { get; set; }

    [JsonProperty("lastName")]
    public string LastName { get; set; }

    [JsonProperty("bio")]
    public string Bio { get; set; }

    [JsonProperty("tagLine")]
    public string TagLine { get; set; }

    [JsonProperty("profilePicture")]
    public Uri ProfilePicture { get; set; }

    [JsonProperty("isTopSpeaker")]
    public bool IsTopSpeaker { get; set; }

    [JsonProperty("links")]
    public List<Link> Links { get; set; }

    [JsonProperty("sessions")]
    public List<long> Sessions { get; set; }

    [JsonProperty("fullName")]
    public string FullName { get; set; }

    [JsonProperty("categoryItems")]
    public List<object> CategoryItems { get; set; }

    [JsonProperty("questionAnswers")]
    public List<object> QuestionAnswers { get; set; }
  }

  public partial class Link
  {
    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("url")]
    public Uri Url { get; set; }

    [JsonProperty("linkType")]
    public string LinkType { get; set; }
  }

  public partial struct Id
  {
    public long? Integer;
    public Guid? Uuid;

    public static implicit operator Id(long Integer) => new Id { Integer = Integer };
    public static implicit operator Id(Guid Uuid) => new Id { Uuid = Uuid };
  }

  internal static class Converter
  {
    public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
    {
      MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
      DateParseHandling = DateParseHandling.None,
      Converters =
            {
                IdConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
    };
  }

  internal class IdConverter : JsonConverter
  {
    public override bool CanConvert(Type t) => t == typeof(Id) || t == typeof(Id?);

    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
    {
      switch (reader.TokenType)
      {
        case JsonToken.String:
        case JsonToken.Date:
          var stringValue = serializer.Deserialize<string>(reader);
          long l;
          if (Int64.TryParse(stringValue, out l))
          {
            return new Id { Integer = l };
          }
          Guid guid;
          if (Guid.TryParse(stringValue, out guid))
          {
            return new Id { Uuid = guid };
          }
          break;
      }
      throw new Exception("Cannot unmarshal type Id");
    }

    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
    {
      var value = (Id)untypedValue;
      if (value.Integer != null)
      {
        serializer.Serialize(writer, value.Integer.Value.ToString());
        return;
      }
      if (value.Uuid != null)
      {
        serializer.Serialize(writer, value.Uuid.Value.ToString("D", System.Globalization.CultureInfo.InvariantCulture));
        return;
      }
      throw new Exception("Cannot marshal type Id");
    }

    public static readonly IdConverter Singleton = new IdConverter();
  }

  internal class ParseStringConverter : JsonConverter
  {
    public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
    {
      if (reader.TokenType == JsonToken.Null) return null;
      var value = serializer.Deserialize<string>(reader);
      long l;
      if (Int64.TryParse(value, out l))
      {
        return l;
      }
      throw new Exception("Cannot unmarshal type long");
    }

    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
    {
      if (untypedValue == null)
      {
        serializer.Serialize(writer, null);
        return;
      }
      var value = (long)untypedValue;
      serializer.Serialize(writer, value.ToString());
      return;
    }

    public static readonly ParseStringConverter Singleton = new ParseStringConverter();
  }

}