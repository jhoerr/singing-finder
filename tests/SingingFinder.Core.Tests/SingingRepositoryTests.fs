namespace SingingFinder.Core.Tests

module SingingCacheTests=
    open System
    open Swensen.Unquote
    open NUnit.Framework
    open SingingFinder.Core
    open SingingRepository

    (*
    [<Test>]
    [<Category "integration">]
    let ``singings are cached``()=
        let timeToFetch() = 
            let stopWatch = System.Diagnostics.Stopwatch.StartNew()
            let singings' = singings()
            let elapsed = stopWatch.ElapsedMilliseconds
            printfn "elapsed: %d ms" elapsed
            elapsed

        test <@ timeToFetch() > 100L @>
        test <@ timeToFetch() < 10L @>
        test <@ timeToFetch() < 10L @>
        
        System.Threading.Thread.Sleep(5)

        test <@ timeToFetch() > 100L @>
    *)
  
module SingingRepositoryTests=

    open System
    open SingingFinder.Core
    open Swensen.Unquote
    open NUnit.Framework
    open SingingRepository    

    let day = DateTime(2017,1,1)

    let def = {Month=Month.January; Day="1"; Name=""; SingingUrl=""; Location=""; Info=""; Latitude=1.0; Longitude=1.0; LocationUrl=""; Book=Book.All; Type=SingingType.Annual }
    let red = {def with Book=Book.``1991 Edition``}
    let black = {def with Book=Book.``Christian Harmony``}
    let blueBlack = {def with Book=Book.``Cooper Edition`` ||| Book.``Christian Harmony``}
    let localRed = {def with Book=Book.``1991 Edition``; Type=SingingType.Regular}
    
    let singings = [red; black; blueBlack; localRed]

    (*
    [<Test>]
    [<Category "integration">]
    let ``fetch all singings``()=
        getSingings()
        |> List.iter (fun s -> printfn "%s (%A)" s.Name s.Book)
    *)

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