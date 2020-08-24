using System;
using BlazorDungeon.Code;

namespace BlazorDungeon.Service
{
    public delegate void ScreenChangeDelegate(object sender, ScreenChangeEventArgs args);

    public class ScreenChangeEventArgs : EventArgs
    {
        public Cell NewValue { get; }

        public ScreenChangeEventArgs(Cell newValue)
        {
            this.NewValue = newValue;
        }
    }

    public interface IScreenChangeBroadcastService : IDisposable
    {
        event ScreenChangeDelegate OnScreenChanged;
    }
}
