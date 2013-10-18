// Learn more about F# at http://fsharp.net. See the 'F# Tutorial' project
// for more guidance on F# programming.

#load "Interpolation.fs"
open NumericalAnalysis.Interpolation
open System

let interpolatedSin =
    seq { for i in 0..8 -> float i * Math.PI / 2.0 } 
        |> Seq.map (fun x -> (x, Math.Sin x)) 
        |> interpolate

seq { for i in 0..64 -> float i * Math.PI / 32.0 } 
    |> Seq.map (fun x -> (x, interpolatedSin x, Math.Sin x))
    |> Seq.iter (fun (x,x1,x2) -> printfn "I(%f) = %f | S(%f) = %f | |I - S| = %f" x x1 x x2 (Math.Abs (x1 - x2)))