# Structured vs Unstructured Data Formats & Serialization Principles (C# / .NET)

This note explains the difference between structured and unstructured data formats, when to choose each, and practical serialization principles in C#/.NET. It covers common formats (JSON, XML, binary), APIs (System.Text.Json, XmlSerializer, protobuf/MessagePack), schema/versioning, performance, streaming, and security concerns.

Definitions: structured vs unstructured
- Structured data: predictable schema (fields, types, relationships). Examples: JSON objects with known fields, XML with elements/attributes, protocol buffers. Structured formats are easy to validate, index, transform, and query.
- Unstructured data: free-form or opaque content without a strict schema. Examples: plain text, logs, images, blobs. Useful for human-readable content or binary payloads without fixed fields.

When to choose which
- Use structured formats when consumers need to parse fields, query, validate, or evolve records (APIs, interchange, config, storage of entities).
- Use unstructured formats when content is free text, binary media, or when schema cannot/should not be enforced (logs, documents, images).
- For cross-service RPC or compact storage choose binary structured formats (protobuf, MessagePack). For human-readable interchange choose JSON or XML.

Serialization APIs and basic examples

JSON (recommended for general-purpose, web APIs)
- .NET modern API: System.Text.Json (fast, low-allocation). NewtonSoft.Json (Json.NET) still widely used for features not yet in System.Text.Json in older .NET versions.

Simple serialization using System.Text.Json:
```csharp
using System.Text.Json;

var product = new Product { Id = 1, Name = "Shoe", Price = 49.99m };
var json = JsonSerializer.Serialize(product);
var p2 = JsonSerializer.Deserialize<Product>(json);

record Product { public int Id { get; init; } public string Name { get; init; } public decimal Price { get; init; } }
```

Common System.Text.Json options:
```csharp
var options = new JsonSerializerOptions {
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    WriteIndented = true,
    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
    ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve // preserves cycles and references
};
JsonSerializer.Serialize(obj, options);
```

XML (use when interoperability with XML ecosystems is required)
- XmlSerializer or DataContractSerializer are common. XML supports attributes, mixed content, and XSD validation.

XmlSerializer example:
```csharp
using System.Xml.Serialization;
using var stream = File.OpenWrite("product.xml");
var xs = new XmlSerializer(typeof(Product));
xs.Serialize(stream, product);
```

Binary structured formats (use for compact, fast, typed interchange)
- Protobuf (protobuf-net for .NET), MessagePack, FlatBuffers. They are schema-based (or schema-first) and produce much smaller payloads than JSON.
- Good for high-throughput services, mobile/network-sensitive apps, or long-term compact storage.

Protobuf-net example (conceptual):
```csharp
[ProtoContract]
public class Product {
  [ProtoMember(1)] public int Id { get; set; }
  [ProtoMember(2)] public string Name { get; set; }
  [ProtoMember(3)] public decimal Price { get; set; }
}
```
Then use `Serializer.Serialize(stream, product)` / `Serializer.Deserialize<Product>(stream)`.

Design & serialization principles

1) Separate domain model from serialized contract
- Use DTOs for persistence/transport instead of directly serializing domain objects with behavior. This isolates serialization concerns and eases versioning.

2) Explicit schema & versioning
- For structured formats plan for evolution: add optional fields, use numeric tags (protobuf) or tolerant readers (JSON).
- Include a version field or use backwards-compatible changes (additive fields). Avoid removing or renaming serialized properties without a migration plan.

3) Schema-first vs code-first
- Schema-first (XSD, .proto) gives strict contracts for multiple languages.
- Code-first (JSON, XML generated from types) is convenient but require careful versioning and documentation.

4) Handling polymorphism and references
- JSON: System.Text.Json needs converters or `ReferenceHandler.Preserve` for object references and support for polymorphism is explicit (type discriminator).
- XML/DataContract supports polymorphism via attributes (XmlInclude / KnownType) but requires care.
- Binary formats often have built-in ways to handle type tags.

5) Validation & schema enforcement
- Validate incoming structured data: JSON Schema, XSD, or programmatic checks to reject unexpected/missing fields.
- Validation helps catch backward-incompatible changes and prevents malformed input from entering business logic.

6) Streaming vs in-memory serialization
- For large payloads use streaming APIs (`Stream`, `Utf8JsonWriter`, `JsonSerializer.SerializeAsync`, streaming parsers) to avoid big allocations and OOM.
- Example reading with `JsonDocument` or `JsonSerializer.DeserializeAsync<T>` from a stream.

7) Atomic writes and integrity
- When writing files: write to a temporary file, flush, then atomically replace the target (File.Move / File.Replace) to avoid partial writes.
- Store checksums (SHA256) or signatures for critical files and verify on read.

8) Security: never trust untrusted serialized data
- Deserializing untrusted data can be an attack vector (object injection, type spoofing). Avoid `BinaryFormatter` (obsolete & insecure).
- Prefer safe, explicit serializers (System.Text.Json, protobuf-net) and whitelist allowed types when polymorphism is used.
- Avoid deserializing into types with dangerous side effects (avoid types that run code during deserialization).

9) Performance considerations
- Binary formats are smaller and faster on the wire; JSON is human-readable but larger.
- Use `JsonSerializerOptions` to tune performance (ignore defaults, configure converters).
- Cache `JsonSerializerOptions` and converters; reuse rather than recreate per operation.
- For extreme throughput, consider pooling buffers, using `Utf8JsonWriter`, and using `Span<byte>`/`Memory<T>`.

10) Human readability and debuggability
- JSON and XML are readable and easy to inspect; helpful during development and for logs. Binary formats need tooling.

Practical examples: JSON with custom converter
```csharp
// Custom converter to handle DateOnly in System.Text.Json
public class DateOnlyConverter : JsonConverter<DateOnly>
{
    public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => DateOnly.Parse(reader.GetString()!);
    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.ToString("yyyy-MM-dd"));
}
```

Best practices checklist
- Prefer System.Text.Json for JSON; avoid BinaryFormatter.
- Use DTOs and version fields; plan for additive changes.
- Validate input data and reject untrusted payloads.
- Use streaming for large data; use binary structured formats for compactness/perf.
- Ensure atomic writes + checksums for file persistence.
- Avoid serializing internal-only state or secrets; scrub sensitive fields or exclude with attributes ([JsonIgnore]).
- Log serialization/deserialization errors with enough context but not sensitive data.

Summary
Serialization is a trade-off between readability, size, speed, and safety. Structured formats (JSON/XML/protobuf) make data interoperable and predictable; unstructured formats are simple for raw content. In .NET, use modern serializers (System.Text.Json, protobuf-net, MessagePack), design stable contracts, validate input, prefer streaming for large data, and always consider security when deserializing external input.