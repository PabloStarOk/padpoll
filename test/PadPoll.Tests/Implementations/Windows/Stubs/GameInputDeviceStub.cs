using System.Runtime.InteropServices.Marshalling;
using PadPoll.Implementations.Windows.GameInput.Enums;
using PadPoll.Implementations.Windows.GameInput.Interfaces;
using PadPoll.Implementations.Windows.GameInput.Structs;

namespace PadPoll.Tests.Implementations.Windows.Stubs;

/// <summary>
/// A stub implementation of the <see cref="IGameInputDevice"/> interface for testing purposes.
/// </summary>
[GeneratedComClass]
internal sealed partial class GameInputDeviceStub : IGameInputDevice
{
    /// <inheritdoc/>
    public unsafe int GetDeviceInfo(out GameInputDeviceInfo* info)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public int GetHapticInfo()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public uint GetDeviceStatus()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public int CreateForceFeedbackEffect()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public bool IsForceFeedbackMotorPoweredOn(uint motorIndex)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public void SetForceFeedbackMotorGain(uint motorIndex, float masterGain)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public void SetRumbleState()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public unsafe int DirectInputEscape(uint command, void* bufferIn, uint bufferInSize, void* bufferOut, uint bufferOutSize, uint* bufferOutSizeWritten)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public int CreateInputMapper()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public unsafe int GetExtraAxisCount(GameInputKind inputKind, uint* extraAxisCount)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public unsafe int GetExtraButtonCount(GameInputKind inputKind, uint* extraButtonCount)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public unsafe int GetExtraAxisIndexes(GameInputKind inputKind, uint extraAxisCount, byte* extraAxisIndexes)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public unsafe int GetExtraButtonIndexes(GameInputKind inputKind, uint extraButtonCount, byte* extraButtonIndexes)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public int CreateRawDeviceReport()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public int SendRawDeviceOutput()
    {
        throw new NotImplementedException();
    }
}