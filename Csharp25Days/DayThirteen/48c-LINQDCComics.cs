using Jay.LearningHelperForStudents;
using Jay.LearningHelperForStudents.Utilities;

//Note : This is complementary to the following RAW SQL query document
//https://github.com/Jay-study-nildana/Azure-CSharp-Corp-Trainer-Syllabus/tree/main/SQLFundamentals/DCComics

var lh = new Lh();
var provider = new Lhdc();

var comics = provider.GetComics();
var heroesById = provider.GetSuperheroes().ToDictionary(h => h.SuperheroID);
var villainsById = provider.GetVillains().ToDictionary(v => v.VillainID);
var heroes = provider.GetSuperheroes();
var teams = provider.GetTeams();
var heroTeams = provider.GetSuperheroTeams();

//show list of all comic books with their hero and villain names (if available)

foreach (var comic in comics)
{
    heroesById.TryGetValue(comic.SuperheroID, out var hero);
    villainsById.TryGetValue(comic.VillainID, out var villain);

    var heroText = hero != null ? $"{hero.Name} ({hero.Alias})" : $"Unknown Hero (ID={comic.SuperheroID})";
    var villainText = villain != null ? $"{villain.Name} ({villain.Alias})" : $"Unknown Villain (ID={comic.VillainID})";

    Console.WriteLine($"{comic.Title} — Issue {comic.IssueNumber} ({comic.ReleaseDate:yyyy-MM-dd})");
    Console.WriteLine($"  Hero: {heroText}");
    Console.WriteLine($"  Villain: {villainText}");
    Console.WriteLine();
}

lh.AddSimpleConsoleDivider();

//--4.Retrieve all superheroes along with their teams

var rows =
    from s in heroes
    join st in heroTeams on s.SuperheroID equals st.SuperheroID
    join t in teams on st.TeamID equals t.TeamID
    select new { SuperheroName = s.Name, TeamName = t.TeamName };

lh.DisplayListOfStringsNow(rows.Select(r => $"{r.SuperheroName} : {r.TeamName}").ToList());

lh.AddSimpleConsoleDivider();

//--5.Retrieve all comics along with the superheroes featured in them

var query =
    from c in comics
    join s in heroes on c.SuperheroID equals s.SuperheroID
    select new
    {
        ComicTitle = c.Title,
        IssueNumber = c.IssueNumber,
        SuperheroName = s.Name
    };

lh.DisplayListOfStringsNow(query.Select(q => $"{q.IssueNumber}. {q.ComicTitle} featuring {q.SuperheroName}").ToList());

lh.AddSimpleConsoleDivider();

//-- 6. Count the number of comics each superhero is featured in

var counts =
    from s in heroes
    join c in comics on s.SuperheroID equals c.SuperheroID into g
    select new
    {
        SuperheroName = s.Name,
        ComicCount = g.Count()
    };

lh.DisplayListOfStringsNow(counts.Select(c => $"{c.SuperheroName} : Comic Count is {c.ComicCount}").ToList());

lh.AddSimpleConsoleDivider();

//--7.List all superheroes who are part of the 'Justice League'

var justiceLeagueNames =
    (from s in heroes
     join st in heroTeams on s.SuperheroID equals st.SuperheroID
     join t in teams on st.TeamID equals t.TeamID
     where t.TeamName == "Justice League"
     select s.Name)
    .Distinct();

Console.WriteLine("Justice League Members:");
lh.DisplayListOfStringsNow(justiceLeagueNames.ToList());

lh.AddSimpleConsoleDivider();

//-- 8. List all comics released before the year 1960

var cutoff = new DateTime(1960, 1, 1);
var oldComics = provider.GetComics()
    .Where(c => c.ReleaseDate < cutoff)
    .OrderBy(c => c.ReleaseDate)
    .ToList();

Console.WriteLine("Comics released before 1960:");
lh.DisplayListOfStringsNow(oldComics.Select(c => $"{c.Title} (Issue {c.IssueNumber}, Released: {c.ReleaseDate:yyyy-MM-dd})").ToList());

lh.AddSimpleConsoleDivider();

//-- 9. Find the first appearance of each superhero

var firstAppearances = heroes
    .OrderBy(s => s.FirstAppearance)
    .Select(s => new { s.Name, s.FirstAppearance });

lh.DisplayListOfStringsNow(firstAppearances.Select(f => $"{f.Name} first appeared in {f.FirstAppearance:yyyy-MM-dd}").ToList());
lh.AddSimpleConsoleDivider();

//-- 10. List all teams along with the number of superheroes in each team

var teamCounts =
    from t in teams
    join st in heroTeams on t.TeamID equals st.TeamID into members
    select new
    {
        TeamName = t.TeamName,
        SuperheroCount = members.Count()
    };

lh.DisplayListOfStringsNow(teamCounts.Select(t => $"${t.TeamName} : Members : {t.SuperheroCount}").ToList());
lh.AddSimpleConsoleDivider();

//-- 11. Find all superheroes who do not belong to any team

var unassignedHeroes =
    from s in heroes
    join st in heroTeams on s.SuperheroID equals st.SuperheroID into memberships
    from m in memberships.DefaultIfEmpty()
    where m == null
    select s.Name;

lh.DisplayListOfStringsNow(unassignedHeroes.Select(u => $"Hero with No Team : {u}").ToList());
lh.AddSimpleConsoleDivider();

//-- 12. List the total number of superheroes and teams

var summary = new
{
    TotalSuperheroes = heroes.Count,
    TotalTeams = teams.Count
};

Console.WriteLine($"TotalSuperheroes: {summary.TotalSuperheroes}");
Console.WriteLine($"TotalTeams: {summary.TotalTeams}");

//-- 13. Retrieve superhero details along with their first comic appearance

var firstAppearances2 =
    from s in heroes
    let first = comics
        .Where(c => c.SuperheroID == s.SuperheroID)
        .OrderBy(c => c.ReleaseDate)
        .FirstOrDefault()
    where first != null
    select new
    {
        SuperheroName = s.Name,
        FirstComicTitle = first.Title,
        first.IssueNumber,
        first.ReleaseDate
    };

lh.DisplayListOfStringsNow(firstAppearances2.Select(f => $"{f.SuperheroName} : {f.FirstComicTitle} : {f.IssueNumber} : {f.ReleaseDate:yyyyy-MM-dd}").ToList());
lh.AddSimpleConsoleDivider();

//-- 14. List all superheroes along with the number of teams they belong to

var counts2 =
    from s in heroes
    join st in heroTeams on s.SuperheroID equals st.SuperheroID into teams2
    select new
    {
        SuperheroName = s.Name,
        TeamCount = teams2.Count()
    };

lh.DisplayListOfStringsNow(counts2.Select(c => $"{c.SuperheroName} : Team Count is {c.TeamCount}").ToList());
lh.AddSimpleConsoleDivider();

//-- 15. Find the most recent comic release for each superhero

var recentPerHero =
    from s in heroes
    let recent = comics
        .Where(c => c.SuperheroID == s.SuperheroID)
        .OrderByDescending(c => c.ReleaseDate)
        .FirstOrDefault()
    where recent != null
    select new
    {
        SuperheroName = s.Name,
        RecentComicTitle = recent.Title,
        recent.ReleaseDate
    };

lh.DisplayListOfStringsNow(recentPerHero.Select(r => $"{r.SuperheroName} : {r.RecentComicTitle} : {r.ReleaseDate:yyyy-MM-dd}").ToList());
lh.AddSimpleConsoleDivider();

