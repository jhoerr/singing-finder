namespace SingingFinder.Core.Tests

module SingingRepositoryTests=

    open System
    open SingingFinder.Core
    open Swensen.Unquote
    open NUnit.Framework
    open SingingRepository

    [<Test>]
    [<Category "integration">]
    let ``time fetch all singings``()=
        let stopWatch = System.Diagnostics.Stopwatch.StartNew()
        let singings' = singings()
        stopWatch.Stop()
        printfn "%f ms\n\n" stopWatch.Elapsed.TotalMilliseconds
    
    [<Test>]
    [<Category "integration">]
    let ``fetch all singings``()=
        singings()
        |> List.iter (fun s -> printf "%s\n" (s.Name))

