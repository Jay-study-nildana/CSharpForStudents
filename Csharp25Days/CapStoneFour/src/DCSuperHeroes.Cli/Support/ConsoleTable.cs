namespace DCSuperHeroes.Cli.Support;

public static class ConsoleTable
{
    public static void Print(IEnumerable<string> headers, IEnumerable<string[]> rows)
    {
        var headerList = headers.ToList();
        var rowList = rows.Select(row => row.ToArray()).ToList();

        if (headerList.Count == 0)
        {
            return;
        }

        var widths = new int[headerList.Count];
        for (var index = 0; index < headerList.Count; index++)
        {
            widths[index] = headerList[index].Length;
        }

        foreach (var row in rowList)
        {
            for (var index = 0; index < headerList.Count && index < row.Length; index++)
            {
                widths[index] = Math.Max(widths[index], row[index].Length);
            }
        }

        WriteRow(headerList.ToArray(), widths);
        Console.WriteLine(string.Join("-+-", widths.Select(width => new string('-', width))));

        foreach (var row in rowList)
        {
            WriteRow(row, widths);
        }
    }

    private static void WriteRow(string[] cells, int[] widths)
    {
        for (var index = 0; index < widths.Length; index++)
        {
            var cell = index < cells.Length ? cells[index] : string.Empty;
            Console.Write(cell.PadRight(widths[index]));

            if (index < widths.Length - 1)
            {
                Console.Write(" | ");
            }
        }

        Console.WriteLine();
    }
}