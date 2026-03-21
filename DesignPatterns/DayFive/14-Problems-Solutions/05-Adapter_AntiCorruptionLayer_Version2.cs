// 05-Adapter_AntiCorruptionLayer.cs
// Intent: Anti-Corruption Adapter (ACL) to protect domain model from third-party model differences.
// DI/Lifetime: Adapter stateless, register Transient. ACL often lives at a boundary (integration layer).
// Testability: Unit-test adapters by feeding third-party DTOs and asserting domain model output.

using System;

// Third-party model (adaptee)
public class ThirdPartyAddress
{
    public string Line1;
    public string Line2;
    public string TownCity;
    public string CountryCode; // "US", "GB", etc.
}

// Domain model (ours)
public class Address
{
    public string Street { get; }
    public string City { get; }
    public string Country { get; }

    public Address(string street, string city, string country) { Street = street; City = city; Country = country; }
}

// Anti-Corruption Adapter: maps third-party address to domain Address and provides validation/normalization
public class ThirdPartyAddressAdapter
{
    public Address ToDomain(ThirdPartyAddress tpa)
    {
        // Normalize: combine lines, map country codes, simple validation
        var street = string.IsNullOrWhiteSpace(tpa.Line2) ? tpa.Line1 : $"{tpa.Line1}, {tpa.Line2}";
        var city = tpa.TownCity ?? "Unknown";
        var country = MapCountryCode(tpa.CountryCode);

        // Validate basic invariants
        if (string.IsNullOrWhiteSpace(street)) throw new InvalidOperationException("Street required from third-party data.");

        return new Address(street, city, country);
    }

    private string MapCountryCode(string code)
    {
        return code?.ToUpperInvariant() switch
        {
            "US" => "United States",
            "GB" => "United Kingdom",
            null => "Unknown",
            _ => code
        };
    }
}

/*
Why Anti-Corruption Layer?
- Prevents leaking third-party model complexities into your domain.
- Encapsulates mapping, normalization, and error handling at the boundary.
*/