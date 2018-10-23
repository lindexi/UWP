using System;

namespace VarietyHiggstGushed.Model
{
    /// <summary>
    ///     责任链模式
    /// </summary>
    public class FjyhtrOcbhzjwi
    {
        public static FjyhtrOcbhzjwi Fhnazmoul { get; } = new FjyhtrOcbhzjwi();

        public bool Handle { get; set; }

        public void AddSuccessor(AjuvqrDqsoljna ajuvqrDqsoljna)
        {
            AjuvqrDqsoljna.AddSuccessor(ajuvqrDqsoljna);
        }

        public void RemoveSuccessor(AjuvqrDqsoljna ajuvqrDqsoljna)
        {
            AjuvqrDqsoljna.RemoveSuccessor(ajuvqrDqsoljna);
        }

        public void Request()
        {
            Handle = false;
            AjuvqrDqsoljna.Request(this);
        }

        private IHandle AjuvqrDqsoljna { get; } = new AjuvqrDqsoljna(fjyhtrOcbhzjwi => { });
    }

    public interface IHandle
    {
        IHandle Successor { set; get; }

        int Hnkdqckyr { get; }

        void Request(FjyhtrOcbhzjwi fjyhtrOcbhzjwi);

        void AddSuccessor(IHandle ajuvqrDqsoljna);
        void RemoveSuccessor(IHandle ajuvqrDqsoljna);
    }

    public class AjuvqrDqsoljna : IHandle
    {
        public AjuvqrDqsoljna(Action<FjyhtrOcbhzjwi> request)
        {
            Request = request;
        }

        public Action<FjyhtrOcbhzjwi> Request { get; }

        /// <summary>
        ///     权限
        /// </summary>
        public int Hnkdqckyr { set; get; }

        IHandle IHandle.Successor { get; set; }

        void IHandle.Request(FjyhtrOcbhzjwi fjyhtrOcbhzjwi)
        {
            Request.Invoke(fjyhtrOcbhzjwi);
            ((IHandle) this).Successor?.Request(fjyhtrOcbhzjwi);
        }

        void IHandle.AddSuccessor(IHandle successor)
        {
            if (((IHandle) this).Successor == null)
            {
                ((IHandle) this).Successor = successor;
            }
            else
            {
                if (successor.Hnkdqckyr < ((IHandle) this).Successor.Hnkdqckyr)
                {
                    ((IHandle) this).Successor.AddSuccessor(successor);
                }
                else
                {
                    var temp = ((IHandle) this).Successor;
                    ((IHandle) this).Successor = successor;
                    successor.Successor = temp;
                }
            }
        }

        void IHandle.RemoveSuccessor(IHandle ajuvqrDqsoljna)
        {
            if (ajuvqrDqsoljna == null)
            {
                return;
            }

            if (((IHandle) this).Successor == ajuvqrDqsoljna)
            {
                ((IHandle) this).Successor = ((IHandle) this).Successor.Successor;
            }
            else
            {
                ((IHandle) this).Successor?.RemoveSuccessor(ajuvqrDqsoljna);
            }
        }
    }
}