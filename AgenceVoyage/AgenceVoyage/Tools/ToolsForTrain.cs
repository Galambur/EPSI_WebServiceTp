using AgenceVoyage.Models;
using System.Collections.Generic;
using System.Linq;

namespace AgenceVoyage.Tools
{
    public class ToolsForTrain
    {

        public static Train TrouverTrainEntreDeuxDestintation(tpagencevoyageContext model, Gare a, Gare b)
        {
            // SQL : https://docs.microsoft.com/fr-fr/dotnet/framework/data/adonet/sql/linq/formulate-joins-and-cross-product-queries
            var arTrainA =
                (from t in model.Train
                 join ta in model.Trainarret on t.IdTrain equals ta.IdTrain
                 where
                     ta.IdGare == a.IdGare
                 select ta).Select(x => x.IdTrain).ToList();

            var arTrainB =
                (from t in model.Train
                 join ta in model.Trainarret on t.IdTrain equals ta.IdTrain
                 where
                     ta.IdGare == b.IdGare
                 select ta).Select(x => x.IdTrain).ToList();

            int? arTrainAB = arTrainA.Where(A => arTrainB.Contains(A)).FirstOrDefault();
            //var trainAB = arTrainAB.GroupBy(x=> x.IdTrain).ToList();
            Train train;
            if (arTrainAB != null && arTrainAB != default(int?))
            {
                train = model.Train.SingleOrDefault(x => x.IdTrain == arTrainAB);
                if (train == default(Train))
                    return null;
                else
                    return train;
            }
            else
                return null;

        }
    }
}
