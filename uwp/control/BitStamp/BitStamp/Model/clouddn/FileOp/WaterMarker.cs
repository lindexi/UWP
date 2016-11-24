// lindexi
// 16:34

using System.Threading.Tasks;
using lindexi.uwp.ImageShack.Model.RPC;

namespace lindexi.uwp.ImageShack.Model.FileOp
{
    public enum MarkerGravity
    {
        NorthWest = 0,
        North,
        NorthEast,
        West,
        Center,
        East,
        SouthWest,
        South,
        SouthEast
    }

    public class WaterMarker
    {
        public WaterMarker(int dissolve = 50, MarkerGravity gravity = MarkerGravity.SouthEast, int dx = 10, int dy = 10)
        {
            Dissolve = dissolve;
            this.dissolve = dissolve;
            this.dx = dx;
            this.dy = dy;
            this.gravity = gravity;
        }

        public int Dissolve
        {
            get
            {
                return dissolve;
            }
            set
            {
                if (value < 0)
                {
                    dissolve = 0;
                }
                else if (value > 100)
                {
                    dissolve = 100;
                }
                else
                {
                    dissolve = value;
                }
            }
        }

        public virtual string MakeRequest(string url)
        {
            return null;
        }

        public static async Task<ExifRet> Call(string url)
        {
            CallRet callRet = await FileOpClient.Get(url);
            return new ExifRet(callRet);
        }

        protected int dissolve;
        protected int dx;
        protected int dy;

        public MarkerGravity gravity;

        protected static string[] Gravitys = new string[9]
        {
            "NorthWest",
            "North",
            "NorthEast",
            "West",
            "Center",
            "East",
            "SouthWest",
            "South",
            "SouthEast"
        };
    }
}