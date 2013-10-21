namespace NumericalAnalysis

module Interpolation =

    let slope (x1, fx1) (x2, fx2) = (fx1 - fx2) / (x1 - x2)

    let private getOrder (fArray: float [][]) =
        let order = fArray.[0].Length-1;
        
        if
            not (Array.forall (fun (arr: float[]) -> (arr.Length-1) = order) fArray)
        then raise (new System.ArgumentException("All internal arrays must have the same length"));

        order

    let private hermiteCoefficientsArr (fArray: float[][]) =
        let order = getOrder fArray

        // Get factorials for fast look up
        let facOrders = 
            seq { 1..order } 
            // Builds up each factorial based on previous one
            |> Seq.scan (fun prev i -> (float i) * prev) 1.0 
            |> Seq.toArray;

        let divDifLength = fArray.Length * order;

        let dividedDifferences: float option [] [] 
            =   Array.init (divDifLength)
                    (fun i -> Array.create (divDifLength-i) None);

        let rec dividedDifference i k = 
            let j = k-i;
            let divDif i k = 
                let x1 = fArray.[k/order].[0];
                let x2 = fArray.[i/order].[0];
                slope (x1, dividedDifference (i+1) k)
                    (x2, dividedDifference i (k-1))

            match dividedDifferences.[i].[j] with
            | Some(coefficient) -> coefficient
            | None ->
                dividedDifferences.[i].[j] <- Some (
                    if i/order = k/order then
                        fArray.[i/order].[j+1] / facOrders.[j]
                    else divDif i k)
                dividedDifferences.[i].[j].Value

        seq { 0..divDifLength-1 } 
            |> Seq.map (fun i -> dividedDifference 0 i)

    let private seqSeqToArrayArray (seq: seq<seq<float>>) =
        seq
            |> Seq.map Seq.toArray
            |> Seq.toArray

    let hermiteCoefficients (fSeq: seq<seq<float>>) =
        fSeq 
            |> seqSeqToArrayArray
            |> hermiteCoefficientsArr

    let private hermiteFundamentals (fSeq: seq<seq<float>>) =
        let data = fSeq |> seqSeqToArrayArray
        let order = getOrder data
        let coefficients = data |> hermiteCoefficientsArr |> Seq.toArray
        (data, order, coefficients)

    let hermite (fSeq: seq<seq<float>>) =
        let data, order, coefficients = fSeq |> hermiteFundamentals;
        
        let rec p n =
            if n = coefficients.Length - 1 then (fun x -> coefficients.[n])
            else (fun x -> (x - data.[n / order].[0])*(p (n+1) x) + coefficients.[n])

        p 0

    let hermiteEquation (fSeq: seq<seq<float>>) =
        let data, order, coefficients = fSeq |> hermiteFundamentals;
        
        let rec p n =
            if n = coefficients.Length - 1 then coefficients.[n].ToString()
            else sprintf "(%s)*(x - %f)+%f" (p (n+1)) data.[n / order].[0] coefficients.[n]

        p 0

    let interpolate (dataPoints: seq<float * float>) =
        dataPoints 
        |> Seq.map (fun (x,fx) -> seq { yield x; yield fx })
        |> hermite

    let interpolateFunc (lower, upper) intervals f =
        let range = upper-lower;
        let intervalSize = range / (float intervals);
        seq { 0..intervals }
            |> Seq.map (fun i -> lower + intervalSize*(float i))
            |> Seq.map (fun x -> (x, f x))
            |> interpolate