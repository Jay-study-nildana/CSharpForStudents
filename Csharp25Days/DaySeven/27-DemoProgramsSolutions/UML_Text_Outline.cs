using System;

var outline = new UML_Text_Outline();
outline.PrintOutline();

Console.WriteLine();
Console.WriteLine("Press any key to exit...");
Console.ReadKey();

/// <summary>
/// Problem: UML_Text_Outline
/// Prints a textual UML-like outline for a small Library domain showing classes, fields, methods, and responsibilities.
/// </summary>
public class UML_Text_Outline
{
    public void PrintOutline()
    {
        Console.WriteLine("UML-like Outline: Library System");
        Console.WriteLine();
        Console.WriteLine("Class: Book");
        Console.WriteLine("+ Title: string");
        Console.WriteLine("+ Author: string");
        Console.WriteLine("+ Pages: int");
        Console.WriteLine("+ ToString(): string");
        Console.WriteLine("Responsibilities: represent immutable book data.");
        Console.WriteLine();
        Console.WriteLine("Class: Member");
        Console.WriteLine("+ Id: int");
        Console.WriteLine("+ Name: string");
        Console.WriteLine("+ BorrowedBooks: IReadOnlyList<Book>");
        Console.WriteLine("+ Borrow(book: Book): void");
        Console.WriteLine("+ Return(book: Book): bool");
        Console.WriteLine("Responsibilities: track borrowed books for a member.");
        Console.WriteLine();
        Console.WriteLine("Class: Loan");
        Console.WriteLine("+ Book: Book");
        Console.WriteLine("+ Member: Member");
        Console.WriteLine("+ LoanDate: DateTime");
        Console.WriteLine("+ DueDate: DateTime");
        Console.WriteLine("Responsibilities: represent loan record and due calculation.");
        Console.WriteLine();
        Console.WriteLine("Class: Library");
        Console.WriteLine("+ AddBook(book: Book): void");
        Console.WriteLine("+ LendBook(title: string, to: Member): Loan");
        Console.WriteLine("+ ReturnBook(loan: Loan): bool");
        Console.WriteLine("+ GetAvailableBooks(): IReadOnlyCollection<Book>");
        Console.WriteLine("Responsibilities: manage collection and coordinate lending operations.");
        Console.WriteLine();
        Console.WriteLine("Design notes: keep Library responsible for collection management; Member responsible for per-member borrowed list; Loan encapsulates loan-specific data. High cohesion and clear responsibilities.");
    }
}