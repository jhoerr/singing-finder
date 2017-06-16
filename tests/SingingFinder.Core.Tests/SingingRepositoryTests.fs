namespace SingingFinder.Core.Tests

module SingingCacheTests=
    open System
    open Swensen.Unquote
    open NUnit.Framework
    open SingingFinder.Core
    open SingingCache
    open SingingRepository

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
  
module SingingRepositoryTests=

    open System
    open SingingFinder.Core
    open Swensen.Unquote
    open NUnit.Framework
    open SingingRepository    

    [<Test>]
    [<Category "integration">]
    let ``fetch all singings``()=
        singings()
        |> List.iter (fun s -> printf "%s\n" (s.Name))

