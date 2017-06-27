namespace SingingFinder.Core

open System
open System.Reflection
open System.Collections.Generic

open Microsoft.FSharp.Reflection

open Newtonsoft.Json

/// A converter for F# lists
type FlagConverter() =
  inherit JsonConverter()

  override x.CanConvert t = 
    t.IsGenericType
    && typeof<int>.Equals (t.GetGenericTypeDefinition())

  override x.WriteJson(writer, value, serializer) =
    serializer.Serialize(writer, value.ToString())

  override x.ReadJson(reader, t, arg, serializer) =
    ``base``.ReadJson(reader, t, arg, serializer)

type DateConverter() =
  inherit JsonConverter()

  override x.CanConvert t = 
    t.IsGenericType
    && typeof<DateTime>.Equals (t.GetGenericTypeDefinition())

  override x.WriteJson(writer, value, serializer) =
    let date = value :?> DateTime
    serializer.Serialize(writer, date.ToString("yyyy-MM-dd"))

  override x.ReadJson(reader, t, arg, serializer) =
    ``base``.ReadJson(reader, t, arg, serializer)
