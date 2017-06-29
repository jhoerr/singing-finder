namespace SingingFinder.Core.Tests

module DatabaseTests=
    open System
    open Swensen.Unquote
    open NUnit.Framework
    open SingingFinder.Core
    open SingingRepository
    open Database

    let testDbConnection () =
        System.Reflection.Assembly.GetExecutingAssembly().CodeBase
        |> UriBuilder
        |> (fun uri -> Uri.UnescapeDataString(uri.Path))
        |> System.IO.Path.GetDirectoryName
        |> (fun dir -> dir + "\\..\\..\\..\\..\\db\\minutes.db")
        |> dbConnectionFromPath

    [<Test>]
    let ``simple query`` ()=
        use cn = testDbConnection()
        cn
        |> dapperQueryWithConnection<Singing> "SELECT * FROM singings"
        |> Seq.iter (fun r -> printfn "%s" r.Name)

    [<Test>]
    let ``complex query`` ()=
        let apply s l = (s,l)
        let query="""select s.name, l.id, l.name
from singings s
inner join locations l on s.location_id = l.id"""

        testDbConnection()
        |> dapperComplexQueryWithConnection<Singing,Location,Singing*Location> query apply
        |> Seq.iter (fun (s,l) -> printfn "%s: %s" s.Name l.Name)
  
module SingingRepositoryTests=

    open System
    open SingingFinder.Core
    open Swensen.Unquote
    open NUnit.Framework
    open SingingRepository    

    let day = DateTime(2017,1,1)

    let def = 
      { Month=Month.January; 
        Day="1"; 
        Time="";
        Name=""; 
        Url=""; 
        Info=""; 
        Book=Book.All; 
        Type=SingingType.Annual; 
        Location= { Name=""; 
                    Address=""; 
                    City=""; 
                    StateProvince=""; 
                    Country=""; 
                    Latitude=0.1; 
                    Longitude=0.1; 
                    Url="";
                    PostalCode="";} }

    let red = {def with Book=Book.``1991 Edition``}
    let black = {def with Book=Book.``Christian Harmony``}
    let blueBlack = {def with Book=Book.``Cooper Edition`` ||| Book.``Christian Harmony``}
    let localRed = {def with Book=Book.``1991 Edition``; Type=SingingType.Regular}
    
    let singings = [red; black; blueBlack; localRed]

    let filterTo book singingType singings =
        singings 
        |> filterSingingsInRange day day book singingType 0 
        |> List.map (fun e-> e.Singing)

    [<Test>]
    let ``no filtering``() =
        test <@  singings |> filterTo Book.All SingingType.Annual = [red; black; blueBlack] @>

    [<Test>]
    let ``filter to specific book (1)``() =
        test <@  singings |> filterTo Book.``1991 Edition`` SingingType.Annual = [red] @>

    [<Test>]
    let ``filter to specific book (2)``() =
        test <@  singings |> filterTo Book.``Christian Harmony`` SingingType.Annual = [black; blueBlack] @>  

    [<Test>]
    let ``filter to specific book (3)``() =
        test <@  singings |> filterTo Book.``Harmonia Sacra`` SingingType.Annual = [] @>  

    [<Test>]
    let ``filter to four-shape singings``() =
        test <@  singings |> filterTo Book.``Four-Shape`` SingingType.Annual  = [red; blueBlack] @>

    [<Test>]
    let ``filter to seven-shape singings``() =
        test <@  singings |> filterTo Book.``Seven-Shape`` SingingType.Annual = [black; blueBlack] @>

    [<Test>]
    let ``filter to local singings``() =
        test <@  singings |> filterTo Book.All SingingType.Regular = [localRed] @>

    [<Test>]
    let ``filter to local singings (no results)``() =
        test <@  singings |> filterTo Book.``Cooper Edition`` SingingType.Regular = [] @>

  