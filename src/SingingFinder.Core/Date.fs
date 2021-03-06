﻿namespace SingingFinder.Core


module Date=

    open System
    open Microsoft.FSharp.Text.Lexing

    // Find the first instance of a day of the week falling on or before the given date
    let rec findNearestDayOnOrBefore (start:DateTime) target =
        if start.DayOfWeek = target 
        then start 
        else findNearestDayOnOrBefore (start.AddDays(-1.0)) target  

    // Find the date of the first instance of a particular day of the week in a given month and year
    // Ex 1: The first Sunday in January 2017 is 1/1/2017
    // Ex 2: The first Tuesday in February 2018 is 2/6/2018
    let rec firstDayOfWeekInMonth year month dayOfWeek = 
        let rec firstDayOfWeekIn' (date:DateTime) = 
            if date.DayOfWeek = dayOfWeek
            then date
            else firstDayOfWeekIn' (date.AddDays(1.0))

        firstDayOfWeekIn' (DateTime(year, month, 1))

    // Find the date of the Nth instance of a particular day of the week in a given month and year
    // Ex 1: The fourth Sunday in January 2017 is 1/22/2017
    // Ex 2: The second Tuesday in February 2018 is 2/13/2018
    // Account for the fact that months can sometimes have an e.g. "fifth" Sunday, and sometimes not.
    let nthDayOfWeekInMonth year month dayOfWeek offset = 
        let firstInstance = dayOfWeek |> firstDayOfWeekInMonth year month
        let nthInstance = firstInstance.AddDays(7.0 * (float)(offset - 1))
        if nthInstance.Month = int month then Some(nthInstance) else None

    // Find the date of the last instance of a particular day of the week in a given month and year
    // Ex 1: The last Sunday in January 2017 is 1/29/2017
    // Ex 2: The last Tuesday in February 2018 is 2/27/2018
    let lastDayOfWeekInMonth year month dayOfWeek =
        let lastDayOfMonth = DateTime(year, month, 1).AddMonths(1).AddDays(-1.0)
        Some(findNearestDayOnOrBefore lastDayOfMonth dayOfWeek)

    let resolveDate year month dayOfWeek cardinality =
        match cardinality with
        | First     -> nthDayOfWeekInMonth  year month dayOfWeek 1
        | Second    -> nthDayOfWeekInMonth  year month dayOfWeek 2
        | Third     -> nthDayOfWeekInMonth  year month dayOfWeek 3
        | Fourth    -> nthDayOfWeekInMonth  year month dayOfWeek 4
        | Fifth     -> nthDayOfWeekInMonth  year month dayOfWeek 5
        | Last      -> lastDayOfWeekInMonth year month dayOfWeek

    // parse the singing day description into a domain SingingDay
    let parse (singing:string) =
        let lexbuf = singing.ToLowerInvariant() |> LexBuffer<char>.FromString
        Parser.start Lexer.read lexbuf

    // resolve a specific calendar day
    let dayOfMonth year month day =
        match month with 
        | 0 -> failwith "Specific month is required"
        | m ->
            let d = DateTime(year, int m, day)
            [ {Start=d; End=d} ]
    
    // find all the dates in a month falling in specific cardinal positions (i.e. second, fourth, last) 
    //   for the given year, month(s) and days of the week.
    let onOrBefore year months cardinalities referenceDay singingDays =
        months 
        |> List.collect (fun m -> 
            cardinalities
            |> List.map (fun c -> resolveDate year m referenceDay c)
            |> List.filter (fun nth -> nth.IsSome)
            |> List.map (fun nth-> nth.Value)
            |> List.map (fun d -> 
              { Start=  singingDays |> List.head |> findNearestDayOnOrBefore d; 
                End=    singingDays |> List.last |> findNearestDayOnOrBefore d } ))
    
    let regular year months dayOfWeek cardinalities =
        [dayOfWeek]
        |> onOrBefore year months cardinalities dayOfWeek

    let every year months dayOfWeek =
        [ First; Second; Third; Fourth; Fifth ]
        |> regular year months dayOfWeek
 
    // determine the singing days from its description
    let parseSingingDates year singing =
        let month = int singing.Month
        let months = 
            match singing.Month with
            | Month.All -> [ 1 .. 12]
            | _         -> [ month ]

        match parse singing.Day with 
        | DayOfMonth(day)       -> dayOfMonth year month day
        | Every(day)            -> every year months day
        | Regular (cards, day)  -> regular year months day cards
        | OnOrBefore(cards, refDay, days) -> onOrBefore year months cards refDay days

    // determine if two date ranges overlap
    let dateRangesOverlap range1 range2 =
        range1.Start <= range2.End && range2.Start <= range1.End

    // determine the upcoming dates for a given singing that fall between the supplied date range.
    let determineSingingDates days singing =
        let startingYearDays = parseSingingDates days.Start.Year singing
        let endingYearDays = parseSingingDates days.End.Year singing
        let daysInRange = 
            startingYearDays 
            @ endingYearDays
            |> List.distinct
            |> List.filter (fun d -> dateRangesOverlap days d)

        { Singing = singing; Days = daysInRange }

    // take a list of singings and determine which, if any, fall within a supplied date range.
    let singingsWithinDateRange (days:Days) (singings:Singing list) = 
        singings
        |> List.map (fun s -> determineSingingDates days s)
        |> List.filter (fun e -> e.Days |> List.isEmpty |> not)
        |> List.sortBy (fun e -> e.Days |> List.head)
