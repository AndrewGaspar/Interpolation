namespace NumericalAnalysis

module Interpolation =
    let interpolate (dataPoints: seq<float * float>) =
        let data = dataPoints |> Seq.toArray
        let rec dividedDifference i k = 
            if i = k then (snd data.[i]);
            else 
                ((dividedDifference (i+1) k) - (dividedDifference i (k-1))) 
                    / (fst data.[k] - fst data.[i])

        let a n = dividedDifference 0 n
        
        let rec p n =
            if n = data.Length - 1 then (fun x -> a n)
            else (fun x -> (x - fst data.[n])*(p (n+1) x) + a n)

        p 0