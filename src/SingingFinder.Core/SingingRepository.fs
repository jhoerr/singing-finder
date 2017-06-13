namespace SingingFinder.Core

open System
open Date

module SingingRepository =

    let ohioStateConvention = {Name = "Ohio State Convention"; Day = "First Sunday and Saturday Before"; Month = 3; Location="First Lutheran Church, 1208 Race Street, Cincinnati, OH"; Latitude=39.108481; Longitude=(-84.516286); Info="""<a href="https://www.facebook.com/events/250222832056537">Facebook group</a>"""; LocationUrl="https://www.google.com/maps/place/First+Lutheran+Church/@39.1084897,-84.5184857,17z/data=!3m1!4b1!4m5!3m4!1s0x8841b1561cc20e27:0x5872a92da07f4066!8m2!3d39.1084897!4d-84.516297"}
    let missouriStateConvention = {Name = "Missouri State Convention"; Day = "Second Sunday and Saturday Before"; Month = 3; Location="St. John's United Church of Christ, Pinckney MO - Hwy 94, 11.5 miles west of Marthasville"; Latitude=38.668384; Longitude=(-91.237651); Info="Paul Figura"; LocationUrl="https://www.google.com/maps/place/St+Johns+Church/@38.667927,-91.2400762,16.75z/data=!4m5!3m4!1s0x0:0x9fa7bbd2711ebe46!8m2!3d38.6683807!4d-91.2376537?hl=en-US"}
    let bloomingtonAllDay = {Name = "Bloomington All-Day"; Day = "Saturday Before the Fourth Sunday"; Month = 10; Location="Fairview United Methodist Church, West 6th Street, Bloomington, IN"; Latitude=39.168000; Longitude=(-86.540055); Info="Bill Shetter or John Hoerr"; LocationUrl="https://www.google.com/maps/place/Fairview+United+Methodist+Church/@39.1680079,-86.5422328,17z/data=!3m1!4b1!4m5!3m4!1s0x886c6720650c091d:0x1e6342da4bdb09!8m2!3d39.1680079!4d-86.5400441"}
    let higherGround = {Name = "Higher Ground"; Day = "Saturday Before the Fourth Sunday"; Month = 3; Location="United Campus Ministries, 321 North 7th Street, Terre Haute IN"; Latitude=39.469710; Longitude=(-87.407011); Info="Darrell Swarens"; LocationUrl="https://www.google.com/maps/place/United+Campus+Ministries/@39.470135,-87.4088817,17z/data=!3m1!4b1!4m5!3m4!1s0x886d653a38eba0a1:0x8a8b532d9259bfaf!8m2!3d39.470135!4d-87.406693" }

    // to do: replace this with sql query
    let singings =
        [
            ohioStateConvention;
            missouriStateConvention;
            bloomingtonAllDay;
            higherGround;
        ]

        // to do: replace this with sql query
    let singingsInRange (startDate:DateTime) (endDate:DateTime) rangeInMiles = 
        singings 
        |> singingsWithinDateRange {Start=startDate; End=endDate}


