using CustomBPM.Core.MetaData;

namespace CustomBPM.Core.Interfaces
{
    public interface IMetadataProvider
    {
        Process GetProcess(string code);
    }
}
