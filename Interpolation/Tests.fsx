// Learn more about F# at http://fsharp.net. See the 'F# Tutorial' project
// for more guidance on F# programming.

#load "Interpolation.fs"
open NumericalAnalysis.Interpolation
open System

let interpolatedSin = interpolateFunc (0.0, 2.0*Math.PI) 16 Math.Sin

let testValues n =
    seq { 0..n }
        |> Seq.map (fun i -> float i * Math.PI / (float n / 2.0))
        |> Seq.map (fun x -> (x, interpolatedSin x, Math.Sin x))
        |> Seq.iter (fun (x,x1,x2) -> printfn "I(%f) = %f | S(%f) = %f | |I - S| = %f" x x1 x x2 (Math.Abs (x1 - x2)))

//[ [1.3; 0.62; -0.522]; [1.6; 0.455; -0.569] ]
//    |> Seq.map List.toSeq
//    |> hermiteCoefficients
//    |> Seq.iter (printfn "%f")

[ [-1.0; 2.0; -8.0; 56.0]; [0.0; 1.0; 0.0; 0.0]; [1.0; 2.0; 8.0; 56.0] ]
    |> Seq.map List.toSeq
    |> hermiteCoefficients
    |> Seq.iter (printfn "%f")