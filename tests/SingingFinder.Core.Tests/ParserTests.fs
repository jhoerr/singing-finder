namespace SingingFinder.Core.Tests


module ParserTests=

    open System
    open System.IO
    open Microsoft.FSharp.Text.Lexing
    open SingingFinder.Core
    open Parser

    open FsUnit
    open NUnit.Framework
    open Swensen.Unquote

    let parse (singing:string) =
        let lexbuf = singing.ToLowerInvariant() |> LexBuffer<char>.FromString
        Parser.start Lexer.read lexbuf
    
    [<Test>]
    let ``specific day``()=
        test <@ "1" |> parse = DayOfMonth(1) @>

    [<Test>]
    let ``every sunday``()=
        test <@ "every monday" |> parse = Every(DayOfWeek.Monday) @>

    [<Test>]
    let ``first sunday``()=
        test <@ "first sunday" |> parse = Regular([First], DayOfWeek.Sunday) @>

    [<Test>]
    let ``1st sunday``()=
        test <@ "1st sunday" |> parse = Regular([First], DayOfWeek.Sunday) @>

    [<Test>]
    let ``first and third monday``()=
        test <@ "first and third monday" |> parse = Regular([First; Third], DayOfWeek.Monday) @>

    [<Test>]
    let ``fiRST AnD THIRD monday``()=
        test <@ "fiRST AnD THIRD monday" |> parse = Regular([First; Third], DayOfWeek.Monday) @>

    [<Test>]
    let ``first and third mondays``()=
        test <@ "first and third mondays" |> parse = Regular([First; Third], DayOfWeek.Monday) @>

    [<Test>]
    let ``first sunday and saturday before``()=
        test <@ "first sunday and saturday before" |> parse = OnOrBefore([First], DayOfWeek.Sunday, [DayOfWeek.Saturday; DayOfWeek.Sunday]) @>

    [<Test>]
    let ``saturday before first sunday``()=
        test <@ "saturday before first sunday" |> parse = OnOrBefore([First], DayOfWeek.Sunday, [DayOfWeek.Saturday]) @>

    [<Test>]
    let ``saturday before first and third sunday``()=
        test <@ "saturday before first and third sunday" |> parse = OnOrBefore([First; Third], DayOfWeek.Sunday, [DayOfWeek.Saturday]) @>

    [<Test>]
    let ``saturday before first, third, and fifth sunday (oxford comma)``()=
        test <@ "saturday before first, third, and fifth sunday" |> parse = OnOrBefore([First; Third; Fifth], DayOfWeek.Sunday, [DayOfWeek.Saturday]) @>

    [<Test>]
    let ``saturday before first, third and fifth sunday (no oxford comma)``()=
        test <@ "saturday before first, third and fifth sunday" |> parse = OnOrBefore([First; Third; Fifth], DayOfWeek.Sunday, [DayOfWeek.Saturday]) @>

    [<Test>]
    let ``thursday and friday and saturday before first and third sunday``()=
        test <@ "thursday and friday and saturday before first and third sunday" |> parse = OnOrBefore([First; Third], DayOfWeek.Sunday, [DayOfWeek.Thursday; DayOfWeek.Friday; DayOfWeek.Saturday]) @>

    [<Test>]
    let ``thursday, friday, and saturday before first and third sunday``()=
        test <@ "thursday, friday, and saturday before first and third sunday" |> parse = OnOrBefore([First; Third], DayOfWeek.Sunday, [DayOfWeek.Thursday; DayOfWeek.Friday; DayOfWeek.Saturday]) @>

    [<Test>]
    let ``thursday, friday, saturday before first, third and fifth sunday``()=
        test <@ "thursday, friday, saturday before first, third and fifth sunday" |> parse = OnOrBefore([First; Third; Fifth], DayOfWeek.Sunday, [DayOfWeek.Thursday; DayOfWeek.Friday; DayOfWeek.Saturday]) @>
    