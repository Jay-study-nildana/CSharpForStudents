// CompositePatternDemo.cs
// Console demo of the Composite Pattern for menu structures.

using System;
using System.Collections.Generic;

// Top-level statements
// Build a sample menu tree
var root = new MenuGroup("Main Menu");
var fileMenu = new MenuGroup("File");
fileMenu.Add(new MenuItem("New"));
fileMenu.Add(new MenuItem("Open"));
fileMenu.Add(new MenuItem("Exit"));
var editMenu = new MenuGroup("Edit");
editMenu.Add(new MenuItem("Cut"));
editMenu.Add(new MenuItem("Copy"));
editMenu.Add(new MenuItem("Paste"));
var helpMenu = new MenuGroup("Help");
helpMenu.Add(new MenuItem("About"));

root.Add(fileMenu);
root.Add(editMenu);
root.Add(helpMenu);

// Render the menu tree
Console.WriteLine("Menu Structure:");
root.Render(0);

// Optionally, interactively add items
while (true)
{
    Console.WriteLine("Add menu item? (y/n)");
    var ans = Console.ReadLine();
    if (ans != "y") break;
    Console.Write("Enter group (File/Edit/Help): ");
    var group = Console.ReadLine();
    Console.Write("Enter item title: ");
    var title = Console.ReadLine();
    MenuGroup? target = group.ToLower() switch
    {
        "file" => fileMenu,
        "edit" => editMenu,
        "help" => helpMenu,
        _ => null
    };
    if (target != null)
    {
        target.Add(new MenuItem(title));
        Console.WriteLine("Updated Menu:");
        root.Render(0);
    }
    else
    {
        Console.WriteLine("Invalid group.");
    }
}


// Component
public abstract class MenuComponent
{
    public virtual void Add(MenuComponent c) => throw new NotSupportedException();
    public virtual void Remove(MenuComponent c) => throw new NotSupportedException();
    public abstract void Render(int depth);
}

// Leaf
public class MenuItem : MenuComponent
{
    public string Title { get; }
    public MenuItem(string title) => Title = title;
    public override void Render(int depth)
    {
        Console.WriteLine(new string(' ', depth * 2) + "- " + Title);
    }
}

// Composite
public class MenuGroup : MenuComponent
{
    private readonly List<MenuComponent> _children = new();
    public string Title { get; }
    public MenuGroup(string title) => Title = title;
    public override void Add(MenuComponent c) => _children.Add(c);
    public override void Remove(MenuComponent c) => _children.Remove(c);
    public override void Render(int depth)
    {
        Console.WriteLine(new string(' ', depth * 2) + "+ " + Title);
        foreach (var child in _children) child.Render(depth + 1);
    }
}

