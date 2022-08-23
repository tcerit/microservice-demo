using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Core.Events;

public class DomainEventConverter : JsonConverter<DomainEvent>
{
    public override DomainEvent Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public override void Write(
        Utf8JsonWriter writer,
        DomainEvent value,
        JsonSerializerOptions options)
    {
        switch (value)
        {
            case null:
                JsonSerializer.Serialize(writer, null, options);
                break;
            default:
                {
                    var type = value.GetType();
                    JsonSerializer.Serialize(writer, value, type, options);
                    break;
                }
        }
    }
}
