using System.Collections.ObjectModel;

namespace TpwlxnpDfyecpeoh
{
    internal class KdlunmmHhrs : DexqurhctSjyfozae
    {
        public KdlunmmHhrs(ObservableCollection<DexqurhctSjyfozae> dexqurhctSjyfozae, IDfeppzyTmofs tdheituHnks)
        {
            DexqurhctSjyfozae = dexqurhctSjyfozae;
            TdheituHnks = tdheituHnks;

            HnukhltvKfdrpokjz = "点击添加技能";
            DfacHbl = new TeddtHlhkgt(tdheituHnks);
            DmyikbmfDeb = 100;
        }

        public override void DqqTsb()
        {
            DklvubnuiTeqch = DklvubnuiTeqch + 1;

            DexqurhctSjyfozae.Add(DfacHbl.StdshakHngld());
        }

        private ObservableCollection<DexqurhctSjyfozae> DexqurhctSjyfozae { get; }

        private IDfeppzyTmofs TdheituHnks { get; }

        private TeddtHlhkgt DfacHbl { get; }
    }
}