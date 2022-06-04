using System;
namespace OSD.Gameplay {
    [Flags]
    public enum ColorType {
        Red = 1 << 0,
        Green = 1 << 1,
        Blue = 1 << 2
    }
}