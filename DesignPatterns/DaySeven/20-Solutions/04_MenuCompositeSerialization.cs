// 04_MenuCompositeSerialization.cs
// Serialize/deserialize a composite menu to/from JSON using a simple DTO with type discriminator.

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Day07.CompositeSerialization
{
    public abstract class MenuComponent
    {
        public abstract string Title { get; }
        public virtual void Add(MenuComponent c) => throw new NotSupportedException();
        public virtual void Remove(MenuComponent c) => throw new NotSupportedException();

        // Convert to DTO for serialization
        public abstract MenuDto ToDto();
        public static MenuComponent FromDto(MenuDto dto)
        {
            if (dto.Type == "Item") return new MenuItem(dto.Title);
            if (dto.Type == "Group")
            {
                var g = new MenuGroup(dto.Title);
                if (dto.Children != null)
                {
                    foreach (var child in dto.Children) g.Add(FromDto(child));
                }
                return g;
            }
            throw new InvalidOperationException("Unknown DTO type");
        }
    }

    public class MenuItem : MenuComponent
    {
        public override string Title { get; }
        public MenuItem(string title) => Title = title;
        public override MenuDto ToDto() => new MenuDto { Type = "Item", Title = Title };
    }

    public class MenuGroup : MenuComponent
    {
        private readonly List<MenuComponent> _children = new();
        public override string Title { get; }
        public MenuGroup(string title) => Title = title;
        public override void Add(MenuComponent c) => _children.Add(c);
        public override void Remove(MenuComponent c) => _children.Remove(c);
        public override MenuDto ToDto()
        {
            var dto = new MenuDto { Type = "Group", Title = Title, Children = new List<MenuDto>() };
            foreach (var child in _children) dto.Children.Add(child.ToDto());
            return dto;
        }
    }

    public class MenuDto
    {
        public string Type { get; set; } = "";
        public string Title { get; set; } = "";
        public List<MenuDto>? Children { get; set; }
    }

    class Program
    {
        static void Main()
        {
            var root = new MenuGroup("Root");
            root.Add(new MenuItem("Home"));
            var s = new MenuGroup("Settings");
            s.Add(new MenuItem("General"));
            s.Add(new MenuItem("Security"));
            root.Add(s);

            var dto = root.ToDto();
            var json = JsonSerializer.Serialize(dto, new JsonSerializerOptions { WriteIndented = true });
            Console.WriteLine("Serialized JSON:");
            Console.WriteLine(json);

            // Deserialize
            var deserializedDto = JsonSerializer.Deserialize<MenuDto>(json)!;
            var reconstructed = MenuComponent.FromDto(deserializedDto);
            Console.WriteLine("\nReconstructed tree (titles only):");
            PrintTitles(reconstructed, 0);
        }

        static void PrintTitles(MenuComponent c, int depth)
        {
            Console.WriteLine(new string(' ', depth * 2) + "- " + c.Title);
            if (c is MenuGroup mg)
            {
                // Reflection to get children is not exposed; we'll reconstruct via DTO in real code.
            }
        }
    }
}